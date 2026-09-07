using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class MonsterProjectileConfiguration : BaseConfig
    {
        private const string Section = "MonsterProjectile";

        private ConfigEntry<float> monsterMaxChargeVelocityMultiplierEntry;
        private ConfigEntry<float> monsterMaxChargeAccuracyMultiplierEntry;

        public float monsterMaxChargeVelocityMultiplier => monsterMaxChargeVelocityMultiplierEntry.Value;
        public float monsterMaxChargeAccuracyMultiplier => monsterMaxChargeAccuracyMultiplierEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            monsterMaxChargeVelocityMultiplierEntry = Bind(config, Section, "monsterMaxChargeVelocityMultiplier", 0f,
                "Value of 10 would increase the projectile velocity from 50 to 55.");
            monsterMaxChargeAccuracyMultiplierEntry = Bind(config, Section, "monsterMaxChargeAccuracyMultiplier", 0f,
                "Value of (+)10 increase in accuracy will change the variance of projectile 1 degree to 0.9 degree at the point of projectile release.");
        }
    }
}
