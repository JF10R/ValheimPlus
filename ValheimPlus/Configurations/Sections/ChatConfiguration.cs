using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ChatConfiguration : BaseConfig
    {
        private const string Section = "Chat";

        private ConfigEntry<float> shoutDistanceEntry;
        private ConfigEntry<float> pingDistanceEntry;
        private ConfigEntry<bool> forcedCaseEntry;
        private ConfigEntry<bool> outOfRangeShoutsDisplayInChatWindowEntry;
        private ConfigEntry<float> defaultWhisperDistanceEntry;
        private ConfigEntry<float> defaultNormalDistanceEntry;
        private ConfigEntry<float> defaultShoutDistanceEntry;

        public float shoutDistance => shoutDistanceEntry.Value;
        public float pingDistance => pingDistanceEntry.Value;
        public bool forcedCase => forcedCaseEntry.Value;
        public bool outOfRangeShoutsDisplayInChatWindow => outOfRangeShoutsDisplayInChatWindowEntry.Value;
        public float defaultWhisperDistance => defaultWhisperDistanceEntry.Value;
        public float defaultNormalDistance => defaultNormalDistanceEntry.Value;
        public float defaultShoutDistance => defaultShoutDistanceEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            shoutDistanceEntry = Bind(config, Section, "shoutDistance", 0f,
                "If the player is outside of this range in meters in comparison to the creator of the shout you will not see the message on the map or in the chat. If this is set to 0, its disabled.");
            pingDistanceEntry = Bind(config, Section, "pingDistance", 0f,
                "If the player is outside of this range in meters in comparison to the creator of the ping on the map you will not see the ping on the map. If this is set to 0, its disabled.");
            forcedCaseEntry = Bind(config, Section, "forcedCase", true,
                "Disable the forced upper and lower case conversions for in-game text messages of all types.");
            outOfRangeShoutsDisplayInChatWindowEntry = Bind(config, Section, "outOfRangeShoutsDisplayInChatWindow", true,
                "With this option enabled you will see the shout message in your chat window even if you are outside of shoutDistance.");
            defaultWhisperDistanceEntry = Bind(config, Section, "defaultWhisperDistance", 4f,
                "This value determines the range in meters that you can see whisper text messages by default.");
            defaultNormalDistanceEntry = Bind(config, Section, "defaultNormalDistance", 15f,
                "This value determines the range in meters that you can see normal text messages by default.");
            defaultShoutDistanceEntry = Bind(config, Section, "defaultShoutDistance", 70f,
                "This value determines the range in meters that you can see shout text messages by default.");
        }
    }
}
