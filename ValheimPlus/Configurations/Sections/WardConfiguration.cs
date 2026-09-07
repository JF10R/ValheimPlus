using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class WardConfiguration : BaseConfig
    {
        private const string Section = "Ward";

        private ConfigEntry<float> wardRangeEntry;
        private ConfigEntry<float> wardEnemySpawnRangeEntry;

        public float wardRange => wardRangeEntry.Value;
        public float wardEnemySpawnRange => wardEnemySpawnRangeEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            wardRangeEntry = Bind(config, Section, "wardRange", 20f,
                "The range of wards by meters.");
            wardEnemySpawnRangeEntry = Bind(config, Section, "wardEnemySpawnRange", 0f,
                "Set the enemy spawn radius around wards in meters\nThis value equals wardRange if its set to 0.");
        }
    }
}
