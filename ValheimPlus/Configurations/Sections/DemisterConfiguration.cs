using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class DemisterConfiguration : BaseConfig
    {
        private const string Section = "Demister";

        private ConfigEntry<float> wispLightEntry;
        private ConfigEntry<float> wispTorchEntry;
        private ConfigEntry<float> MistwalkerEntry;

        public float wispLight => wispLightEntry.Value;
        public float wispTorch => wispTorchEntry.Value;
        public float Mistwalker => MistwalkerEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.");
            wispLightEntry = Bind(config, Section, "wispLight", 10f,
                "This value determines the range of Wisp Light demister field.");
            wispTorchEntry = Bind(config, Section, "wispTorch", 12f,
                "This value determines the range of Wisp Torch demister field.");
            MistwalkerEntry = Bind(config, Section, "mistwalker", 5f,
                "This value determines the range of Mistwalker demister field.");
        }
    }
}
