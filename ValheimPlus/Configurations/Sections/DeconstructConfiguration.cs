using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class DeconstructConfiguration : BaseConfig
    {
        private const string Section = "Deconstruct";

        private ConfigEntry<int> percentageOfReturnedResourceEntry;

        public int percentageOfReturnedResource => percentageOfReturnedResourceEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            percentageOfReturnedResourceEntry = Bind(config, Section, "percentageOfReturnedResource", 100,
                "Percentage of the build cost returned when deconstructing a piece. Clamped to 0-100. At 100 the base game behaviour is used, which returns an amount based on the piece quality.");
        }
    }
}
