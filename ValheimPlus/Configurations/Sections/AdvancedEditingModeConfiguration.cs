using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedEditingModeConfiguration : BaseConfig
    {
        private const string Section = "AdvancedEditingMode";

        private ConfigEntry<KeyCode> enterAdvancedEditingModeEntry;
        private ConfigEntry<KeyCode> resetAdvancedEditingModeEntry;
        private ConfigEntry<KeyCode> abortAndExitAdvancedEditingModeEntry;
        private ConfigEntry<KeyCode> confirmPlacementOfAdvancedEditingModeEntry;
        private ConfigEntry<KeyCode> copyObjectRotationEntry;
        private ConfigEntry<KeyCode> pasteObjectRotationEntry;
        private ConfigEntry<KeyCode> increaseScrollSpeedEntry;
        private ConfigEntry<KeyCode> decreaseScrollSpeedEntry;

        public KeyCode enterAdvancedEditingMode => enterAdvancedEditingModeEntry.Value;
        public KeyCode resetAdvancedEditingMode => resetAdvancedEditingModeEntry.Value;
        public KeyCode abortAndExitAdvancedEditingMode => abortAndExitAdvancedEditingModeEntry.Value;
        public KeyCode confirmPlacementOfAdvancedEditingMode => confirmPlacementOfAdvancedEditingModeEntry.Value;
        public KeyCode copyObjectRotation => copyObjectRotationEntry.Value;
        public KeyCode pasteObjectRotation => pasteObjectRotationEntry.Value;
        public KeyCode increaseScrollSpeed => increaseScrollSpeedEntry.Value;
        public KeyCode decreaseScrollSpeed => decreaseScrollSpeedEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes\nChange false to true to enable this section, if you set this to false the mode will not be accessible.");
            enterAdvancedEditingModeEntry = Bind(config, Section, "enterAdvancedEditingMode", KeyCode.Keypad0,
                "Enter the advanced editing mode with this key");
            resetAdvancedEditingModeEntry = Bind(config, Section, "resetAdvancedEditingMode", KeyCode.F7,
                "Reset the object to its original position and rotation");
            abortAndExitAdvancedEditingModeEntry = Bind(config, Section, "abortAndExitAdvancedEditingMode", KeyCode.F8,
                "Exit the advanced editing mode with this key and reset the object");
            confirmPlacementOfAdvancedEditingModeEntry = Bind(config, Section, "confirmPlacementOfAdvancedEditingMode", KeyCode.KeypadEnter,
                "Confirm the placement of the object and place it");
            copyObjectRotationEntry = Bind(config, Section, "copyObjectRotation", KeyCode.Keypad7,
                "Copy the object rotation of the currently selected object in AEM");
            pasteObjectRotationEntry = Bind(config, Section, "pasteObjectRotation", KeyCode.Keypad8,
                "Apply the copied object rotation to the currently selected object in AEM");
            increaseScrollSpeedEntry = Bind(config, Section, "increaseScrollSpeed", KeyCode.KeypadPlus,
                "Increases the amount an object rotates and moves. Holding Shift will increase in increments of 10 instead of 1.");
            decreaseScrollSpeedEntry = Bind(config, Section, "decreaseScrollSpeed", KeyCode.KeypadMinus,
                "Decreases the amount an object rotates and moves. Holding Shift will decrease in increments of 10 instead of 1.");
        }
    }
}
