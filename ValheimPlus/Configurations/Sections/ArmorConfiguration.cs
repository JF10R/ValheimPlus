using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ArmorConfiguration : BaseConfig
    {
        private const string Section = "Armor";

        private ConfigEntry<float> helmetsEntry;
        private ConfigEntry<float> chestsEntry;
        private ConfigEntry<float> legsEntry;
        private ConfigEntry<float> capesEntry;

        public float helmets => helmetsEntry.Value;
        public float chests => chestsEntry.Value;
        public float legs => legsEntry.Value;
        public float capes => capesEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section. This section contains modifiers.\nModifiers are increases and reduction in percent declared by 50, or -50.");
            helmetsEntry = Bind(config, Section, "helmets", 0f,
                "Each of these values increase or reduce the armor of the specific item type by %.\nThe value 50 will increase the armor from 14 to 21. The value -50 will reduce the armor from 14 to 7.");
            chestsEntry = Bind(config, Section, "chests", 0f,
                "Each of these values increase or reduce the armor of the specific item type by %.\nThe value 50 will increase the armor from 14 to 21. The value -50 will reduce the armor from 14 to 7.");
            legsEntry = Bind(config, Section, "legs", 0f,
                "Each of these values increase or reduce the armor of the specific item type by %.\nThe value 50 will increase the armor from 14 to 21. The value -50 will reduce the armor from 14 to 7.");
            capesEntry = Bind(config, Section, "capes", 0f,
                "Each of these values increase or reduce the armor of the specific item type by %.\nThe value 50 will increase the armor from 14 to 21. The value -50 will reduce the armor from 14 to 7.");
        }
    }
}
