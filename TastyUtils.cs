using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

//Original Aedenthorn Utilities has been renamed to TastyUtils as I add my own custom items in the future
//Idea, code and credit go back to the original author
namespace OttoFuel
{
    public class TastyUtils
    {

        // Valheim 1.0 API shims.
        // Container.Save() and Inventory.Changed() are private, and Changed() gained
        // two parameters in 1.0. Resolve both once through Harmony AccessTools.
        private static readonly System.Reflection.MethodInfo? ContainerSaveMethod =
            AccessTools.Method(typeof(Container), "Save");

        private static readonly System.Reflection.MethodInfo? InventoryChangedMethod =
            AccessTools.Method(typeof(Inventory), "Changed",
                new[] { typeof(bool), typeof(bool) });

        // Smelter.m_blockedSmoke is private in 1.0. A publicized assembly is no longer
        // needed for it.
        private static readonly AccessTools.FieldRef<Smelter, bool>? SmelterBlockedSmoke =
            AccessTools.FieldRefAccess<Smelter, bool>("m_blockedSmoke");

        public static void SetSmelterBlockedSmoke(Smelter smelter, bool value)
        {
            if (smelter == null || SmelterBlockedSmoke == null) return;
            SmelterBlockedSmoke(smelter) = value;
        }

        /// <summary>
        /// Remove one named item from a container, then save the container and raise the
        /// inventory changed event.
        /// </summary>
        public static void TakeOneFromContainer(Container container, string itemName)
        {
            if (container == null) return;
            Inventory inventory = container.GetInventory();
            if (inventory == null) return;

            // 1.0 signature: RemoveItem(string name, int amount, int itemQuality, bool worldLevelBased).
            // Pass -1 and false to keep the pre-1.0 behaviour: any quality, any world level.
            inventory.RemoveItem(itemName, 1, -1, false);

            ContainerSaveMethod?.Invoke(container, new object[] { });
            InventoryChangedMethod?.Invoke(inventory, new object[] { true, false });
        }

        public static bool IgnoreKeyPresses(bool extra = false)
        {
            if (!extra)
                return ZNetScene.instance == null || Player.m_localPlayer == null || Minimap.IsOpen() || Console.IsVisible() || TextInput.IsVisible() ||
                    ZNet.instance.InPasswordDialog() || Chat.instance?.HasFocus() == true;
            return ZNetScene.instance == null || Player.m_localPlayer == null || Minimap.IsOpen() || Console.IsVisible() || TextInput.IsVisible() ||
                ZNet.instance.InPasswordDialog() || Chat.instance?.HasFocus() == true || StoreGui.IsVisible() || InventoryGui.IsVisible() || Menu.IsVisible() ||
                TextViewer.instance?.IsVisible() == true;
        }

        public static bool CheckKeyDown(string value)
        {
            try
            {
                return Input.GetKeyDown(value.ToLower());
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckKeyHeld(string value, bool req = true)
        {
            try
            {
                return Input.GetKey(value.ToLower());
            }
            catch
            {
                return !req;
            }
        }

        public static string GetPrefabName(string name)
        {
            char[] anyOf = new char[] { '(', ' ' };
            int num = name.IndexOfAny(anyOf);
            string result;
            if (num >= 0)
                result = name.Substring(0, num);
            else
                result = name;
            return result;
        }

        public static void AddContainer(Container container, ZNetView nview)
        {
            try
            {
                OttoFuelPlugin.OttoFuelLogger.LogDebug(
                    $"Checking {container.name} {nview != null} {nview?.GetZDO() != null} {nview?.GetZDO()?.GetLong("creator".GetStableHashCode())}");
                if (container.GetInventory() == null || nview?.GetZDO() == null ||
                    (!container.name.StartsWith("piece_", StringComparison.Ordinal) &&
                     !container.name.StartsWith("Container", StringComparison.Ordinal) &&
                     nview.GetZDO().GetLong("creator".GetStableHashCode()) == 0)) return;
                OttoFuelPlugin.OttoFuelLogger.LogDebug($"Adding {container.name}");
                OttoFuelPlugin.ContainerList.Add(container);
            }
            catch
            {
                // ignored
            }
        }

        public static List<Container> GetNearbyContainers(Vector3 center, float range)
        {
            List<Container> containers = new();
            foreach (Container container in OttoFuelPlugin.ContainerList.Where(container => container != null &&
                         container.GetComponentInParent<Piece>() != null && Player.m_localPlayer != null &&
                         container?.transform != null && container.GetInventory() != null && (range <= 0 ||
                             Vector3.Distance(center, container.transform.position) <
                             range) && container.CheckAccess(Player.m_localPlayer.GetPlayerID()) &&
                         !container.IsInUse()))
            {
                var containerPos = container.transform.position;

                if (!PrivateArea.CheckAccess(containerPos, 0f, false))
                    continue;
                if (!PrivateArea.InsideFactionArea(container.transform.position, Character.Faction.Players))
                {
                    container.Load();
                    containers.Add(container);
                   // OttoFuelPlugin.OttoFuelLogger.LogMessage(container);
                    continue;
                }
                else
                {
                    container.Load();
                    containers.Add(container);
                }
            }
            return containers;
        }
    }
}