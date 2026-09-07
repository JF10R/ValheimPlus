using BepInEx.Configuration;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimPlus.RPC;

namespace ValheimPlus.Configurations
{
    /// <summary>
    /// Base for a config section backed by BepInEx <see cref="ConfigEntry{T}"/> objects.
    /// </summary>
    public abstract class BaseConfig
    {
        /// <summary>Configuration Manager tag shared by this section's settings.</summary>
        private readonly ConfigurationManagerAttributes attributes = new();

        /// <summary>Tag for "enabled", ordered to the top of its section.</summary>
        private readonly ConfigurationManagerAttributes enabledAttributes = new() { Order = int.MaxValue };

        /// <summary>One call per entry, run when this section is handed to ServerSync.</summary>
        private readonly List<Action<bool>> syncRegistrations = new();

        private string sectionName;
        private ConfigEntry<bool> enabledEntry;

        /// <summary>Whether the user turned this section on. False until <see cref="Bind"/> has run.</summary>
        public bool IsEnabled => enabledEntry?.Value ?? false;

        /// <summary>Declares this section's config entries, <see cref="BindEnabled"/> first.</summary>
        public abstract void Bind(ConfigFile config);

        /// <summary>Binds the section's "enabled" key, which backs <see cref="IsEnabled"/>.</summary>
        protected void BindEnabled(ConfigFile config, string section, bool defaultValue, string description)
        {
            sectionName = section;
            enabledEntry = config.Bind(section, "enabled", defaultValue,
                new ConfigDescription(description, null, enabledAttributes));
            syncRegistrations.Add(_ => ConfigSyncGlue.Register(enabledEntry, true));
        }

        /// <summary>Binds a setting, which a server pushes to its clients like any other.</summary>
        protected ConfigEntry<T> Bind<T>(
            ConfigFile config, string section, string key, T defaultValue, string description)
        {
            return Bind(config, section, key, defaultValue, description, local: false);
        }

        /// <summary>Binds a setting that is registered with ServerSync but never takes its value.</summary>
        protected ConfigEntry<T> BindLocal<T>(
            ConfigFile config, string section, string key, T defaultValue, string description)
        {
            return Bind(config, section, key, defaultValue, description, local: true);
        }

        private ConfigEntry<T> Bind<T>(ConfigFile config, string section, string key, T defaultValue,
            string description, bool local)
        {
            sectionName = section;
            var entry = config.Bind(section, key, defaultValue,
                new ConfigDescription(description, null, attributes));

            // Taking a server's hotkeys is the client's call.
            syncRegistrations.Add(syncHotkeys => ConfigSyncGlue.Register(entry,
                !local && (syncHotkeys || typeof(T) != typeof(KeyCode))));

            return entry;
        }

        /// <summary>Hands every entry in this section to ServerSync. Returns how many were registered.</summary>
        internal int RegisterForServerSync(bool syncHotkeys)
        {
            foreach (var register in syncRegistrations) register(syncHotkeys);
            return syncRegistrations.Count;
        }

