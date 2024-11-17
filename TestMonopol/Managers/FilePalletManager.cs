using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TestMonopol.Models;

namespace TestMonopol.Managers
{
    public class FilePalletManager : LogicManager
    {
        public override List<Pallet> ProcessPallets()
        {
            Console.Write("Введите путь к файлу JSON: ");
            string filePath = Console.ReadLine();

            string jsonString = File.ReadAllText(filePath);
            var palletsData = JsonSerializer.Deserialize<List<PalletData>>(jsonString);
            var pallets = new List<Pallet>();

            foreach (var palletData in palletsData)
            {
                var pallet = new Pallet(palletData.PalletID, palletData.Width, palletData.Height, palletData.Depth);

                foreach (var boxData in palletData.Boxes)
                {
                    Box box = boxData.ExpirationDate.HasValue
                        ? new Box(boxData.BoxID, boxData.Width, boxData.Height, boxData.Depth, boxData.Weight, expirationDate: boxData.ExpirationDate.Value)
                        : new Box(boxData.BoxID, boxData.Width, boxData.Height, boxData.Depth, boxData.Weight, productionDate: boxData.ProductionDate.Value);

                    pallet.AddBox(box);
                }

                pallets.Add(pallet);
            }

            return pallets;
        }
    }
}
