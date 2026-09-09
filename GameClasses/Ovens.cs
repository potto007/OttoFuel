using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace OttoFuel.GameClasses
{
    /// <summary>
    /// Refuels the stone oven and any other fuelled cooking station.
    /// A cooking station is a separate game class from a fireplace or a smelter, so it
    /// needs its own patch. The cooking spot burns no fuel and is skipped.
    /// </summary>
    internal class Oven_Patches
    {
        [HarmonyPatch(typeof(CookingStation), "UpdateFuel")]
        private static class CookingStation_UpdateFuel_Patch
        {
            private static void Postfix(CookingStation __instance, ZNetView ___m_nview)
            {
                if (!OttoFuelPlugin.refuelOvens.Value)
                    return;

                // UpdateFuel returns early on a station that burns no fuel. A postfix still
                // runs, so repeat the check here.
                if (!__instance.m_useFuel || __instance.m_fuelItem == null)
                    return;

                if (!Player.m_localPlayer || !OttoFuelPlugin.isOn.Value ||
                    ___m_nview == null || !___m_nview.IsOwner())
                    return;

                if (Time.time - OttoFuelPlugin.lastFuel < 0.1)
                {
                    OttoFuelPlugin.fuelCount++;
                    RefuelOven(__instance, ___m_nview, OttoFuelPlugin.fuelCount * 33);
                }
                else
                {
                    OttoFuelPlugin.fuelCount = 0;
                    OttoFuelPlugin.lastFuel = Time.time;
                    RefuelOven(__instance, ___m_nview, 0);
                }
            }
        }

        public static async void RefuelOven(CookingStation oven, ZNetView znview, int delay)
        {
            try
            {
                await Task.Delay(delay);

                if (!oven || !znview || !znview.IsValid() || !OttoFuelPlugin.modEnabled.Value)
                    return;

                string fuelName = oven.m_fuelItem.m_itemData.m_shared.m_name;
                int maxFuel = oven.m_maxFuel - Mathf.CeilToInt(znview.GetZDO().GetFloat("fuel", 0f));
                if (maxFuel <= 0)
                    return;

                List<Container> nearbyContainers = TastyUtils.GetNearbyContainers
                    (oven.transform.position, OttoFuelPlugin.ovenRange.Value);

                if (OttoFuelPlugin.nofloorpickup.Value)
                {
                    Vector3 position = oven.transform.position + Vector3.up;
                    foreach (Collider collider in Physics.OverlapSphere(position,
                        OttoFuelPlugin.dropRange.Value, LayerMask.GetMask(new string[] { "item" })))
                    {
                        if (!collider?.attachedRigidbody)
                            continue;

                        ItemDrop item = collider.attachedRigidbody.GetComponent<ItemDrop>();
                        if (item?.GetComponent<ZNetView>()?.IsValid() != true)
                            continue;

                        if (item.m_itemData.m_shared.m_name != fuelName || maxFuel <= 0)
                            continue;

                        string name = TastyUtils.GetPrefabName(item.gameObject.name);
                        if (OttoFuelPlugin.fuelDisallowTypes.Value.Split(',').Contains(name))
                            continue;

                        OttoFuelPlugin.Dbgl($"auto adding oven fuel {name} from ground");

                        int amount = Mathf.Min(item.m_itemData.m_stack, maxFuel);
                        maxFuel -= amount;

                        for (int i = 0; i < amount; i++)
                        {
                            if (item.m_itemData.m_stack <= 1)
                            {
                                if (znview.GetZDO() == null)
                                    OttoFuelPlugin.Destroy(item.gameObject);
                                else
                                    ZNetScene.instance.Destroy(item.gameObject);
                                znview.InvokeRPC("RPC_AddFuel", new object[] { });
                                if (OttoFuelPlugin.distributedFilling.Value)
                                    return;
                                break;
                            }

                            item.m_itemData.m_stack--;
                            znview.InvokeRPC("RPC_AddFuel", new object[] { });
                            Traverse.Create(item).Method("Save").GetValue();
                            if (OttoFuelPlugin.distributedFilling.Value)
                                return;
                        }
                    }
                }

                foreach (Container c in nearbyContainers)
                {
                    if (maxFuel <= 0)
                        break;

                    List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();
                    c.GetInventory().GetAllItems(fuelName, itemList);

                    foreach (var fuelItem in itemList)
                    {
                        if (fuelItem == null || maxFuel <= 0)
                            continue;
                        if (OttoFuelPlugin.leaveLastItem.Value && fuelItem.m_stack <= 1)
                            continue;
                        if (OttoFuelPlugin.fuelDisallowTypes.Value.Split(',').Contains(fuelItem.m_dropPrefab.name))
                            continue;

                        maxFuel--;

                        OttoFuelPlugin.Dbgl($"container at {c.transform.position} has {fuelItem.m_stack} {fuelItem.m_dropPrefab.name}, taking one for the oven");

                        znview.InvokeRPC("RPC_AddFuel", new object[] { });
                        TastyUtils.TakeOneFromContainer(c, fuelName);

                        if (OttoFuelPlugin.distributedFilling.Value)
                            return;
                    }
                }
            }
            catch { }
        }
    }
}
