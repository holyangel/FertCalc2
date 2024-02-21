using System.Xml.Serialization;

namespace FertCalc2
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
        private string mixesFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UserMixes.xml");
        private readonly object fileAccessLock = new object();
        private bool isPerGallon = true; // Default to gallons
        private double ConversionFactor => isPerGallon ? 1 : 3.78541;

        public MainPage()
        {
            InitializeComponent();
            InitializeFertilizerEntryMappings();
            AttachTextChangedEventHandlers();
            LoadMixesFromFile();
            PopulateMixesPicker();
            PopulateComparisonMixesPicker();

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

        private void ToggleUnitButton_Clicked(object sender, EventArgs e)
        {
            isPerGallon = !isPerGallon; // Toggle the flag
            ToggleUnitButton.Text = isPerGallon ? "Switch to mL/L" : "Switch to g/Gal"; // Update button text
            UpdateNutrientDisplays(); // Recalculate and update the display
        }

        private void OnQuantityChanged(object sender, TextChangedEventArgs e)
        {
            var conversionFactor = isPerGallon ? 1 : 1 * 3.78541; // Convert to per liter if not isPerGallon, otherwise leave as per gallon.

            var totals = fertilizerService.CalculateTotals(fertilizerEntryMappings);
            
            // Update UI with calculated totals
            TotalNitrogen.Text = $"(N): {(totals["N"] * conversionFactor):N2}";
            TotalPhosphorous.Text = $"(P): {(totals["P"] * conversionFactor):N2}";
            TotalPotassium.Text = $"(K): {(totals["K"] * conversionFactor):N2}";
            TotalMagnesium.Text = $"(Mg): {(totals["Mg"] * conversionFactor):N2}";
            TotalCalcium.Text = $"(Ca): {(totals["Ca"] * conversionFactor):N2}";
            TotalSulfur.Text = $"(S): {(totals["S"] * conversionFactor):N2}";
            TotalIron.Text = $"(Fe): {(totals["Fe"] * conversionFactor):N2}";
            TotalZinc.Text = $"(Zn): {(totals["Zn"] * conversionFactor):N2}";
            TotalBoron.Text = $"(B): {(totals["B"] * conversionFactor):N2}";
            TotalManganese.Text = $"(Mn): {(totals["Mn"] * conversionFactor):N2}";
            TotalCopper.Text = $"(Cu): {(totals["Cu"] * conversionFactor):N2}";
            TotalMolybdenum.Text = $"(Mo): {(totals["Mo"] * conversionFactor):N2}";
            PPM.Text = $"PPM: {(totals["PPM"] * conversionFactor):N2}";

            // If you have a comparison feature, apply similar conversion to the comparison values
            if (ComparisonMixesPicker.SelectedItem != null && ComparisonMixesPicker.SelectedItem.ToString() != "Reset")
            {
                ApplyComparisonMixDetails(savedMixes[ComparisonMixesPicker.SelectedItem.ToString()]);
            }
        }

        private void UpdateNutrientDisplays()
        {
            // Example conversion (adjust according to your calculation logic)
            var conversionFactor = isPerGallon ? 1 : 1 * 3.78541; // Convert to per liter if not isPerGallon, otherwise leave as per gallon.

            var totals = fertilizerService.CalculateTotals(fertilizerEntryMappings);

            // Adjust each value by the conversionFactor before displaying
            TotalNitrogen.Text = $"(N): {(totals["N"] * conversionFactor):N2}";
            TotalPhosphorous.Text = $"(P): {(totals["P"] * conversionFactor):N2}";
            TotalPotassium.Text = $"(K): {(totals["K"] * conversionFactor):N2}";
            TotalMagnesium.Text = $"(Mg): {(totals["Mg"] * conversionFactor):N2}";
            TotalCalcium.Text = $"(Ca): {(totals["Ca"] * conversionFactor):N2}";
            TotalSulfur.Text = $"(S): {(totals["S"] * conversionFactor):N2}";
            TotalIron.Text = $"(Fe): {(totals["Fe"] * conversionFactor):N2}";
            TotalZinc.Text = $"(Zn): {(totals["Zn"] * conversionFactor):N2}";
            TotalBoron.Text = $"(B): {(totals["B"] * conversionFactor):N2}";
            TotalManganese.Text = $"(Mn): {(totals["Mn"] * conversionFactor):N2}";
            TotalCopper.Text = $"(Cu): {(totals["Cu"] * conversionFactor):N2}";
            TotalMolybdenum.Text = $"(Mo): {(totals["Mo"] * conversionFactor):N2}";
            PPM.Text = $"PPM: {(totals["PPM"] * conversionFactor):N2}";

            // If you have a comparison feature, apply similar conversion to the comparison values
            if (ComparisonMixesPicker.SelectedItem != null && ComparisonMixesPicker.SelectedItem.ToString() != "Reset")
            {
                ApplyComparisonMixDetails(savedMixes[ComparisonMixesPicker.SelectedItem.ToString()]);
            }
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

        private void PopulateComparisonMixesPicker()
        {
            ComparisonMixesPicker.Items.Clear(); // Clear existing items first

            // Add "Reset" option first
            ComparisonMixesPicker.Items.Add("Reset");

            // Extract mix names and sort them
            var sortedMixNames = savedMixes.Keys.ToList();
            sortedMixNames.Sort();

            // Add sorted mix names to the Picker
            foreach (var mixName in sortedMixNames)
            {
                ComparisonMixesPicker.Items.Add(mixName);
            }
        }

        private void RefreshMixesInPicker()
        {
            LoadMixesFromFile(); // Reload mixes from file
            PopulateMixesPicker(); // Repopulate Picker
            PopulateComparisonMixesPicker(); // Repopulate Comparison Picker
        }

        private void PredefinedMixesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMix = PredefinedMixesPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedMix) || selectedMix == "Reset")
            {
                ClearAllEntries(); // Clear fertilizer quantities
                PredefinedMixesPicker.SelectedIndex = -1; // Reset selection to default
            }
            else if (savedMixes.ContainsKey(selectedMix))
            {
                ClearAllEntries(); // Clear fertilizer quantities before applying from dictionary
                ApplyMixDetails(savedMixes[selectedMix]); // Apply quantities from dictionary
            }
        }

        private void ComparisonMixesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMix = ComparisonMixesPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedMix) || selectedMix == "Reset")
            {
                HideComparisonVisibility(); // Hide all the Comparison Labels
                ResetComparisonLabels(); // Clear comparison values
                ComparisonMixesPicker.SelectedIndex = -1; // Reset selection to default
            }
            else if (savedMixes.ContainsKey(selectedMix))
            {
                ShowComparisonVisibility();
                ApplyComparisonMixDetails(savedMixes[selectedMix]);
            }
        }

        private void ShowComparisonVisibility()
        {
            // Setting the visibility of all comparison elements at once
            Compare.IsVisible = true;
            CompNitrogen.IsVisible = true;
            CompPhosphorous.IsVisible = true;
            CompPotassium.IsVisible = true;
            CompMagnesium.IsVisible = true;
            CompCalcium.IsVisible = true;
            CompSulfur.IsVisible = true;
            CompIron.IsVisible = true;
            CompZinc.IsVisible = true;
            CompBoron.IsVisible = true;
            CompManganese.IsVisible = true;
            CompCopper.IsVisible = true;
            CompMolybdenum.IsVisible = true;
            CompPPM.IsVisible = true;
        }

        private void HideComparisonVisibility()
        {
            // Setting the visibility of all comparison elements at once
            Compare.IsVisible = false;
            CompNitrogen.IsVisible = false;
            CompPhosphorous.IsVisible = false;
            CompPotassium.IsVisible = false;
            CompMagnesium.IsVisible = false;
            CompCalcium.IsVisible = false;
            CompSulfur.IsVisible = false;
            CompIron.IsVisible = false;
            CompZinc.IsVisible = false;
            CompBoron.IsVisible = false;
            CompManganese.IsVisible = false;
            CompCopper.IsVisible = false;
            CompMolybdenum.IsVisible = false;
            CompPPM.IsVisible = false;
        }

        private void ResetComparisonLabels()
        {
            // Reset the text of all comparison labels to indicate no data
            CompNitrogen.Text = "(N): ";
            CompPhosphorous.Text = "(P): ";
            CompPotassium.Text = "(K): ";
            CompMagnesium.Text = "(Mg): ";
            CompCalcium.Text = "(Ca): ";
            CompSulfur.Text = "(S): ";
            CompIron.Text = "(Fe): ";
            CompZinc.Text = "(Zn): ";
            CompBoron.Text = "(B): ";
            CompManganese.Text = "(Mn): ";
            CompCopper.Text = "(Cu): ";
            CompMolybdenum.Text = "(Mo): ";
            CompPPM.Text = "PPM: ";
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
            PPM.Text = "PPM: ";
        }

        private void ApplyMixDetails(Dictionary<string, double> mixDetails)
        {
            foreach (var detail in mixDetails)
            {
                if (fertilizerEntryMappings.TryGetValue(detail.Key, out var entry))
                {
                    entry.Text = detail.Value.ToString();
                    UpdateNutrientDisplays();
                }
            }
        }

        private void ApplyComparisonMixDetails(Dictionary<string, double> mixDetails)
        {
            // Initialize nutrient totals for comparison
            var totals = new Dictionary<string, double>
            {
                {"N", 0}, {"P", 0}, {"K", 0}, {"Mg", 0}, {"Ca", 0}, {"S", 0},
                {"Fe", 0}, {"Zn", 0}, {"B", 0}, {"Mn", 0}, {"Cu", 0}, {"Mo", 0}, {"PPM", 0}
            };

            // Calculate total nutrients based on the mix details
            foreach (var detail in mixDetails)
            {
                if (fertilizerService.Fertilizers.TryGetValue(detail.Key, out var fertilizer))
                {
                    // Multiplying each value by detail.Value and then by ConversionFactor for the unit conversion
                    totals["N"] += (fertilizer.N * detail.Value) * ConversionFactor;
                    totals["P"] += (fertilizer.P * detail.Value) * ConversionFactor;
                    totals["K"] += (fertilizer.K * detail.Value) * ConversionFactor;
                    totals["Mg"] += (fertilizer.Mg * detail.Value) * ConversionFactor;
                    totals["Ca"] += (fertilizer.Ca * detail.Value) * ConversionFactor;
                    totals["S"] += (fertilizer.S * detail.Value) * ConversionFactor;
                    totals["Fe"] += (fertilizer.Fe * detail.Value) * ConversionFactor;
                    totals["Zn"] += (fertilizer.Zn * detail.Value) * ConversionFactor;
                    totals["B"] += (fertilizer.B * detail.Value) * ConversionFactor;
                    totals["Mn"] += (fertilizer.Mn * detail.Value) * ConversionFactor;
                    totals["Cu"] += (fertilizer.Cu * detail.Value) * ConversionFactor;
                    totals["Mo"] += (fertilizer.Mo * detail.Value) * ConversionFactor;
                    totals["PPM"] += (fertilizer.PPM * detail.Value) * ConversionFactor;
                }
            }

            // Update and color the comparison labels
            UpdateAndColorLabel(CompNitrogen, totals["N"], "N");
            UpdateAndColorLabel(CompPhosphorous, totals["P"], "P");
            UpdateAndColorLabel(CompPotassium, totals["K"], "K");
            UpdateAndColorLabel(CompMagnesium, totals["Mg"], "Mg");
            UpdateAndColorLabel(CompCalcium, totals["Ca"], "Ca");
            UpdateAndColorLabel(CompSulfur, totals["S"], "S");
            UpdateAndColorLabel(CompIron, totals["Fe"], "Fe");
            UpdateAndColorLabel(CompZinc, totals["Zn"], "Zn");
            UpdateAndColorLabel(CompBoron, totals["B"], "B");
            UpdateAndColorLabel(CompManganese, totals["Mn"], "Mn");
            UpdateAndColorLabel(CompCopper, totals["Cu"], "Cu");
            UpdateAndColorLabel(CompMolybdenum, totals["Mo"], "Mo");
            UpdateAndColorLabel(CompPPM, totals["PPM"], "PPM");
        }

        private void UpdateAndColorLabel(Label label, double comparisonValue, string nutrientKey)
        {
            // Attempt to retrieve the current mix's value for this nutrient.
            if (fertilizerService.CalculateTotals(fertilizerEntryMappings).TryGetValue(nutrientKey, out double currentMixValue))
            {
                double convertedCurrentMixValue = currentMixValue * ConversionFactor;
                double epsilon = 0.01; // Small value to account for floating-point precision issues

                // Update the label with the comparison value.
                label.Text = $"({nutrientKey}): {comparisonValue:N2}";

                // Adjust comparison logic to use epsilon for floating-point comparison
                if (Math.Abs(comparisonValue - convertedCurrentMixValue) < epsilon)
                {
                    label.TextColor = Colors.DodgerBlue;
                }
                else if (comparisonValue > convertedCurrentMixValue)
                {
                    label.TextColor = Colors.Green;
                }
                else
                {
                    label.TextColor = Colors.Red;
                }
            }
            else
            {
                // Log or handle the error if the nutrient key is not found
                Console.WriteLine($"Nutrient key '{nutrientKey}' not found in current totals.");
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
            lock (fileAccessLock)
            {
                var mixCollection = ConvertToSerializableForm();
                var serializer = new XmlSerializer(typeof(MixCollection));
                using (var writer = new StreamWriter(mixesFilePath))
                {
                    serializer.Serialize(writer, mixCollection);
                }
            }
        }

        private void LoadMixesFromFile()
        {
            lock (fileAccessLock)
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

        private async void DeleteMixButton_Clicked(object sender, EventArgs e)
        {
            var selectedMix = PredefinedMixesPicker.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedMix) || selectedMix == "Reset")
            {
                await DisplayAlert("Delete Mix", "No mix selected or cannot delete 'Reset' option.", "OK");
                return;
            }

            bool confirmDelete = await DisplayAlert("Delete Mix", $"Are you sure you want to delete '{selectedMix}'?", "Yes", "No");
            if (confirmDelete)
            {
                // Remove the selected mix
                if (savedMixes.ContainsKey(selectedMix))
                {
                    savedMixes.Remove(selectedMix);
                    SaveMixesToFile(); // Persist the removal to file
                    RefreshMixesInPicker(); // Update UI
                    ClearAllEntries(); // Optional: Clear the current mix details from the UI
                    await DisplayAlert("Delete Mix", $"Mix '{selectedMix}' has been deleted.", "OK");
                }
            }
        }

        private async void OnImportClicked(object sender, EventArgs e)
        {
            try
            {
                var fileResult = await FilePicker.PickAsync();
                if (fileResult != null)
                {
                    var importFilePath = fileResult.FullPath;

                    // Deserialize the imported mixes
                    MixCollection importedMixes;
                    var serializer = new XmlSerializer(typeof(MixCollection));
                    using (var reader = new StreamReader(importFilePath))
                    {
                        importedMixes = (MixCollection)serializer.Deserialize(reader);
                    }

                    // Merge with existing mixes
                    bool newMixesAdded = false;
                    foreach (var mix in importedMixes.Mixes)
                    {
                        if (!savedMixes.ContainsKey(mix.Name))
                        {
                            savedMixes.Add(mix.Name, mix.FertilizerQuantities.ToDictionary(fq => fq.Name, fq => fq.Quantity));
                            newMixesAdded = true;
                        }
                    }

                    if (newMixesAdded)
                    {
                        SaveMixesToFile(); // Save the updated list
                        RefreshMixesInPicker();
                        await DisplayAlert("Import Successful", "New mixes have been added.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Import Notice", "No new mixes to add.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Import Failed", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            try
            {
                // Assuming mixesFilePath is the path to your existing UserMixes.xml
                if (File.Exists(mixesFilePath))
                {
                    // Use Share API to share the file
                    await Share.RequestAsync(new ShareFileRequest
                    {
                        Title = "Export User Mixes",
                        File = new ShareFile(mixesFilePath)
                    });
                }
                else
                {
                    await DisplayAlert("Export Failed", "UserMixes.xml not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Export Failed", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}