using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp1
{
    public class PositionData
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public int Rent0 { get; set; }

        public int Rent1 { get; set; }

        public int Rent2 { get; set; }

        public int Rent3 { get; set; }

        public int Rent4 { get; set; }

        public int Rent5 { get; set; }

        public string Group { get; set; }

        public string HouseCost { get; set; }

        public string Owner { get; set; }

        public bool Morguage { get; set; }

        public byte SetLevel { get; set; }

        public byte HouseLevel { get; set; }

        public SolidColorBrush Kleur { get; set; }
    }
}
