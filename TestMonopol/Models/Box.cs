using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonopol.Models
{
    public class Box : WarehouseItem
    {
        public DateTime? ExpirationDate { get; private set; }
        public DateTime? ProductionDate { get; private set; }

        public Box(string id, double width, double height, double depth, double weight, DateTime? expirationDate = null, DateTime? productionDate = null)
            : base(id, width, height, depth, weight)
        {
            if (expirationDate.HasValue)
            {
                ExpirationDate = expirationDate.Value.Date;
            }
            else if (productionDate.HasValue)
            {
                ProductionDate = productionDate.Value.Date;
                ExpirationDate = ProductionDate?.AddDays(100);
            }
            else
            {
                throw new ArgumentException("Должен быть указан либо срок годности, либо дата производства.");
            }
        }

        public override double Volume => Width * Height * Depth;

        public DateTime GetExpirationDate()
        {
            if (ExpirationDate.HasValue)
            {
                return ExpirationDate.Value;
            }
            throw new InvalidOperationException("Срок годности не указан.");
        }
    }

}
