using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using ServerSync;
using System;
using ValheimPlus.Utility;

namespace ValheimPlus.Configurations
{
    /// <summary>
    /// Wraps the ServerSync library, which pushes a server's config entries to its clients and puts
    /// each client's own values back on disconnect.
    /// </summary>
    internal static class ConfigSyncGlue
    {
        /// <summary>The ServerSync method that applies a received config package.</summary>
        private const string HandleRpcName = "HandleConfigSyncRPC";

        /// <summary>
        /// Own instance, so reapplying the mod's patches does not tear out the hook asking for it.
        /// </summary>
        private static readonly Harmony Harmony = new("mod.valheim_plus.serversync");

        private static ConfigSync configSync;

        /// <summary>Raised when this client starts or stops taking its values from a server.</summary>
        public static event Action<bool> SourceOfTruthChanged;

        /// <summary>Raised once a received config package has been applied.</summary>
        public static event Action ConfigApplied;

        public static void Initialize(string guid, string displayName, string version, string minimumVersion)
        {
            configSync = new ConfigSync(guid)
            {
                DisplayName = displayName,
                CurrentVersion = version,
                MinimumRequiredVersion = minimumVersion,
            };

            configSync.SourceOfTruthChanged += value => SourceOfTruthChanged?.Invoke(value);

            HookConfigApplied();
        }

        /// <summary>
        /// ServerSync announces the server as the source of truth before it applies the package it
        /// just received, so anything rebuilt from that event still reads this client's own values.
        /// The method doing the applying lands after they are in, and also covers a later broadcast
        /// and an admin pushing a change back to the server.
        /// </summary>
        private static void HookConfigApplied()
        {
            try
            {
                var original = AccessTools.DeclaredMethod(typeof(ConfigSync), HandleRpcName)
                    ?? throw new MissingMethodException(nameof(ConfigSync), HandleRpcName);

                Harmony.Patch(original, postfix: new HarmonyMethod(AccessTools.DeclaredMethod(
                    typeof(ConfigSyncGlue), nameof(ConfigAppliedPostfix))));
            }
            catch (Exception e)
            {
                PatchLog.Failed(nameof(HookConfigApplied),
                    "A server's settings will not take effect until the game is restarted.", e);
            }
        }

        /// <summary>The result is false while the package is still arriving in fragments.</summary>
        [UsedImplicitly]
        private static void ConfigAppliedPostfix(bool __result)
        {
            if (__result) ConfigApplied?.Invoke();
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
