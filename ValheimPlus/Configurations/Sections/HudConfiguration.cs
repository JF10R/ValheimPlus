using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class HudConfiguration : BaseConfig
    {
        private const string Section = "Hud";

        private ConfigEntry<bool> showRequiredItemsEntry;
        private ConfigEntry<bool> experienceGainedNotificationsEntry;
        private ConfigEntry<bool> removeDamageFlashEntry;
        private ConfigEntry<int> displayBowAmmoCountsEntry;

        public bool showRequiredItems => showRequiredItemsEntry.Value;
        public bool experienceGainedNotifications => experienceGainedNotificationsEntry.Value;
        public bool removeDamageFlash => removeDamageFlashEntry.Value;
        public int displayBowAmmoCounts => displayBowAmmoCountsEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            showRequiredItemsEntry = Bind(config, Section, "showRequiredItems", false,
                "Shows the required amount of items AND the amount of items in your inventory in build mode and while crafting.\nThis is enabled when the CraftFromChest section is enabled.");
            experienceGainedNotificationsEntry = Bind(config, Section, "experienceGainedNotifications", false,
                "Shows small notifications about all skill experienced gained in the top left corner.");
            removeDamageFlashEntry = Bind(config, Section, "removeDamageFlash", false,
                "Set to true to remove the red screen flash overlay when the player takes damage.");
            displayBowAmmoCountsEntry = Bind(config, Section, "displayBowAmmoCounts", 0,
                "If bow is in hotbar, display current ammo & total ammo under hotbar icon - never (0), when equipped (1), or always (2).");
        }
    }
}
