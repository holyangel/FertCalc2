using System.Xml.Serialization;

namespace FertCalc
{
    [XmlRoot("MixCollection")]
    public class MixCollection
    {
        [XmlElement("Mix")]
        public List<Mix> Mixes { get; set; } = new List<Mix>();
    }

    public class Mix
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("Fertilizers"), XmlArrayItem("Fertilizer")]
        public List<FertilizerQuantity> FertilizerQuantities { get; set; } = new List<FertilizerQuantity>();
    }

    public class FertilizerQuantity
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("quantity")]
        public double Quantity { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        private readonly FertilizerService fertilizerService = new FertilizerService();
        private Dictionary<string, Entry> fertilizerEntryMappings;
        private Dictionary<string, Dictionary<string, double>> savedMixes = new Dictionary<string, Dictionary<string, double>>();
        private readonly string mixesFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UserMixes.xml");

        public MainPage()
        {
            InitializeComponent();
            InitializeFertilizerEntryMappings();
            AttachTextChangedEventHandlers();
            LoadMixesFromFile();
            PopulateMixesPicker();
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

        private void PopulateMixesPicker()
        {
            PredefinedMixesPicker.Items.Clear(); // Clear existing items first

            // Add "Reset" option first
            PredefinedMixesPicker.Items.Add("Reset");

            // Extract mix names and sort them
            var sortedMixNames = savedMixes.Keys.ToList();
            sortedMixNames.Sort();

            // Add sorted mix names to the Picker
            foreach (var mixName in sortedMixNames)
            {
                PredefinedMixesPicker.Items.Add(mixName);
            }
        }

        private void RefreshMixesInPicker()
        {
            LoadMixesFromFile(); // Reload mixes from file
            PopulateMixesPicker(); // Repopulate Picker
        }

        private void PredefinedMixesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMix = PredefinedMixesPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedMix) || selectedMix == "Reset")
            {
                PredefinedMixesPicker.SelectedIndex = 0;
                ClearAllEntries();
            }
            else if (savedMixes.ContainsKey(selectedMix))
            {
                ApplyMixDetails(savedMixes[selectedMix]);
            }
        }

        private void ClearAllEntries()
        {
            foreach (var entry in fertilizerEntryMappings.Values)
            {
                entry.Text = string.Empty;
            }

            TotalNitrogen.Text = "(N): ";
            TotalPhosphorous.Text = "(P): ";
            TotalPotassium.Text = "(K): ";
            TotalMagnesium.Text = "(Mg): ";
            TotalCalcium.Text = "(Ca): ";
            TotalSulfur.Text = "(S): ";
            TotalIron.Text = "(Fe): ";
            TotalZinc.Text = "(Zn): ";
            TotalBoron.Text = "(B): ";
            TotalManganese.Text = "(Mn): ";
            TotalCopper.Text = "(Cu): ";
            TotalMolybdenum.Text = "(Mo): ";
            TotalPPM.Text = "PPM: ";
        }

        private void ApplyMixDetails(Dictionary<string, double> mixDetails)
        {
            foreach (var detail in mixDetails)
            {
                if (fertilizerEntryMappings.TryGetValue(detail.Key, out var entry))
                {
                    entry.Text = detail.Value.ToString();
                }
            }
        }

        private MixCollection ConvertToSerializableForm()
        {
            var mixCollection = new MixCollection();
            foreach (var mixEntry in savedMixes)
            {
                var mix = new Mix { Name = mixEntry.Key };
                foreach (var fertEntry in mixEntry.Value)
                {
                    mix.FertilizerQuantities.Add(new FertilizerQuantity { Name = fertEntry.Key, Quantity = fertEntry.Value });
                }
                mixCollection.Mixes.Add(mix);
            }
            return mixCollection;
        }

        private async void SaveMixButton_Clicked(object sender, EventArgs e)
        {
            // Prompt user for a mix name
            string mixName = await DisplayPromptAsync("Save Mix", "Enter a name for this mix:");

            if (string.IsNullOrWhiteSpace(mixName))
            {
                // User cancelled or entered an invalid name
                await DisplayAlert("Error", "Invalid mix name.", "OK");
                return;
            }

            if (savedMixes.ContainsKey(mixName))
            {
                // Ask if they want to overwrite the existing mix
                bool overwrite = await DisplayAlert("Confirm", "A mix with this name already exists. Overwrite?", "Yes", "No");
                if (!overwrite) return;
            }

            // Capture current fertilizer quantities
            Dictionary<string, double> mixDetails = new Dictionary<string, double>();
            foreach (var entry in fertilizerEntryMappings)
            {
                string text = entry.Value.Text;
                if (!string.IsNullOrWhiteSpace(text) && double.TryParse(text, out double quantity))
                {
                    mixDetails[entry.Key] = quantity;
                }
            }

            // Save or update the mix
            savedMixes[mixName] = mixDetails;

            // Persist the changes
            SaveMixesToFile();

            // Refresh the picker items to include the new/updated mix
            RefreshMixesInPicker();

            // Optionally, select the newly saved mix in the picker
            PredefinedMixesPicker.SelectedItem = mixName;

            await DisplayAlert("Saved", $"Mix '{mixName}' has been saved.", "OK");
        }

        private void SaveMixesToFile()
        {
            var mixCollection = ConvertToSerializableForm();
            var serializer = new XmlSerializer(typeof(MixCollection));
            using (var writer = new StreamWriter(mixesFilePath))
            {
                serializer.Serialize(writer, mixCollection);
            }
        }

        private void LoadMixesFromFile()
        {
            if (File.Exists(mixesFilePath))
            {
                var serializer = new XmlSerializer(typeof(MixCollection));
                using (var reader = new StreamReader(mixesFilePath))
                {
                    var mixCollection = (MixCollection)serializer.Deserialize(reader);
                    savedMixes = mixCollection.Mixes.ToDictionary(
                        mix => mix.Name,
                        mix => mix.FertilizerQuantities.ToDictionary(fq => fq.Name, fq => fq.Quantity)
                    );
                }
            }
        }
    }
}