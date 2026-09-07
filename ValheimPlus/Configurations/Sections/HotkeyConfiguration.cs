using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class HotkeyConfiguration : BaseConfig
    {
        private const string Section = "Hotkeys";

        private ConfigEntry<KeyCode> rollForwardsEntry;
        private ConfigEntry<KeyCode> rollBackwardsEntry;

        public KeyCode rollForwards => rollForwardsEntry.Value;
        public KeyCode rollBackwards => rollBackwardsEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes\nChange false to true to enable this section.");
            rollForwardsEntry = Bind(config, Section, "rollForwards", KeyCode.F9,
                "Roll forwards on hot key pressed.");
            rollBackwardsEntry = Bind(config, Section, "rollBackwards", KeyCode.F10,
                "Roll backwards on hot key pressed.");
        }
    }
}
