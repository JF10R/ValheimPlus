using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class TurretConfiguration : BaseConfig
    {
        private const string Section = "Turret";

        private ConfigEntry<bool> ignorePlayersEntry;
        private ConfigEntry<bool> unlimitedAmmoEntry;
        private ConfigEntry<float> turnRateEntry;
        private ConfigEntry<float> attackCooldownEntry;
        private ConfigEntry<float> viewDistanceEntry;
        private ConfigEntry<float> projectileVelocityEntry;
        private ConfigEntry<float> projectileAccuracyEntry;

        public bool ignorePlayers => ignorePlayersEntry.Value;
        public bool unlimitedAmmo => unlimitedAmmoEntry.Value;
        public float turnRate => turnRateEntry.Value;
        public float attackCooldown => attackCooldownEntry.Value;
        public float viewDistance => viewDistanceEntry.Value;
        public float projectileVelocity => projectileVelocityEntry.Value;
        public float projectileAccuracy => projectileAccuracyEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            ignorePlayersEntry = Bind(config, Section, "ignorePlayers", false,
                "Change false to true to make the balista ignore players.");
            unlimitedAmmoEntry = Bind(config, Section, "unlimitedAmmo", false,
                "Change false to true to prevent consumption of Balista ammo.");
            turnRateEntry = Bind(config, Section, "turnRate", 0f,
                "This value determines the rate at which the balista turns. A multiplier of -50 will result in the balista turning 50% faster.");
            attackCooldownEntry = Bind(config, Section, "attackCooldown", 0f,
                "This value determines the rate of fire of the balista. A multiplier of -50 will result in the balista shooting 100% faster.");
            viewDistanceEntry = Bind(config, Section, "viewDistance", 0f,
                "This value determines the distance a balista can see targets. A multiplier of 50 will result in the balista seeing 50% further.");
            projectileVelocityEntry = Bind(config, Section, "projectileVelocity", 0f,
                "This value determines the velocity of the projectiles a ballista fires. A multiplier of 50 will result in the projectile velocity being 50% faster.");
            projectileAccuracyEntry = Bind(config, Section, "projectileAccuracy", 0f,
                "This value determines the accuracy of the projectiles a ballista fires. A multiplier of 50 will result in the projectile being 50% more accurate.");
        }
    }
}
