using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace OttoFuel.GameClasses
{
    /// <summary>
    /// Reloads ballistas with missiles from nearby chests and the ground.
    /// The ballista is the game's Turret class. It loads one missile per RPC_AddAmmo
    /// call, keyed by the missile's prefab name, and it holds one missile type at a
    /// time, so a partly loaded ballista only takes more of the type it already has.
    /// </summary>
    internal static class Turret_Patches
    {
        // FixedUpdate runs every physics tick. Check each ballista about once a second.
        private const float CheckInterval = 1f;

        private static readonly ConditionalWeakTable<Turret, StrongBox<float>> NextCheck = new();

        [HarmonyPatch(typeof(Turret), "FixedUpdate")]
        private static class Turret_FixedUpdate_Patch
        {
            private static void Postfix(Turret __instance, ZNetView ___m_nview)
            {
                if (!OttoFuelPlugin.modEnabled.Value || !OttoFuelPlugin.isOn.Value ||
                    !OttoFuelPlugin.reloadBallistas.Value || !Player.m_localPlayer ||
                    ___m_nview == null || !___m_nview.IsValid() || !___m_nview.IsOwner())
                    return;

                // A turret with fixed default ammo or no ammo cap never needs loading.
                if (__instance.m_maxAmmo <= 0 || __instance.m_defaultAmmo)
                    return;

                // Start each ballista at a random offset so they do not all scan on one tick.
                StrongBox<float> next = NextCheck.GetValue(__instance,
                    _ => new StrongBox<float>(Time.time + Random.Range(0f, CheckInterval)));
                if (Time.time < next.Value)
                    return;
                next.Value = Time.time + CheckInterval;

                try
                {
                    Reload(__instance, ___m_nview);
                }
                catch (System.Exception e)
                {
                    OttoFuelPlugin.OttoFuelLogger.LogDebug($"Ballista reload failed: {e}");
                }
            }
        }

        private static void Reload(Turret turret, ZNetView znview)
        {
            int room = turret.m_maxAmmo - turret.GetAmmo();
            if (room <= 0)
                return;

            // Prefab name of the loaded missile type, or null while the ballista is empty.
            string? loadedType = turret.GetAmmo() > 0 ? turret.GetAmmoType() : null;

            if (OttoFuelPlugin.nofloorpickup.Value)
            {
                Vector3 position = turret.transform.position + Vector3.up;
                foreach (Collider collider in Physics.OverlapSphere(position,
                    OttoFuelPlugin.dropRange.Value, LayerMask.GetMask("item")))
                {
                    if (room <= 0)
                        return;
                    if (!collider?.attachedRigidbody)
                        continue;

                    ItemDrop item = collider!.attachedRigidbody.GetComponent<ItemDrop>();
                    if (item?.GetComponent<ZNetView>()?.IsValid() != true)
                        continue;

                    string name = TastyUtils.GetPrefabName(item.gameObject.name);
                    if (!CanLoad(turret, name, loadedType))
                        continue;
                    if (!TastyUtils.ClaimDroppedItem(item))
                        continue;

                    int amount = Mathf.Min(item.m_itemData.m_stack, room);
                    for (int i = 0; i < amount; i++)
                    {
                        AddAmmo(znview, name);
                        room--;
                        if (item.m_itemData.m_stack <= 1)
                        {
                            ZNetScene.instance.Destroy(item.gameObject);
                            break;
                        }
                        item.m_itemData.m_stack--;
                        Traverse.Create(item).Method("Save").GetValue();
                    }
                    loadedType = name;
                    OttoFuelPlugin.Dbgl($"loaded {amount} {name} into ballista from ground");
                }
            }

            foreach (Container c in TastyUtils.GetNearbyContainers(turret.transform.position,
                OttoFuelPlugin.ballistaRange.Value))
            {
                foreach (Turret.AmmoType ammoType in turret.m_allowedAmmo)
                {
                    if (room <= 0)
                        return;
                    if (!ammoType.m_ammo)
                        continue;

                    string name = ammoType.m_ammo.name;
                    if (!CanLoad(turret, name, loadedType))
                        continue;

                    string sharedName = ammoType.m_ammo.m_itemData.m_shared.m_name;
                    Inventory inventory = c.GetInventory();
                    int available = inventory.CountItems(sharedName, -1, false);
                    if (OttoFuelPlugin.leaveLastItem.Value)
                        available--;

                    int amount = Mathf.Min(available, room);
                    for (int i = 0; i < amount; i++)
                    {
                        AddAmmo(znview, name);
                        TastyUtils.TakeOneFromContainer(c, sharedName);
                        room--;
                    }
                    if (amount > 0)
                    {
                        loadedType = name;
                        OttoFuelPlugin.Dbgl($"loaded {amount} {name} into ballista from container at {c.transform.position}");
                    }
                }
            }
        }

        /// <summary>
        /// True when the ballista accepts this missile prefab and it matches the type
        /// already loaded. Turret.IsItemAllowed is private, so this reads m_allowedAmmo.
        /// </summary>
        private static bool CanLoad(Turret turret, string prefabName, string? loadedType)
        {
            if (loadedType != null && prefabName != loadedType)
                return false;
            foreach (Turret.AmmoType ammoType in turret.m_allowedAmmo)
            {
                if (ammoType.m_ammo && ammoType.m_ammo.name == prefabName)
                    return true;
            }
            return false;
        }

        private static void AddAmmo(ZNetView znview, string prefabName)
        {
            znview.InvokeRPC("RPC_AddAmmo", prefabName);
        }
    }
}
