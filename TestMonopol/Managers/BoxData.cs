using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopol.Managers
{
    public class BoxData
    {
        public string BoxID { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ProductionDate { get; set; }
    }
}
