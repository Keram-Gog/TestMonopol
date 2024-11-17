using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopol.Models
{
    public class Pallet : WarehouseItem
    {
        private List<Box> _boxes = new List<Box>();

        public Pallet(string id, double width, double height, double depth)
            : base(id, width, height, depth, 30.0) // Вес паллеты 30 кг
        {
        }

        public void AddBox(Box box)
        {
            if (box.Width > Width || box.Depth > Depth)
            {
                throw new InvalidOperationException("Коробка превышает размеры паллеты по ширине или глубине.");
            }
            _boxes.Add(box);
        }

        public IReadOnlyList<Box> Boxes => _boxes.AsReadOnly();

        public override double Volume => Width * Height * Depth + _boxes.Sum(box => box.Volume);

        public override double Weight => 30 + _boxes.Sum(box => box.Weight);

        public DateTime? GetExpirationDate()
        {
            if (_boxes.Count == 0)
                return null;

            return _boxes.Min(box => box.GetExpirationDate());
        }
    }

}
