using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ServerSync;
using UnityEngine;
using HarmonyLib;

namespace OttoFuel
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class OttoFuelPlugin : BaseUnityPlugin
    {
        internal const string ModName = "OttoFuel";
        internal const string ModVersion = "1.5.3";
        // Author is the BepInEx GUID prefix and the Thunderstore namespace.
        internal const string Author = "potto007";
        internal const string Maintainer = "Paul Otto";
        // OttoFuel is a fork of AutomaticFuel by TastyChickenLegs.
        internal const string OriginalAuthor = "TastyChickenLegs";
        internal const string OriginalMod = "AutomaticFuel";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        private static readonly bool isDebug = false;

        public static ConfigEntry<float> dropRange;
        public static ConfigEntry<float> containerRange;
        public static ConfigEntry<float> fireplaceRange;
        public static ConfigEntry<float> smelterOreRange;
        public static ConfigEntry<float> smelterFuelRange;
        public static ConfigEntry<string> fuelDisallowTypes;
        public static ConfigEntry<string> oreDisallowTypes;
        public static ConfigEntry<KeyCode> toggleKeythree;
        public static string toggleString = "Auto Fuel: {0}";
        public static ConfigEntry<bool> refuelStandingTorches;
        public static ConfigEntry<bool> refuelWallTorches;
        public static ConfigEntry<bool> refuelFirePits;
        public static ConfigEntry<bool> blastFurnaceFix;
        public static ConfigEntry<bool> restrictKilnOutput;
        public static ConfigEntry<int> restrictKilnOutputAmount;

        public static ConfigEntry<bool> leaveLastItem;
        public static ConfigEntry<bool> isOn;
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> distributedFilling;
  
 
        public static ConfigEntry<bool> configStackSmelters;
        public static ConfigEntry<bool> configBlastFurnaceTakesAll;
        internal static readonly List<Container> ContainerList = new();
        public static ConfigEntry<float> mRange;
        public static ConfigEntry<bool> refuelBraziers;
        public static ConfigEntry<bool> turnOffKiln;
        public static ConfigEntry<bool> refuelHearth;
        public static ConfigEntry<bool> refuelOvens;
        public static ConfigEntry<float> ovenRange;
        public static ConfigEntry<bool> refuelHotTub;
        public static ConfigEntry<bool> turnOffWindmills;
        public static ConfigEntry<bool> turnOffSpinningWheel;
        public static ConfigEntry<bool> nofloorpickup;
        internal static string ConnectionError = "";
        public static ConfigEntry<bool> turnoffSmelter;
        public static ConfigEntry<bool> turnoffBlastFurnace;
 
        public static float lastFuel;
        public static int fuelCount;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource OttoFuelLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        /// <summary>
        /// Seeds the config file before BepInEx reads it.
        /// BaseUnityPlugin loads the config in its own constructor, so this work cannot
        /// wait for Awake. An explicit static constructor removes beforefieldinit, so the
        /// runtime must run this before it builds the first instance.
        /// </summary>
        static OttoFuelPlugin()
        {
            GameClasses.ConfigMigration.Prepare(Paths.ConfigPath, ConfigFileName, OttoFuelLogger.LogInfo);
        }

        public enum Toggle
        {
            On = 1,
            Off = 0
        }
        
        public void Awake()
        {

            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);
           
            dropRange = config("General", "DropRange", 15f,
                new ConfigDescription("The maximum range to pull dropped fuel",
                new AcceptableValueRange<float>(1f, 50f)));
            fireplaceRange = config("Fireplace", "FireplaceRange", 5f,
                 new ConfigDescription("The maximum range to pull fuel from containers for fireplaces",
                 new AcceptableValueRange<float>(1f, 50f)));
            smelterOreRange = config("Smelters", "SmelterOreRange", 15f,
                             new ConfigDescription("The maximum range to pull ore from containers for smelters",
                             new AcceptableValueRange<float>(1f, 50f)));
            smelterFuelRange = config("Smelters", "SmelterFuelRange", 15f,
                             new ConfigDescription("The maximum range to pull fuel from containers for smelters",
                             new AcceptableValueRange<float>(1f, 50f)));
            restrictKilnOutputAmount = config("Smelters", "RestrictKilnOutputAmount", 50,
                new ConfigDescription("Amount of coal to shut off kiln fueling max 1000",
                new AcceptableValueRange<int>(1, 1000)));


            

            configStackSmelters = config("Smelters", "AllowStackSmelters", false, "Allows smelters, kiln to be stacked.  Takes away the smoke and smoke blocked check");
            fuelDisallowTypes = config("Fireplace", "FuelDisallowTypes", "RoundLog,FineWood", "Types of item to disallow as fuel (i.e. anything that is consumed), comma-separated.");
            oreDisallowTypes = config("Smelters", "OreDisallowTypes", "RoundLog,FineWood", "Types of item to disallow as ore (i.e. anything that is transformed), comma-separated).");
            configBlastFurnaceTakesAll = config("Smelters", "BlastFurnaceTakesAll", true, "Allows the Blast Furnace to take all ore");
            //toggleString = config("General", "ToggleString", "Auto Fuel: {0}", "Text to show on toggle. {0} is replaced with true/false");

            //toggleKeyNew = config("General", "ToggleKey", new KeyboardShortcut(KeyCode.F10),
            //new ConfigDescription("HotKey to disable and enable AutoFuel", new AcceptableShortcuts()));

            toggleKeythree = config("General", "ToggleKey", KeyCode.F10, "Key to toggle behaviour. Leave blank to disable the toggle key. Use https://docs.unity3d.com/Manual/ConventionalGameInput.html");
            turnOffWindmills = config("Smelters", "Turn Off Windmills", false, "Turn off the Windmills");
            turnOffSpinningWheel = config("Smelters", "Turn Off SpinningWheel", false, "Turn off the Spinnng Wheel");
            turnOffKiln = config("Smelters", "Turn Off Kiln", false, "Turn off the Kiln");
            turnoffSmelter = config("Smelters", "Turn off Smelter", false, "Turn off the Smelter");
            turnoffBlastFurnace = config("Smelters", "Turn off Blast Furnace", false, "Turn off Blast Furnace");
            refuelStandingTorches = config("Fireplace", "RefuelStandingTorches", true, "Refuel standing torches");
            refuelBraziers = config("Fireplace", "RefuelBraziers", true, "Refuel Braziers");
            refuelHotTub = config("Fireplace", "RefuelHotTub", true, "Refuel HotTub");
            refuelWallTorches = config("Fireplace", "RefuelWallTorches", true, "Refuel wall torches");
            refuelFirePits = config("Fireplace", "RefuelFirePits", true, "Refuel fire pits");
            refuelHearth = config("Fireplace", "RefuelHearth", true, "Refuel Hearth");
            refuelOvens = config("Oven", "RefuelOvens", true, "Refuel the stone oven and any other cooking station that burns fuel");
            ovenRange = config("Oven", "OvenRange", 5f,
                new ConfigDescription("The maximum range to pull fuel from containers for ovens",
                new AcceptableValueRange<float>(1f, 50f)));
            restrictKilnOutput = config("Smelters", "RestrictKilnOutput", false, "Restrict kiln output");
            nofloorpickup = config("General", "Use Dropped Items for Fuel", true, "Use Dropped Items for Fuel");

            isOn = config("", "IsOn", true, "Behaviour is currently on or not");
            distributedFilling = config("Smelters", "DistributedFueling", true, "If true, refilling will occur one piece of fuel or ore at a time, making filling take longer but be better distributed between objects.");
            leaveLastItem = config("Smelters", "LeaveLastItem", false, "Don't use last of item in chest");
            modEnabled = config("", "Enabled", true, "Enable this mod");

            if (!modEnabled.Value)
                return;

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                OttoFuelLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                OttoFuelLogger.LogError($"There was an issue loading your {ConfigFileName}");
                OttoFuelLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }



        #endregion
        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug)
                Debug.Log((pref ? typeof(OttoFuelPlugin).Namespace + " " : "") + str);
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKeythree.Value) && !TastyUtils.IgnoreKeyPresses(true))
            {
                isOn.Value = !isOn.Value;
                Config.Save();
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, string.Format(toggleString, isOn.Value), 0, null, false);
            }
        }

        // Container Patches
        [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
        static class ContainerAwakePatch
        {
            static void Postfix(Container __instance, ZNetView ___m_nview)
            {

                TastyUtils.AddContainer(__instance, ___m_nview);
            }
        }

        [HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
        static class ContainerOnDestroyedPatch
        {
            static void Prefix(Container __instance)
            {
                ContainerList.Remove(__instance);
            }
        }

    }
 
}