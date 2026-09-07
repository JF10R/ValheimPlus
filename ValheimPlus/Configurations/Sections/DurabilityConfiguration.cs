using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class DurabilityConfiguration : BaseConfig
    {
        private const string Section = "Durability";

        private ConfigEntry<float> axesEntry;
        private ConfigEntry<float> pickaxesEntry;
        private ConfigEntry<float> hammerEntry;
        private ConfigEntry<float> cultivatorEntry;
        private ConfigEntry<float> hoeEntry;
        private ConfigEntry<float> weaponsEntry;
        private ConfigEntry<float> armorEntry;
        private ConfigEntry<float> bowsEntry;
        private ConfigEntry<float> shieldsEntry;
        private ConfigEntry<float> torchEntry;

        public float axes => axesEntry.Value;
        public float pickaxes => pickaxesEntry.Value;
        public float hammer => hammerEntry.Value;
        public float cultivator => cultivatorEntry.Value;
        public float hoe => hoeEntry.Value;
        public float weapons => weaponsEntry.Value;
        public float armor => armorEntry.Value;
        public float bows => bowsEntry.Value;
        public float shields => shieldsEntry.Value;
        public float torch => torchEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section. This section contains modifiers.\nModifiers are increases and reduction in percent declared by 50, or -50.");
            axesEntry = Bind(config, Section, "axes", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            pickaxesEntry = Bind(config, Section, "pickaxes", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            hammerEntry = Bind(config, Section, "hammer", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            cultivatorEntry = Bind(config, Section, "cultivator", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            hoeEntry = Bind(config, Section, "hoe", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            weaponsEntry = Bind(config, Section, "weapons", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            armorEntry = Bind(config, Section, "armor", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            bowsEntry = Bind(config, Section, "bows", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            shieldsEntry = Bind(config, Section, "shields", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
            torchEntry = Bind(config, Section, "torch", 0f,
                "Each of these values increase or reduce the durability of the specific item type by %.\nThe value 50 will increase the durability from 100 to 150. The value -50 will reduce the durability from 100 to 50.");
        }
    }
}
