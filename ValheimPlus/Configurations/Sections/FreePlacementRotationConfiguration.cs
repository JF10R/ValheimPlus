using BepInEx.Configuration;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class FreePlacementRotationConfiguration : BaseConfig
    {
        private const string Section = "FreePlacementRotation";

        private ConfigEntry<KeyCode> rotateYEntry;
        private ConfigEntry<KeyCode> rotateXEntry;
        private ConfigEntry<KeyCode> rotateZEntry;
        private ConfigEntry<KeyCode> copyRotationParallelEntry;
        private ConfigEntry<KeyCode> copyRotationPerpendicularEntry;

        public KeyCode rotateY => rotateYEntry.Value;
        public KeyCode rotateX => rotateXEntry.Value;
        public KeyCode rotateZ => rotateZEntry.Value;
        public KeyCode copyRotationParallel => copyRotationParallelEntry.Value;
        public KeyCode copyRotationPerpendicular => copyRotationPerpendicularEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section, if you set this to false the mode will not be accessible.");
            rotateYEntry = Bind(config, Section, "rotateY", KeyCode.LeftAlt,
                "Rotates placement marker by 1 degree with keep ability to attach to nearly pieces.");
            rotateXEntry = Bind(config, Section, "rotateX", KeyCode.C,
                "Rotates placement marker by 1 degree with keep ability to attach to nearly pieces.");
            rotateZEntry = Bind(config, Section, "rotateZ", KeyCode.V,
                "Rotates placement marker by 1 degree with keep ability to attach to nearly pieces.");
            copyRotationParallelEntry = Bind(config, Section, "copyRotationParallel", KeyCode.F,
                "Copy rotation of placement marker from target piece in front of you.");
            copyRotationPerpendicularEntry = Bind(config, Section, "copyRotationPerpendicular", KeyCode.G,
                "Set rotation to be perpendicular to piece in front of you.");
        }
    }
}
