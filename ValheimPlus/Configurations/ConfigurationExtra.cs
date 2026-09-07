using BepInEx;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ValheimPlus.Configurations
{
    public class ConfigurationExtra
    {
        /// <summary>
        /// The pre-BepInEx config file. Only still referenced so its values can be imported once and
        /// the file then set aside; nothing reads it after that.
        /// </summary>
        public static string ConfigIniPath = Path.GetDirectoryName(Paths.BepInExConfigPath) +
                                             Path.DirectorySeparatorChar + "valheim_plus.cfg";
    }

    public static class IniDataExtensions
    {
        public static float GetFloat(this KeyDataCollection data, string key, float defaultVal)
        {
            if (float.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result))
            {
                return result;
            }

            ValheimPlusPlugin.Logger.LogWarning($" [Float] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static bool GetBool(this KeyDataCollection data, string key)
        {
            var truevals = new[] { "y", "yes", "true", "1", "enabled" };
            return truevals.Contains($"{data[key]}".ToLower());
        }

        public static int GetInt(this KeyDataCollection data, string key, int defaultVal)
        {
            if (int.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result))
            {
                return result;
            }

            ValheimPlusPlugin.Logger.LogWarning($" [Int] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static object GetEnumValue(this KeyDataCollection data, string key, object defaultVal)
        {
            var enumType = defaultVal.GetType();
            try
            {
                return Enum.Parse(enumType, data[key], true);
            }
            catch
            {
                ValheimPlusPlugin.Logger.LogWarning(
                    $" [{enumType}] Could not read {key}, using default value of {defaultVal}");
                return defaultVal;
            }
        }

        public static object GetFlags(this KeyDataCollection data, string key, object defaultVal)
        {
            var enumType = defaultVal.GetType();
            var flags = new List<object>();

            var values = data[key].Split(',').ToList();
            values.ForEach(x => x.Trim());

            foreach (var opt in values)
            {
                try
                {
                    var flag = Enum.Parse(enumType, opt, true);
                    flags.Add(flag);
                }
                catch
                {
                    ValheimPlusPlugin.Logger.LogWarning($" [{enumType.Name}] Unrecognized value `{opt}` in {key}");
                }
            }

            var value = flags.Aggregate(0, (current, flag) => current | (int)flag);
            try
            {
                return Enum.ToObject(enumType, value);
            }
            catch
            {
                ValheimPlusPlugin.Logger.LogWarning(
                    $" [{enumType}] Could not read {key}, using default value of {defaultVal}");
                return defaultVal;
            }
        }

        public static KeyCode GetKeyCode(this KeyDataCollection data, string key, KeyCode defaultVal)
        {
            if (Enum.TryParse<KeyCode>(data[key].Trim(), out var result))
            {
                return result;
            }

            ValheimPlusPlugin.Logger.LogWarning($" [KeyCode] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }
    }
}
