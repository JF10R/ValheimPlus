using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class StaminaUsageConfiguration : BaseConfig
    {
        private const string Section = "StaminaUsage";

        private ConfigEntry<float> axesEntry;
        private ConfigEntry<float> bowsEntry;
        private ConfigEntry<float> blockingEntry;
        private ConfigEntry<float> clubsEntry;
        private ConfigEntry<float> knivesEntry;
        private ConfigEntry<float> pickaxesEntry;
        private ConfigEntry<float> polearmsEntry;
        private ConfigEntry<float> spearsEntry;
        private ConfigEntry<float> swordsEntry;
        private ConfigEntry<float> unarmedEntry;
        private ConfigEntry<float> hammerEntry;
        private ConfigEntry<float> hoeEntry;
        private ConfigEntry<float> cultivatorEntry;
        private ConfigEntry<float> fishingEntry;

        public float axes => axesEntry.Value;
        public float bows => bowsEntry.Value;
        public float blocking => blockingEntry.Value;
        public float clubs => clubsEntry.Value;
        public float knives => knivesEntry.Value;
        public float pickaxes => pickaxesEntry.Value;
        public float polearms => polearmsEntry.Value;
        public float spears => spearsEntry.Value;
        public float swords => swordsEntry.Value;
        public float unarmed => unarmedEntry.Value;
        public float hammer => hammerEntry.Value;
        public float hoe => hoeEntry.Value;
        public float cultivator => cultivatorEntry.Value;
        public float fishing => fishingEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section. This section contains modifiers.\nModifiers are increases and reduction in percent declared by 50, or -50.");
            axesEntry = Bind(config, Section, "axes", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            bowsEntry = Bind(config, Section, "bows", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            blockingEntry = Bind(config, Section, "blocking", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            clubsEntry = Bind(config, Section, "clubs", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            knivesEntry = Bind(config, Section, "knives", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            pickaxesEntry = Bind(config, Section, "pickaxes", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            polearmsEntry = Bind(config, Section, "polearms", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            spearsEntry = Bind(config, Section, "spears", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            swordsEntry = Bind(config, Section, "swords", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            unarmedEntry = Bind(config, Section, "unarmed", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            hammerEntry = Bind(config, Section, "hammer", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            hoeEntry = Bind(config, Section, "hoe", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            cultivatorEntry = Bind(config, Section, "cultivator", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
            fishingEntry = Bind(config, Section, "fishing", 0f,
                "Each of these values change the respective tool in stamina usage by increases and reduction in percent declared by 50, or -50.");
        }
    }
}
