using BepInEx.Configuration;
using ServerSync;
using System;

namespace ValheimPlus.Configurations
{
    /// <summary>
    /// Wraps the ServerSync library, which pushes a server's config entries to its clients and puts
    /// each client's own values back on disconnect.
    /// </summary>
    internal static class ConfigSyncGlue
    {
        private static ConfigSync configSync;

        /// <summary>Raised when this client starts or stops taking its values from a server.</summary>
        public static event Action<bool> SourceOfTruthChanged;

        public static void Initialize(string guid, string displayName, string version, string minimumVersion)
        {
            configSync = new ConfigSync(guid)
            {
                DisplayName = displayName,
                CurrentVersion = version,
                MinimumRequiredVersion = minimumVersion,
            };

            configSync.SourceOfTruthChanged += value => SourceOfTruthChanged?.Invoke(value);
        }

        public static void SetModRequired(bool required)
        {
            if (configSync != null) configSync.ModRequired = required;
        }

        /// <summary>
        /// Registers an entry for syncing. <paramref name="synchronized"/> false keeps the entry in
        /// ServerSync's bookkeeping but leaves this client's own value in effect.
        /// </summary>
        public static void Register<T>(ConfigEntry<T> entry, bool synchronized)
        {
            if (configSync == null) return;
            configSync.AddConfigEntry(entry).SynchronizedConfig = synchronized;
        }

        /// <summary>
        /// Registers the entry deciding whether non-admins may change synced settings while connected.
        /// May only be called once.
        /// </summary>
        public static void RegisterLocking(ConfigEntry<bool> entry)
        {
            if (configSync != null) configSync.AddLockingConfigEntry(entry);
        }
    }
}
