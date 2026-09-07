using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedBuildingModeConfiguration : BaseConfig
    {
        private const string Section = "AdvancedBuildingMode";

        private ConfigEntry<KeyCode> enterAdvancedBuildingModeEntry;
        private ConfigEntry<KeyCode> exitAdvancedBuildingModeEntry;
        private ConfigEntry<KeyCode> copyObjectRotationEntry;
        private ConfigEntry<KeyCode> pasteObjectRotationEntry;
        private ConfigEntry<KeyCode> increaseScrollSpeedEntry;
        private ConfigEntry<KeyCode> decreaseScrollSpeedEntry;

        public KeyCode enterAdvancedBuildingMode => enterAdvancedBuildingModeEntry.Value;
        public KeyCode exitAdvancedBuildingMode => exitAdvancedBuildingModeEntry.Value;
        public KeyCode copyObjectRotation => copyObjectRotationEntry.Value;
        public KeyCode pasteObjectRotation => pasteObjectRotationEntry.Value;
        public KeyCode increaseScrollSpeed => increaseScrollSpeedEntry.Value;
        public KeyCode decreaseScrollSpeed => decreaseScrollSpeedEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes\nChange false to true to enable this section, if you set this to false the mode will not be accessible.");
            enterAdvancedBuildingModeEntry = Bind(config, Section, "enterAdvancedBuildingMode", KeyCode.F1,
                "Enter the advanced building mode with this key when building");
            exitAdvancedBuildingModeEntry = Bind(config, Section, "exitAdvancedBuildingMode", KeyCode.F3,
                "Exit the advanced building mode with this key when building");
            copyObjectRotationEntry = Bind(config, Section, "copyObjectRotation", KeyCode.Keypad7,
                "Copy the object rotation of the currently selected object in ABM");
            pasteObjectRotationEntry = Bind(config, Section, "pasteObjectRotation", KeyCode.Keypad8,
                "Apply the copied object rotation to the currently selected object in ABM");
            increaseScrollSpeedEntry = Bind(config, Section, "increaseScrollSpeed", KeyCode.KeypadPlus,
                "Increases the amount an object rotates and moves. Holding Shift will increase in increments of 10 instead of 1.");
            decreaseScrollSpeedEntry = Bind(config, Section, "decreaseScrollSpeed", KeyCode.KeypadMinus,
                "Decreases the amount an object rotates and moves. Holding Shift will decrease in increments of 10 instead of 1.");
        }
    }
}
