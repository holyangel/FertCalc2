using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertCalc2
{
    public class Fertilizer
    {
        public string Name { get; set; }
        public double N { get; set; } // Nitrogen
        public double P { get; set; } // Phosphorus
        public double K { get; set; } // Potassium
        public double Mg { get; set; } // Magnesium
        public double Ca { get; set; } // Calcium
        public double S { get; set; } // Sulfur
        public double Fe { get; set; } // Iron
        public double Zn { get; set; } // Zinc
        public double B { get; set; } // Boron
        public double Mn { get; set; } // Manganese
        public double Cu { get; set; } // Copper
        public double Mo { get; set; } // Molybdenum
        public double PPM { get; set; } // Total parts per million

        // Constructor to initialize a Fertilizer object with all nutrient values
        public Fertilizer(string name, double n, double p, double k, double mg, double ca, double s, double fe, double zn, double b, double mn, double cu, double mo, double ppm)
        {
            Name = name;
            N = n;
            P = p;
            K = k;
            Mg = mg;
            Ca = ca;
            S = s;
            Fe = fe;
            Zn = zn;
            B = b;
            Mn = mn;
            Cu = cu;
            Mo = mo;
            PPM = ppm;
        }

        // Default constructor
        public Fertilizer() { }

    }
}
