using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ShieldConfiguration : BaseConfig
    {
        private const string Section = "Shields";

        private ConfigEntry<float> blockRatingEntry;

        public float blockRating => blockRatingEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section, if you set this to false the mode will not be accessible.");
            blockRatingEntry = Bind(config, Section, "blockRating", 0f,
                "Increase or decrease the block value on all shields in %. -50 would be 50% less block rating, 50 would be 50% more block rating.");
        }
    }
}