        /// <summary>
        /// Locks or unlocks this section in Configuration Manager, <paramref name="lockNote"/> saying
        /// why in the category header. A null category falls back to the plain section name.
        /// </summary>
        internal void SetEditable(bool editable, string lockNote)
        {
            attributes.ReadOnly = !editable;
            enabledAttributes.ReadOnly = !editable;
            attributes.Category = editable ? null : $"{sectionName} ({lockNote})";
            enabledAttributes.Category = attributes.Category;
        }
    }
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data, string section);
    }

    public abstract class BaseConfig<T> : IConfig where T : class, IConfig, new()
    {
        [LoadingOption(LoadingMode.Never)] public bool IsEnabled { get; set; } = false;
        [LoadingOption(LoadingMode.Never)] public virtual bool NeedsServerSync { get; set; } = false;

        public static IniData iniUpdated = null;

        public static T LoadIni(IniData data, string section, bool verbose)
        {
            var n = new T();

            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                if (verbose)
                {
                    ValheimPlusPlugin.Logger.LogInfo($"[{section}] Section is NOT enabled.");
                    ValheimPlusPlugin.Logger.LogInfo("");
                }

                return n;
            }
            else if (verbose)
            {
                ValheimPlusPlugin.Logger.LogInfo($"[{section}] Section is enabled.");
            }

            var keyData = data[section];
            n.LoadIniData(keyData, section);

            if (verbose)
            {
                ValheimPlusPlugin.Logger.LogInfo($"[{section}] Done with section.");
                ValheimPlusPlugin.Logger.LogInfo("");
            }

            return n;
        }

        private static Dictionary<Type, DGetDataValue> _getValues = new Dictionary<Type, DGetDataValue>()
        {
            { typeof(float), GetFloatValue },
            { typeof(int), GetIntValue },
            { typeof(KeyCode), GetKeyCodeValue },
            { typeof(bool), GetBoolValue },
            { typeof(string), GetStringValue },
            { typeof(Enum), GetEnumValue }
        };

        public void LoadIniData(KeyDataCollection data, string section)
        {
            IsEnabled = true;
            var thisConfiguration = GetCurrentConfiguration(section);
            if (thisConfiguration == null)
            {
                thisConfiguration = this as T;
                if (thisConfiguration == null)
                    ValheimPlusPlugin.Logger.LogInfo("[{section}] Error on setting Configuration");
            }

            foreach (var property in typeof(T).GetProperties())
            {
                if (IgnoreLoading(property))
                {
                    continue;
                }

                var currentValue = property.GetValue(thisConfiguration);
                if (LoadLocalOnly(property))
                {
                    property.SetValue(this, currentValue, null);
                    continue;
                }

                var keyName = GetKeyNameFromProperty(property);

                if (!data.ContainsKey(keyName))
                {
                    ValheimPlusPlugin.Logger.LogInfo($"[{section}] Key {keyName} not defined, using default value");
                    continue;
                }

                var propertyType = property.PropertyType;
                if (propertyType.IsEnum) 
                    propertyType = typeof(Enum);

                if (_getValues.TryGetValue(propertyType, out var getValue))
                {
                    var value = getValue(data, currentValue, keyName);
                    if (!currentValue.Equals(value))
                        ValheimPlusPlugin.Logger.LogInfo(
                            $"[{section}] Updating {keyName} from {currentValue} to {value}");
                    property.SetValue(this, value, null);
                }
                else
                {
                    ValheimPlusPlugin.Logger.LogWarning(
                        $"[{section}] Could not load data of type {propertyType} for key {keyName}");
                }
            }
        }

        delegate object DGetDataValue(KeyDataCollection data, object currentValue, string keyName);

        private static object GetFloatValue(KeyDataCollection data, object currentValue, string keyName)
        {
            return data.GetFloat(keyName, (float)currentValue);
        }

        private static object GetBoolValue(KeyDataCollection data, object currentValue, string keyName)
        {
            return data.GetBool(keyName);
        }

        private static object GetStringValue(KeyDataCollection data, object currentValue, string keyName) => 
            data[keyName];

        private static object GetEnumValue(KeyDataCollection data, object currentValue, string keyName)
        {
            var enumType = currentValue.GetType();
            var isFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);
            return isFlagEnum
                ? data.GetFlags(keyName, currentValue)
                : data.GetEnumValue(keyName, currentValue);
        }

        private static object GetIntValue(KeyDataCollection data, object currentValue, string keyName)
        {
            return data.GetInt(keyName, (int)currentValue);
        }

        private static object GetKeyCodeValue(KeyDataCollection data, object currentValue, string keyName)
        {
            return data.GetKeyCode(keyName, (KeyCode)currentValue);
        }

        private string GetKeyNameFromProperty(PropertyInfo property)
        {
            var keyName = property.Name;

            // Set first char of keyName to lowercase
            if (keyName != string.Empty && char.IsUpper(keyName[0]))
            {
                keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
            }

            return keyName;
        }

        private bool IgnoreLoading(PropertyInfo property)
        {
            var loadingOption = property.GetCustomAttribute<LoadingOption>();
            var loadingMode = loadingOption?.LoadingMode ?? LoadingMode.Always;

            return (loadingMode == LoadingMode.Never);
        }

        private bool LoadLocalOnly(PropertyInfo property)
        {
            var loadingOption = property.GetCustomAttribute<LoadingOption>();
            var loadingMode = loadingOption?.LoadingMode ?? LoadingMode.Always;

            return VPlusConfigSync.SyncRemote &&
                   (property.PropertyType == typeof(KeyCode) && !ConfigurationExtra.SyncHotkeys ||
                    loadingMode == LoadingMode.LocalOnly);
        }

        private static object GetCurrentConfiguration(string section)
        {
            if (Configuration.Current == null) return null;
            var properties = Configuration.Current.GetType().GetProperties();
            PropertyInfo property = properties.SingleOrDefault(p =>
                p.Name.Equals(section, System.StringComparison.CurrentCultureIgnoreCase));
            if (property == null)
            {
                ValheimPlusPlugin.Logger.LogWarning($"Property '{section}' not found in Configuration");
                return null;
            }

            var thisConfiguration = property.GetValue(Configuration.Current) as T;
            return thisConfiguration;
        }
    }

    public abstract class ServerSyncConfig<T> : BaseConfig<T> where T : class, IConfig, new()
    {
        [LoadingOption(LoadingMode.Never)] public override bool NeedsServerSync { get; set; } = true;
    }

    public class LoadingOption : Attribute
    {
        public LoadingMode LoadingMode { get; }

        public LoadingOption(LoadingMode loadingMode)
        {
            LoadingMode = loadingMode;
        }
    }

    /// <summary>
    /// Defines, when a property is loaded
    /// </summary>
    public enum LoadingMode
    {
        Always = 0,
        RemoteOnly = 1,
        LocalOnly = 2,
        Never = 3
    }
}
