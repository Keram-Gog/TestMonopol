using System;
using System.Collections.Generic;
using TestMonopol.Models;

namespace TestMonopol.Managers
{
    public class UserInputPalletManager : LogicManager
    {
        public override List<Pallet> ProcessPallets()
        {
            var pallets = new List<Pallet>();

            Console.Write("Введите количество паллет: ");
            int palletCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < palletCount; i++)
            {
                Console.WriteLine($"\nВвод данных для паллеты #{i + 1}:");
                Console.Write("Введите ID паллеты: ");
                string palletId = Console.ReadLine();

                Console.Write("Введите ширину паллеты: ");
                double width = double.Parse(Console.ReadLine());

                Console.Write("Введите высоту паллеты: ");
                double height = double.Parse(Console.ReadLine());

                Console.Write("Введите глубину паллеты: ");
                double depth = double.Parse(Console.ReadLine());

                var pallet = new Pallet(palletId, width, height, depth);
                pallets.Add(pallet);
            }

            return pallets;
        }
    }
}
