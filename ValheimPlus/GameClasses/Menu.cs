using HarmonyLib;
using JetBrains.Annotations;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Report an open settings window to the game as a menu.
    /// </summary>
    [HarmonyPatch(typeof(Menu), nameof(Menu.IsVisible))]
    public static class Menu_IsVisible_Patch
    {
        [UsedImplicitly]
        private static void Postfix(ref bool __result)
        {
            __result |= ConfigurationManagerWatcher.BlocksGameInput;
        }
    }
}
