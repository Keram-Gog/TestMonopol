using System;
using System.Collections.Generic;
using TestMonopol.Models;

namespace TestMonopol.Managers
{
    public class RandomPalletManager : LogicManager
    {
        private static Random _random = new Random();
        public override List<Pallet> ProcessPallets()
        {

            Console.Write("Введите количество паллет для генерации: ");
            int palletCount = int.Parse(Console.ReadLine());

            var pallets = new List<Pallet>();
            for (int i = 0; i < palletCount; i++)
            {
                string palletId = $"Pallet-{i + 1}";
                double width = _random.Next(100, 200);
                double height = _random.Next(150, 200);
                double depth = _random.Next(100, 150);

                var pallet = new Pallet(palletId, width, height, depth);
                AddRandomBoxesToPallet(pallet);
                pallets.Add(pallet);
            }

            return pallets;
        }

        private void AddRandomBoxesToPallet(Pallet pallet)
        {
            int boxCount = _random.Next(3, 6);
            for (int i = 0; i < boxCount; i++)
            {
                string boxId = $"{pallet.ID}-Box-{i + 1}";
                double width = _random.Next(30, 60);
                double height = _random.Next(10, 40);
                double depth = _random.Next(30, 60);
                double weight = _random.Next(5, 20);

                DateTime? productionDate = _random.Next(0, 2) == 0
                    ? (DateTime?)DateTime.Today.AddDays(-_random.Next(1, 100))
                    : null;

                DateTime? expirationDate = productionDate.HasValue
                ? productionDate.Value.AddDays(100)
                    : (DateTime?)DateTime.Today.AddDays(_random.Next(1, 100));

                Box box = productionDate.HasValue
                    ? new Box(boxId, width, height, depth, weight, productionDate: productionDate.Value)
                    : new Box(boxId, width, height, depth, weight, expirationDate: expirationDate.Value);

                pallet.AddBox(box);
            }
        }
    }
}
