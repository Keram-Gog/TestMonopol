using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopol.Managers
{
    public class PalletData
    {
        public string PalletID { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public List<BoxData> Boxes { get; set; }
    }
}
