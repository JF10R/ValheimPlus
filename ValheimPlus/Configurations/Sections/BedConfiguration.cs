using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class BedConfiguration : BaseConfig
    {
        private const string Section = "Bed";

        private ConfigEntry<bool> sleepWithoutSpawnEntry;
        private ConfigEntry<bool> unclaimedBedsOnlyEntry;

        public bool sleepWithoutSpawn => sleepWithoutSpawnEntry.Value;
        public bool unclaimedBedsOnly => unclaimedBedsOnlyEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section");
            sleepWithoutSpawnEntry = Bind(config, Section, "sleepWithoutSpawn", false,
                "Change false to true to enable sleeping without setting bed as spawn.\nWhen hovering over a bed you will be presented with a Hot-Key 'LShift+E'. This Hot-Key will allow for you to sleep on any bed without having to set a spawn-point.");
            unclaimedBedsOnlyEntry = Bind(config, Section, "unclaimedBedsOnly", false,
                "Change false to true to enable sleeping on only unclaimed beds without setting bed as spawn.\nWith this option enabled only beds that are not claimed by other players can be slept on without setting spawn-point using 'Shift+E'");
        }
    }
}
