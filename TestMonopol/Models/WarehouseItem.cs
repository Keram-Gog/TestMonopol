using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopol.Models
{
    public abstract class WarehouseItem
    {
        public string ID { get; set; }
        public double Width { get; set; }    // Ширина
        public double Height { get; set; }   // Высота
        public double Depth { get; set; }    // Глубина
        public virtual double Weight { get; set; }   // Вес

        public WarehouseItem(string id, double width, double height, double depth, double weight)
        {
            ID = id;
            Width = width;
            Height = height;
            Depth = depth;
            Weight = weight;
        }

        // Абстрактное свойство для объема
        public abstract double Volume { get; }
    }

}
