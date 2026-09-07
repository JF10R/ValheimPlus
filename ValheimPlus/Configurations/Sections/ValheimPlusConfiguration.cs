using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ValheimPlusConfiguration : BaseConfig
    {
        private const string Section = "ValheimPlus";

        private ConfigEntry<bool> mainMenuLogoEntry;

        public bool mainMenuLogo => mainMenuLogoEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, true,
                "Change false to true to enable this section.");
            mainMenuLogoEntry = Bind(config, Section, "mainMenuLogo", true,
                "Display the Valheim Plus logo in the main menu");
        }
    }
}
