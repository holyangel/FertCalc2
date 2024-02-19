namespace FertCalc
{
    public partial class MainPage : ContentPage
    {
        private FertilizerService fertilizerService = new FertilizerService();
        private Dictionary<string, Entry> fertilizerEntryMappings;

        public MainPage()
        {
            InitializeComponent();
            InitializeFertilizerEntryMappings();
            AttachTextChangedEventHandlers();
        }

        private void InitializeFertilizerEntryMappings()
        {
            fertilizerEntryMappings = new Dictionary<string, Entry>
            {
                {"BioBizz Algamic", BioBizzAlgamicBox},
                {"BioBizz Bloom", BioBizzBloomBox},
                {"BioBizz Grow", BioBizzGrowBox},
                {"CaliMagic", CaliMagicBox},
                {"Canna Calmag", CannaCalmagBox},
                {"CannaCoco 'A'", CannaCocoABox},
                {"CannaCoco 'B'", CannaCocoBBox},
                {"Canna Flores", CannaFloresBox},
                {"Canna PH Down", CannaPHDownBox},
                {"Canna PK13/14", CannaPK1314Box},
                {"Canna Vega", CannaVegaBox},
                {"DryPart Bloom", DryPartBloomBox},
                {"Epsom Salt", EpsomSaltBox},
                {"Gypsum", GypsumBox},
                {"Jacks 0-12-26", Jacks01226Box},
                {"Jacks 5-12-26", Jacks51226Box},
                {"Jacks 5-50-18", Jacks55018Box},
                {"Jacks 7-15-30", Jacks71530Box},
                {"Jacks 10-30-20", Jacks103020Box},
                {"Jacks 12-4-16", Jacks12416Box},
                {"Jacks 15-0-0", Jacks1500Box},
                {"Jacks 15-5-20", Jacks15520Box},
                {"Jacks 15-6-17", Jacks15617Box},
                {"Jacks 18-8-23", Jacks18823Box},
                {"Jacks 20-10-20", Jacks201020Box},
                {"Jacks 20-20-20", Jacks202020Box},
                {"K-Trate LX", KTrateLXBox},
                {"Koolbloom", KoolbloomBox},
                {"L.Koolbloom", LKoolbloomBox},
                {"MagNit", MagNitBox},
                {"Mag-Trate LX", MagTrateLXBox},
                {"MAP", MAPBox},
                {"MaxiBloom", MaxiBloomBox},
                {"MaxiGrow", MaxiGrowBox},
                {"Megacrop", MegacropBox},
                {"Megacrop A", MegacropABox},
                {"Megacrop 'B'", MegacropBBox},
                {"MC Cal-Mag", MCCalMagBox},
                {"MC Sweet Candy", MCSweetCandyBox},
                {"MOAB", MOABBox},
                {"Monster Bloom", MonsterBloomBox},
                {"MPK", MPKBox},
                {"PP Bloom", PPBloomBox},
                {"PP Boost", PPBoostBox},
                {"PP CalKick", PPCalKickBox},
                {"PP Finisher", PPFinisherBox},
                {"PP Grow", PPGrowBox},
                {"PP Spike", PPSpikeBox}
            };
        }

        private void OnQuantityChanged(object sender, TextChangedEventArgs e)
        {
            var totals = fertilizerService.CalculateTotals(fertilizerEntryMappings);
            
            // Update UI with calculated totals
            TotalNitrogen.Text = $"(N): {totals["N"]:N2}";
            TotalPhosphorous.Text = $"(P): {totals["P"]:N2}";
            TotalPotassium.Text = $"(K): {totals["K"]:N2}";
            TotalMagnesium.Text = $"(Mg): {totals["Mg"]:N2}";
            TotalCalcium.Text = $"(Ca): {totals["Ca"]:N2}";
            TotalSulfur.Text = $"(S): {totals["S"]:N2}";
            TotalIron.Text = $"(Fe): {totals["Fe"]:N2}";
            TotalZinc.Text = $"(Zn): {totals["Zn"]:N2}";
            TotalBoron.Text = $"(B): {totals["B"]:N2}";
            TotalManganese.Text = $"(Mn): {totals["Mn"]:N2}";
            TotalCopper.Text = $"(Cu): {totals["Cu"]:N2}";
            TotalMolybdenum.Text = $"(Mo): {totals["Mo"]:N2}";
            TotalPPM.Text = $"PPM: {totals["TotalPPM"]:N2}";
        }

        private void AttachTextChangedEventHandlers()
        {
            foreach (var entry in fertilizerEntryMappings.Values)
            {
                entry.TextChanged += OnQuantityChanged;
            }
        }
    }
}