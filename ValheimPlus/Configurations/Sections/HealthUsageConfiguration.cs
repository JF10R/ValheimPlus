using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class HealthUsageConfiguration : BaseConfig
    {
        private const string Section = "HealthUsage";

        private ConfigEntry<float> bloodMagicEntry;

        public float bloodMagic => bloodMagicEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section. This section contains modifiers.\nModifiers are increases and reduction in percent declared by 50, or -50.");
            bloodMagicEntry = Bind(config, Section, "bloodMagic", 0f,
                "Each of these values change the respective tool in health usage by increases and reduction in percent declared by 50, or -50.");
        }
    }
}
