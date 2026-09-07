using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class CameraConfiguration : BaseConfig
    {
        private const string Section = "Camera";

        private ConfigEntry<float> cameraMaximumZoomDistanceEntry;
        private ConfigEntry<float> cameraBoatMaximumZoomDistanceEntry;
        private ConfigEntry<float> cameraFOVEntry;

        public float cameraMaximumZoomDistance => cameraMaximumZoomDistanceEntry.Value;
        public float cameraBoatMaximumZoomDistance => cameraBoatMaximumZoomDistanceEntry.Value;
        public float cameraFOV => cameraFOVEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            cameraMaximumZoomDistanceEntry = Bind(config, Section, "cameraMaximumZoomDistance", 6f,
                "The maximum zoom distance to your character in-game.\nDefault is 6");
            cameraBoatMaximumZoomDistanceEntry = Bind(config, Section, "cameraBoatMaximumZoomDistance", 6f,
                "The maximum zoom distance to your character when in a boat.\nDefault is 6");
            cameraFOVEntry = Bind(config, Section, "cameraFOV", 65f,
                "The in-game camera FOV.\nDefault is 65");
        }
    }
}
