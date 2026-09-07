using BepInEx.Configuration;

namespace ValheimPlus.Configurations.Sections
{
    public class PickableConfiguration : BaseConfig
    {
        private const string Section = "Pickable";

        private ConfigEntry<float> ediblesEntry;
        private ConfigEntry<float> flowersAndIngredientsEntry;
        private ConfigEntry<float> materialsEntry;
        private ConfigEntry<float> valuablesEntry;
        private ConfigEntry<float> surtlingCoresEntry;
        private ConfigEntry<float> blackCoresEntry;
        private ConfigEntry<float> questItemsEntry;

        public float edibles => ediblesEntry.Value;
        public float flowersAndIngredients => flowersAndIngredientsEntry.Value;
        public float materials => materialsEntry.Value;
        public float valuables => valuablesEntry.Value;
        public float surtlingCores => surtlingCoresEntry.Value;
        public float blackCores => blackCoresEntry.Value;
        public float questItems => questItemsEntry.Value;

        public override void Bind(ConfigFile config)
        {
            BindEnabled(config, Section, false,
                "Change false to true to enable this section.\nEach value below (in percent) will modify the yield when \"picking\" items (default key E) such as berries and flowers.\nA value of 100 will double drops, 200 will triple and so on.");
            ediblesEntry = Bind(config, Section, "edibles", 0f,
                "All berries, all mushrooms, onions and carrots");
            flowersAndIngredientsEntry = Bind(config, Section, "flowersAndIngredients", 0f,
                "Barley, Flax, Dandelion, Thistle, Carrot Seeds, Turnip Seeds, Turnip, Onion Seeds");
            materialsEntry = Bind(config, Section, "materials", 0f,
                "Bone Fragments, Flint, Stone, Wood (branches on the ground)");
            valuablesEntry = Bind(config, Section, "valuables", 0f,
                "Amber, Amber Pearl, Coins, Ruby");
            surtlingCoresEntry = Bind(config, Section, "surtlingCores", 0f,
                "Surtling Core only");
            blackCoresEntry = Bind(config, Section, "blackCores", 0f,
                "Black Core only");
            questItemsEntry = Bind(config, Section, "questItems", 0f,
                "Items needed to sacrifice to bosses (excluding the first two bosses)");
        }
    }
}
