using System;
using System.IO;
using System.Text;

namespace OttoFuel.GameClasses
{
    /// <summary>
    /// Seeds the OttoFuel config file before BepInEx reads it.
    /// OttoFuel uses a new plugin GUID, so it gets a new config file name. Without this
    /// step a player who upgrades from AutomaticFuel silently returns to stock settings.
    /// The setting that matters most is LeaveLastItem: at stock the mod empties a chest
    /// down to zero.
    /// </summary>
    internal static class ConfigMigration
    {
        // AutomaticFuel 1.4.8 renamed this key. Accept the older spelling too.
        private const string DroppedFuelKeyOld = "Use Dropped Items";
        private const string DroppedFuelKeyNew = "Use Dropped Items for Fuel";

        /// <summary>
        /// Runs before the plugin binds any config entry. It never overwrites an
        /// existing OttoFuel config.
        /// </summary>
        internal static void Prepare(string configDirectory, string targetFileName, Action<string> log)
        {
            try
            {
                if (string.IsNullOrEmpty(configDirectory) || !Directory.Exists(configDirectory))
                    return;

                string target = Path.Combine(configDirectory, targetFileName);
                if (File.Exists(target))
                    return;

                string legacy = FindLegacyConfig(configDirectory);
                if (legacy != null)
                {
                    MigrateFrom(legacy, target, log);
                    return;
                }

                WriteSaneDefaults(target, log);
            }
            catch (Exception ex)
            {
                // A config that fails to seed must never stop the mod from loading.
                // BepInEx then writes a stock file, which is the old behaviour.
                log($"Could not prepare the OttoFuel config: {ex.Message}");
            }
        }

        private static string? FindLegacyConfig(string configDirectory)
        {
            string exact = Path.Combine(configDirectory, "TastyChickenLegs.AutomaticFuel.cfg");
            if (File.Exists(exact))
                return exact;

            // Another distribution may use a different GUID prefix.
            string[] matches = Directory.GetFiles(configDirectory, "*AutomaticFuel.cfg");
            return matches.Length > 0 ? matches[0] : null;
        }

        private static void MigrateFrom(string legacy, string target, Action<string> log)
        {
            string[] lines = File.ReadAllLines(legacy);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("## Migrated from " + Path.GetFileName(legacy) + " by OttoFuel.");
            sb.AppendLine("## Your AutomaticFuel settings were carried over.");
            sb.AppendLine("## Settings added by OttoFuel, such as the Oven section, use their defaults.");
            sb.AppendLine();

            foreach (string line in lines)
            {
                string trimmed = line.TrimStart();

                // Drop the old comment block. BepInEx rewrites its own on the first save.
                if (trimmed.StartsWith("#", StringComparison.Ordinal))
                    continue;

                // Carry the renamed key across so dropped-item pickup keeps its value.
                if (trimmed.StartsWith(DroppedFuelKeyOld + " ", StringComparison.Ordinal) ||
                    trimmed.StartsWith(DroppedFuelKeyOld + "=", StringComparison.Ordinal))
                {
                    int eq = line.IndexOf('=');
                    if (eq >= 0)
                    {
                        sb.AppendLine(DroppedFuelKeyNew + " =" + line.Substring(eq + 1));
                        continue;
                    }
                }

                sb.AppendLine(line);
            }

            File.WriteAllText(target, sb.ToString());
            log($"Created {Path.GetFileName(target)} from {Path.GetFileName(legacy)}. " +
                "Your AutomaticFuel settings carried over.");
        }

        private static void WriteSaneDefaults(string target, Action<string> log)
        {
            // Only the values that differ from the stock defaults. BepInEx fills in the
            // rest and rewrites the file with its own comments on the first save.
            string contents =
                "## OttoFuel starter config, written on first run.\n" +
                "## Only the settings that differ from the stock defaults appear here.\n" +
                "## BepInEx adds every other setting the first time it saves.\n" +
                "\n" +
                "[Smelters]\n" +
                "\n" +
                "## true keeps the last item in a chest, so the mod never empties it.\n" +
                "LeaveLastItem = true\n";

            File.WriteAllText(target, contents.Replace("\n", Environment.NewLine));
            log($"No previous config found. Wrote {Path.GetFileName(target)} with OttoFuel defaults.");
        }
    }
}
