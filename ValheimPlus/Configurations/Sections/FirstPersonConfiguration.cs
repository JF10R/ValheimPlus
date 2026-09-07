using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class FirstPersonConfiguration : BaseConfig
    {
        private const string Section = "FirstPerson";

        private ConfigEntry<KeyCode> hotkeyEntry;
        private ConfigEntry<KeyCode> raiseFOVHotkeyEntry;
        private ConfigEntry<float> defaultFOVEntry;
        private ConfigEntry<KeyCode> lowerFOVHotkeyEntry;

        public KeyCode hotkey => hotkeyEntry.Value;
        public KeyCode raiseFOVHotkey => raiseFOVHotkeyEntry.Value;
        public float defaultFOV => defaultFOVEntry.Value;
        public KeyCode lowerFOVHotkey => lowerFOVHotkeyEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            hotkeyEntry = Bind(config, Section, "hotkey", KeyCode.F10,
                "Hotkey to enable First Person.");
            raiseFOVHotkeyEntry = Bind(config, Section, "raiseFOVHotkey", KeyCode.PageUp,
                "Hotkey to raise Field Of View.");
            defaultFOVEntry = Bind(config, Section, "defaultFOV", 65.0f,
                "Default Field Of View to use.");
            lowerFOVHotkeyEntry = Bind(config, Section, "lowerFOVHotkey", KeyCode.PageDown,
                "Hotkey to lower Field Of View.");
        }
    }
}
