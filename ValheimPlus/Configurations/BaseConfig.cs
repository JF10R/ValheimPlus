using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;

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
}
