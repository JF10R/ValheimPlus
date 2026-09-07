using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class StaminaConfiguration : BaseConfig
    {
        private const string Section = "Stamina";

        private ConfigEntry<float> dodgeStaminaUsageEntry;
        private ConfigEntry<float> encumberedStaminaDrainEntry;
        private ConfigEntry<float> jumpStaminaDrainEntry;
        private ConfigEntry<float> runStaminaDrainEntry;
        private ConfigEntry<float> sneakStaminaDrainEntry;
        private ConfigEntry<float> staminaRegenEntry;
        private ConfigEntry<float> staminaRegenDelayEntry;
        private ConfigEntry<float> swimStaminaDrainEntry;

        public float dodgeStaminaUsage => dodgeStaminaUsageEntry.Value;
        public float encumberedStaminaDrain => encumberedStaminaDrainEntry.Value;
        public float jumpStaminaDrain => jumpStaminaDrainEntry.Value;
        public float runStaminaDrain => runStaminaDrainEntry.Value;
        public float sneakStaminaDrain => sneakStaminaDrainEntry.Value;
        public float staminaRegen => staminaRegenEntry.Value;
        public float staminaRegenDelay => staminaRegenDelayEntry.Value;
        public float swimStaminaDrain => swimStaminaDrainEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section. This section contains modifiers.\nModifiers are increases and reduction in percent declared by 50, or -50.");
            dodgeStaminaUsageEntry = Bind(config, Section, "dodgeStaminaUsage", 0f,
                "Changes the amount of stamina cost of using the dodge roll by %");
            encumberedStaminaDrainEntry = Bind(config, Section, "encumberedStaminaDrain", 0f,
                "Changes the stamina drain of being overweight by %");
            jumpStaminaDrainEntry = Bind(config, Section, "jumpStaminaDrain", 0f,
                "Changes the stamina cost of jumping by %");
            runStaminaDrainEntry = Bind(config, Section, "runStaminaDrain", 0f,
                "Changes the stamina cost of running by %");
            sneakStaminaDrainEntry = Bind(config, Section, "sneakStaminaDrain", 0f,
                "Changes the stamina drain by sneaking by %");
            staminaRegenEntry = Bind(config, Section, "staminaRegen", 0f,
                "Changes the total amount of stamina recovered per second by %");
            staminaRegenDelayEntry = Bind(config, Section, "staminaRegenDelay", 0f,
                "Changes the delay until stamina regeneration sets in by %");
            swimStaminaDrainEntry = Bind(config, Section, "swimStaminaDrain", 0f,
                "Changes the stamina drain of swimming by %");
        }
    }
}
