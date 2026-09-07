using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class WispSpawnerConfiguration : BaseConfig
    {
        private const string Section = "WispSpawner";

        private ConfigEntry<int> maximumWispsEntry;
        private ConfigEntry<bool> onlySpawnAtNightEntry;
        private ConfigEntry<float> wispSpawnIntervalMultiplierEntry;
        private ConfigEntry<float> wispSpawnChanceMultiplierEntry;

        public int maximumWisps => maximumWispsEntry.Value;
        public bool onlySpawnAtNight => onlySpawnAtNightEntry.Value;
        public float wispSpawnIntervalMultiplier => wispSpawnIntervalMultiplierEntry.Value;
        public float wispSpawnChanceMultiplier => wispSpawnChanceMultiplierEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            maximumWispsEntry = Bind(config, Section, "maximumWisps", 3,
                "This value determines the maximum amount of Wisp per spawner.");
            onlySpawnAtNightEntry = Bind(config, Section, "onlySpawnAtNight", true,
                "This value determines if the Wisps can spawn during the day.");
            wispSpawnIntervalMultiplierEntry = Bind(config, Section, "wispSpawnIntervalMultiplier", 0f,
                "This value determines the rate at which the Wisps try to spawn. A multiplier of -50 will result in a wisp trying to spawn every 2.5 seconds (5 seconds by default).");
            wispSpawnChanceMultiplierEntry = Bind(config, Section, "wispSpawnChanceMultiplier", 0f,
                "This value determines the chance of a Wisp to spawn. A multiplier of 200 will result in a 100% wisp spawn chance.");
        }
    }
}
