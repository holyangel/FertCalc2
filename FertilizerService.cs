using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertCalc
{
    public class FertilizerService
    {
        private Dictionary<string, Fertilizer> fertilizers = new Dictionary<string, Fertilizer>();
        public Dictionary<string, Fertilizer> Fertilizers => fertilizers;

        public FertilizerService()
        {
            InitializeFertilizers();
        }

        private void InitializeFertilizers()
        {
            fertilizers = new Dictionary<string, Fertilizer>
            {
                { "BioBizz Algamic", new Fertilizer { Name = "BioBizz Algamic", N = 2, P = 0.873, K = 0.83, Mg = 0.04, Ca = 0.12, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0.018, Cu = 0, Mo = 0, PPM = 3.881 } },
                { "BioBizz Bloom", new Fertilizer { Name = "BioBizz Bloom", N = 20, P = 28.3, K = 36.2, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 84.5 } },
                { "BioBizz Grow", new Fertilizer { Name = "BioBizz Grow", N = 40, P = 13.1, K = 49.8, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 102.9 } },
                { "CaliMagic", new Fertilizer { Name = "CaliMagic", N = 10, P = 0, K = 0, Mg = 15, Ca = 50, S = 0, Fe = 1, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 76 } },
                { "Canna Calmag", new Fertilizer { Name = "Canna Calmag", N = 8.242, P = 0, K = 0, Mg = 3.3497, Ca = 9.51, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 21.1017 } },
                { "CannaCoco 'A'", new Fertilizer { Name = "CannaCoco 'A'", N = 15.85, P = 0, K = 2.425, Mg = 2.5043, Ca = 26.258, S = 0, Fe = 0.0834, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 47.1207 } },
                { "CannaCoco 'B'", new Fertilizer { Name = "CannaCoco 'B'", N = 4.226, P = 8.981, K = 9.6687, Mg = 9.51, Ca = 0, S = 9.51, Fe = 0, Zn = 0.0369, B = 0.0369, Mn = 0.0634, Cu = 0.005, Mo = 0, PPM = 42.0379 } },
                { "Canna Flores", new Fertilizer { Name = "Canna Flores", N = 0, P = 2.4425, K = 3.4754, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0.004226, PPM = 5.922126 } },
                { "Canna PH Down", new Fertilizer { Name = "Canna PH Down", N = 2.09, P = 0, K = 0, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 2.09 } },
                { "Canna PK13/14", new Fertilizer { Name = "Canna PK13/14", N = 0, P = 6.0854, K = 12.7431, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 18.8285 } },
                { "Canna Vega", new Fertilizer { Name = "Canna Vega", N = 6.9786, P = 0, K = 2.3167, Mg = 0.5581, Ca = 2.2333, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 12.0867 } },
                { "DryPart Bloom", new Fertilizer { Name = "DryPart Bloom", N = 15.8, P = 19.6, K = 32.9, Mg = 18.5, Ca = 18.5, S = 37, Fe = 0.291, Zn = 0.0396, B = 0.037, Mn = 0.0687, Cu = 0.0396, Mo = 0.0528, PPM = 142.8287 } },
                { "Epsom Salt", new Fertilizer { Name = "Epsom Salt", N = 0, P = 0, K = 0, Mg = 26, Ca = 0, S = 103, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 129 } },
                { "Gypsum", new Fertilizer { Name = "Gypsum", N = 0, P = 0, K = 0, Mg = 0, Ca = 58.12, S = 47.55, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 105.67 } },
                { "Jacks 0-12-26", new Fertilizer { Name = "Jacks 0-12-26", N = 0, P = 13.8, K = 57, Mg = 15.8, Ca = 0, S = 34.3, Fe = 0.792, Zn = 0.0396, B = 0.132, Mn = 0.132, Cu = 0.0396, Mo = 0.0238, PPM = 122.059 } },
                { "Jacks 5-12-26", new Fertilizer { Name = "Jacks 5-12-26", N = 13.209, P = 13.835, K = 57.02, Mg = 16.644, Ca = 0, S = 22.456, Fe = 0.7925, Zn = 0.0396, B = 0.1321, Mn = 0.132, Cu = 0.0396, Mo = 0.0502, PPM = 124.35 } },
                { "Jacks 5-50-18", new Fertilizer { Name = "Jacks 5-50-18", N = 13.2, P = 57.6, K = 39.5, Mg = 2.64, Ca = 0, S = 3.7, Fe = 0.132, Zn = 0.00528, B = 0.0264, Mn = 0.0528, Cu = 0.0106, Mo = 0.00264, PPM = 116.86972 } },
                { "Jacks 7-15-30", new Fertilizer { Name = "Jacks 7-15-30", N = 18.5, P = 17.3, K = 65.8, Mg = 5.283, Ca = 0, S = 25.626, Fe = 0.185, Zn = 0.132, B = 0.0528, Mn = 0.132, Cu = 0.132, Mo = 0.005283, PPM = 133.148083 } },
                { "Jacks 10-30-20", new Fertilizer { Name = "Jacks 10-30-20", N = 26.42, P = 34.59, K = 43.86, Mg = 1.321, Ca = 0, S = 0, Fe = 0.265, Zn = 0.132, B = 0.0528, Mn = 0.132, Cu = 0.132, Mo = 0.00265, PPM = 106.90745 } },
                { "Jacks 12-4-16", new Fertilizer { Name = "Jacks 12-4-16", N = 31.7, P = 4.61, K = 35.1, Mg = 5.28, Ca = 18.5, S = 0, Fe = 0.396, Zn = 0.0925, B = 0.0528, Mn = 0.132, Cu = 0.0528, Mo = 0.00264, PPM = 95.91874 } },
                { "Jacks 15-0-0", new Fertilizer { Name = "Jacks 15-0-0", N = 39.623, P = 0, K = 0, Mg = 0, Ca = 47.55, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 87.173 } },
                { "Jacks 15-5-20", new Fertilizer { Name = "Jacks 15-5-20", N = 39.622, P = 5.76, K = 43.9, Mg = 3.96, Ca = 7.92, S = 0, Fe = 0.396, Zn = 0.132, B = 0.0528, Mn = 0.211, Cu = 0.0528, Mo = 0.00264, PPM = 102.00924 } },
                { "Jacks 15-6-17", new Fertilizer { Name = "Jacks 15-6-17", N = 39.622, P = 6.917, K = 37.28, Mg = 4.755, Ca = 10.567, S = 0, Fe = 0.6076, Zn = 0.13208, B = 0.0528, Mn = 0.132, Cu = 0.026415, Mo = 0.023774, PPM = 100.115669 } },
                { "Jacks 18-8-23", new Fertilizer { Name = "Jacks 18-8-23", N = 47.55, P = 9.22, K = 50.4, Mg = 1.32, Ca = 0, S = 4.2, Fe = 0.396, Zn = 0.132, B = 0.0528, Mn = 0.132, Cu = 0.0528, Mo = 0.00264, PPM = 113.45824 } },
                { "Jacks 20-10-20", new Fertilizer { Name = "Jacks 20-10-20", N = 52.89, P = 11.53, K = 43.86, Mg = 2.0013, Ca = 0, S = 12.945, Fe = 0.39626, Zn = 0.132, B = 0.0528, Mn = 0.1981, Cu = 0.132, Mo = 0.0023775, PPM = 124.1398375 } },
                { "Jacks 20-20-20", new Fertilizer { Name = "Jacks 20-20-20", N = 52.89, P = 23.06, K = 43.86, Mg = 0, Ca = 0, S = 0, Fe = 0.265, Zn = 0.132, B = 0.0528, Mn = 0.132, Cu = 0.132, Mo = 0.00265, PPM = 120.52645 } },
                { "K-Trate LX", new Fertilizer { Name = "K-Trate LX", N = 37, P = 5.76, K = 83.3, Mg = 0, Ca = 0, S = 0, Fe = 0.185, Zn = 0.0925, B = 0.037, Mn = 0.0925, Cu = 0.0185, Mo = 0.0185, PPM = 126.504 } },
                { "MagNit", new Fertilizer { Name = "MagNit", N = 29.1, P = 0, K = 0, Mg = 25.4, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 54.5 } },
                { "Mag-Trate LX", new Fertilizer { Name = "Mag-Trate LX", N = 26.4, P = 0, K = 0, Mg = 23.8, Ca = 0, S = 0, Fe = 0.132, Zn = 0.066, B = 0.01, Mn = 0.025, Cu = 0.005, Mo = 0.005, PPM = 50.443 } },
                { "MAP", new Fertilizer { Name = "MAP", N = 31.7, P = 70.3, K = 0, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 102 } },
                { "Koolbloom", new Fertilizer { Name = "Koolbloom", N = 2, P = 45, K = 28, Mg = 1, Ca = 0, S = 1.5, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0.001, PPM = 77.501 } },
                { "L.Koolbloom", new Fertilizer { Name = "L.Koolbloom", N = 0, P = 11.5, K = 21.9, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 33.4 } },
                { "MaxiBloom", new Fertilizer { Name = "MaxiBloom", N = 13.2, P = 17.2, K = 30.8, Mg = 9.2, Ca = 13.2, S = 10.6, Fe = 0.264, Zn = 0, B = 0, Mn = 0, Cu = 0.015, Mo = 0.000405, PPM = 94.479405 } },
                { "MaxiGrow", new Fertilizer { Name = "MaxiGrow", N = 26.42, P = 5.77, K = 30.7, Mg = 5.3, Ca = 15.9, S = 8, Fe = 0.317, Zn = 0, B = 0, Mn = 0.133, Cu = 0, Mo = 0.005283, PPM = 92.545283 } },
                { "Megacrop", new Fertilizer { Name = "Megacrop", N = 25.705, P = 9.01, K = 33.39, Mg = 7.95, Ca = 21.2, S = 10.6, Fe = 0.3975, Zn = 0.159, B = 0.0689, Mn = 0.0742, Cu = 0.0212, Mo = 0.0345, PPM = 108.6103 } },
                { "Megacrop A", new Fertilizer { Name = "Megacrop A", N = 29.979, P = 14.283, K = 65.3315, Mg = 7.9615, Ca = 0, S = 10.6593, Fe = 0.4629, Zn = 0.1323, B = 0.1349, Mn = 0.611, Cu = 0.0079, Mo = 0.0032, PPM = 129.5665 } },
                { "Megacrop 'B'", new Fertilizer { Name = "Megacrop 'B'", N = 40.9975, P = 0, K = 0, Mg = 0, Ca = 50.255, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 91.2525 } },
                { "MC Cal-Mag", new Fertilizer { Name = "MC Cal-Mag", N = 34.34, P = 0, K = 0, Mg = 9.25, Ca = 29.1, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 72.69 } },
                { "MC Sweet Candy", new Fertilizer { Name = "MC Sweet Candy", N = 0, P = 0, K = 35.1, Mg = 14.5, Ca = 0, S = 34.3, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 83.9 } },
                { "MOAB", new Fertilizer { Name = "MOAB", N = 0, P = 59.95, K = 70.17, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 130.12 } },
                { "Monster Bloom", new Fertilizer { Name = "Monster Bloom", N = 0, P = 57.6, K = 65.8, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 123.4 } },
                { "MPK", new Fertilizer { Name = "MPK", N = 0, P = 59.95, K = 74.56, Mg = 0, Ca = 0, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 134.51 } },
                { "PP CalKick", new Fertilizer { Name = "PP CalKick", N = 39.623, P = 0, K = 30.7, Mg = 29.06, Ca = 0, S = 0, Fe = 0.6604, Zn = 0.1323, B = 0.0528, Mn = 0.1323, Cu = 0.1323, Mo = 0.039625, PPM = 100.532725 } },
                { "PP Bloom", new Fertilizer { Name = "PP Bloom", N = 26.415, P = 34.59, K = 43.86, Mg = 3.434, Ca = 0, S = 4.491, Fe = 0.6604, Zn = 0.1323, B = 0.0528, Mn = 0.1323, Cu = 0.1323, Mo = 0.007925, PPM = 113.908025 } },
                { "PP Boost", new Fertilizer { Name = "PP Boost", N = 39.623, P = 34.59, K = 32.9, Mg = 0, Ca = 0, S = 4.491, Fe = 0.26415, Zn = 0.1323, B = 0.0528, Mn = 0.1323, Cu = 0.1323, Mo = 0.013209, PPM = 112.331059 } },
                { "PP Grow", new Fertilizer { Name = "PP Grow", N = 31.7, P = 9.224, K = 57.02, Mg = 6.604, Ca = 0, S = 13.474, Fe = 0.6604, Zn = 0.1323, B = 0.0528, Mn = 0.1323, Cu = 0.1323, Mo = 0.013209, PPM = 119.145309 } },
                { "PP Finisher", new Fertilizer { Name = "PP Finisher", N = 10.566, P = 35.74, K = 81.14, Mg = 0, Ca = 0, S = 0, Fe = 0.26415, Zn = 0.1323, B = 0.0528, Mn = 0.1323, Cu = 0.1323, Mo = 0.013209, PPM = 128.173059 } },
                { "PP Spike", new Fertilizer { Name = "PP Spike", N = 0, P = 0, K = 0, Mg = 7.132, Ca = 14.265, S = 0, Fe = 0, Zn = 0, B = 0, Mn = 0, Cu = 0, Mo = 0, PPM = 21.397 } }
            };
        }

        public Dictionary<string, double> CalculateTotals(Dictionary<string, Entry> fertilizerEntryMappings)
        {
            var totals = new Dictionary<string, double>
            {
                {"N", 0},
                {"P", 0},
                {"K", 0},
                {"Mg", 0},
                {"Ca", 0},
                {"S", 0},
                {"Fe", 0},
                {"Zn", 0},
                {"B", 0},
                {"Mn", 0},
                {"Cu", 0},
                {"Mo", 0},
                {"PPM", 0}
            };

            foreach (var mapping in fertilizerEntryMappings)
            {
                if (double.TryParse(mapping.Value.Text, out double quantity))
                {
                    var fertilizer = fertilizers[mapping.Key];
                    totals["N"] += quantity * fertilizer.N;
                    totals["P"] += quantity * fertilizer.P;
                    totals["K"] += quantity * fertilizer.K;
                    totals["Mg"] += quantity * fertilizer.Mg;
                    totals["Ca"] += quantity * fertilizer.Ca;
                    totals["S"] += quantity * fertilizer.S;
                    totals["Fe"] += quantity * fertilizer.Fe;
                    totals["Zn"] += quantity * fertilizer.Zn;
                    totals["B"] += quantity * fertilizer.B;
                    totals["Mn"] += quantity * fertilizer.Mn;
                    totals["Cu"] += quantity * fertilizer.Cu;
                    totals["Mo"] += quantity * fertilizer.Mo;
                    totals["PPM"] += quantity * fertilizer.PPM;
                }
            }

            return totals;
        }
    }
}
