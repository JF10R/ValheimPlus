using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class ShipConfiguration : BaseConfig
    {
        private const string Section = "Ship";

        private ConfigEntry<float> forwardSpeedEntry;
        private ConfigEntry<float> backwardSpeedEntry;
        private ConfigEntry<float> rudderSpeedEntry;
        private ConfigEntry<float> steerForceEntry;
        private ConfigEntry<float> waterImpactDamageEntry;

        public float forwardSpeed => forwardSpeedEntry.Value;
        public float backwardSpeed => backwardSpeedEntry.Value;
        public float rudderSpeed => rudderSpeedEntry.Value;
        public float steerForce => steerForceEntry.Value;
        public float waterImpactDamage => waterImpactDamageEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            forwardSpeedEntry = Bind(config, Section, "forwardSpeed", 0f,
                "This value determines the constant amount of force applied to the ship when sailing forward.\nA multiplier of 50 will result in the ship being 50% faster, -50 will result in the ship being 50% slower.");
            backwardSpeedEntry = Bind(config, Section, "backwardSpeed", 0f,
                "This value determines the amount of force applied to the ship when sailing backward.\nA multiplier of 50 will result in the ship moving backward 50% faster, -50 will result in the opposite.");
            rudderSpeedEntry = Bind(config, Section, "rudderSpeed", 0f,
                "This value determines the speed of turning the wheel of the ship.\nA multiplier of 50 will result in the wheel turning 50% faster, -50 will result in the opposite.");
            steerForceEntry = Bind(config, Section, "steerForce", 0f,
                "This value determines the force applied to the ship when steering.\nA multiplier of 50 will result in the ship turning 50% faster, -50 will result in the opposite.");
            waterImpactDamageEntry = Bind(config, Section, "waterImpactDamage", 0f,
                "This value determines the amount of damage the ship takes while sailing.\nA multiplier of 50 will result in the ship taking 50% more damage, -50 will result in the opposite.");
        }
    }
}
