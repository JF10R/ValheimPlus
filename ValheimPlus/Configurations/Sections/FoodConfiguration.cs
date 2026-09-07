using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class FoodConfiguration : BaseConfig
    {
        private const string Section = "Food";

        private ConfigEntry<float> foodDurationMultiplierEntry;
        private ConfigEntry<bool> disableFoodDegradationEntry;

        public float foodDurationMultiplier => foodDurationMultiplierEntry.Value;
        public bool disableFoodDegradation => disableFoodDegradationEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            foodDurationMultiplierEntry = Bind(config, Section, "foodDurationMultiplier", 0f,
                "Increase or reduce the time that food lasts by %.\nThe value 50 would cause food to run out 50% slower, -50% would cause the food to run out 50% faster.");
            disableFoodDegradationEntry = Bind(config, Section, "disableFoodDegradation", false,
                "This option prevents food degrading over time - in other words, it retains its maximum benefit until it runs out instead of reducing its effect over time.");
        }
    }
}
