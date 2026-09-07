using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class WagonConfiguration : BaseConfig
    {
        private const string Section = "Wagon";

        private ConfigEntry<float> wagonExtraMassFromItemsEntry;
        private ConfigEntry<float> wagonBaseMassEntry;

        public float wagonExtraMassFromItems => wagonExtraMassFromItemsEntry.Value;
        public float wagonBaseMass => wagonBaseMassEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            wagonExtraMassFromItemsEntry = Bind(config, Section, "wagonExtraMassFromItems", 0f,
                "This value changes the physical weight of wagons by +/- more/less from item weight inside.\nThe value 50 would increase the weight by 50% more. The value -100 would remove the entire extra weight.");
            wagonBaseMassEntry = Bind(config, Section, "wagonBaseMass", 20f,
                "Change the base wagon physical mass of the wagon object.\nThis is essentially the base weight of a cart.");
        }
    }
}
