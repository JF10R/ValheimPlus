using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class PlayerProjectileConfiguration : BaseConfig
    {
        private const string Section = "PlayerProjectile";

        private ConfigEntry<float> playerMinChargeVelocityMultiplierEntry;
        private ConfigEntry<float> playerMaxChargeVelocityMultiplierEntry;
        private ConfigEntry<float> playerMinChargeAccuracyMultiplierEntry;
        private ConfigEntry<float> playerMaxChargeAccuracyMultiplierEntry;
        private ConfigEntry<bool> enableScaleWithSkillLevelEntry;

        public float playerMinChargeVelocityMultiplier => playerMinChargeVelocityMultiplierEntry.Value;
        public float playerMaxChargeVelocityMultiplier => playerMaxChargeVelocityMultiplierEntry.Value;
        public float playerMinChargeAccuracyMultiplier => playerMinChargeAccuracyMultiplierEntry.Value;
        public float playerMaxChargeAccuracyMultiplier => playerMaxChargeAccuracyMultiplierEntry.Value;
        public bool enableScaleWithSkillLevel => enableScaleWithSkillLevelEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            playerMinChargeVelocityMultiplierEntry = Bind(config, Section, "playerMinChargeVelocityMultiplier", 0f,
                "Value of 50 would increase the minimum charge velocity from 2 to 3.");
            playerMaxChargeVelocityMultiplierEntry = Bind(config, Section, "playerMaxChargeVelocityMultiplier", 0f,
                "Value of 50 would increase the maximum charge velocity (of Finwood bow) from 50 to 75.");
            playerMinChargeAccuracyMultiplierEntry = Bind(config, Section, "playerMinChargeAccuracyMultiplier", 0f,
                "Value of (+)50 increase in accuracy will change the variance of arrows 20 degree to 10 degree at the point of minimum charge release.");
            playerMaxChargeAccuracyMultiplierEntry = Bind(config, Section, "playerMaxChargeAccuracyMultiplier", 0f,
                "Value of (+)50 increase in accuracy will change the variance of arrows 1 degree to 0.5 degree at the point of maximum charge release.");
            enableScaleWithSkillLevelEntry = Bind(config, Section, "enableScaleWithSkillLevel", false,
                "Enabling this option will linearly scale by skill level from the base values of the weapon to the modified values (according to multipliers above).");
        }
    }
}
