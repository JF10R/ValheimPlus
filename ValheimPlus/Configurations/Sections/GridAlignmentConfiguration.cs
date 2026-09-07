using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class GridAlignmentConfiguration : BaseConfig
    {
        private const string Section = "GridAlignment";

        private ConfigEntry<KeyCode> alignEntry;
        private ConfigEntry<KeyCode> alignToggleEntry;
        private ConfigEntry<KeyCode> changeDefaultAlignmentEntry;

        public KeyCode align => alignEntry.Value;
        public KeyCode alignToggle => alignToggleEntry.Value;
        public KeyCode changeDefaultAlignment => changeDefaultAlignmentEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.\nThis offers a global fixed grid system to make precise placements.");
            alignEntry = Bind(config, Section, "align", KeyCode.LeftAlt,
                "Key to enable grid alignment.");
            alignToggleEntry = Bind(config, Section, "alignToggle", KeyCode.F7,
                "Key to toggle grid alignment.");
            changeDefaultAlignmentEntry = Bind(config, Section, "changeDefaultAlignment", KeyCode.F6,
                "Key to change the default alignment.");
        }
    }
}
