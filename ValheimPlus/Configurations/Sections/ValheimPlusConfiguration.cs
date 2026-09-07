using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ValheimPlusConfiguration : BaseConfig
    {
        private const string Section = "ValheimPlus";

        private ConfigEntry<bool> mainMenuLogoEntry;
        private ConfigEntry<bool> disableConfigAutoUpdatesEntry;

        public bool mainMenuLogo => mainMenuLogoEntry.Value;
        public bool disableConfigAutoUpdates => disableConfigAutoUpdatesEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, true,
                "Change false to true to enable this section.");
            mainMenuLogoEntry = Bind(config, Section, "mainMenuLogo", true,
                "Display the Valheim Plus logo in the main menu");
            disableConfigAutoUpdatesEntry = Bind(config, Section, "disableConfigAutoUpdates", false,
                "Disables configuration file auto updates from GitHub, useful if GitHub is blocked on your network.");
        }
    }
}
