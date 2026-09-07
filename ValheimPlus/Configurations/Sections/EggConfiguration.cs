using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class EggConfiguration : BaseConfig
    {
        private const string Section = "Egg";

        private ConfigEntry<bool> showHatchTimeEntry;
        private ConfigEntry<float> hatchTimeEntry;
        private ConfigEntry<float> growTimeEntry;
        private ConfigEntry<bool> requireShelterEntry;
        private ConfigEntry<bool> canStackEntry;
        private ConfigEntry<bool> soldByDefaultEntry;
        private ConfigEntry<int> sellPriceEntry;

        public bool showHatchTime => showHatchTimeEntry.Value;
        public float hatchTime => hatchTimeEntry.Value;
        public float growTime => growTimeEntry.Value;
        public bool requireShelter => requireShelterEntry.Value;
        public bool canStack => canStackEntry.Value;
        public bool soldByDefault => soldByDefaultEntry.Value;
        public int sellPrice => sellPriceEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            showHatchTimeEntry = Bind(config, Section, "showHatchTime", false,
                "If set to true the time until the egg hatches will be displayed on hover.");
            hatchTimeEntry = Bind(config, Section, "hatchTime", 300f,
                "This value determines the time it takes for an egg to initially hatch into a chicken in seconds.\nA value of 300 means 5 minutes.");
            growTimeEntry = Bind(config, Section, "growTime", 3000f,
                "This value determines the time it takes for a chicken to grow into an adult in seconds.\nA value of 3000 means 50 minutes.");
            requireShelterEntry = Bind(config, Section, "requireShelter", true,
                "This value determines whether or not an egg requires a roof and fire to grow.\nIf set to false, eggs will grow anywhere.");
            canStackEntry = Bind(config, Section, "canStack", false,
                "This value determines whether or not eggs can grow in a stack on the ground.\nIf set to true eggs will grow as many chickens as there are eggs in the dropped stack.");
            soldByDefaultEntry = Bind(config, Section, "soldByDefault", false,
                "If set to true eggs are sold by Haldor without any requirements.");
            sellPriceEntry = Bind(config, Section, "sellPrice", 1500,
                "This value determines the cost of an egg sold by Haldor");
        }
    }
}
