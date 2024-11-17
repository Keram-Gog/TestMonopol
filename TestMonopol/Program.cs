using TestMonopol.Managers;
using TestMonopol.Models;

public class Program
{
    public static void Main()
    {
        LogicManager palletManager = null; //Переменная для управления паллетами
        List<Pallet> pallets = new List<Pallet>(); // Список паллет для хранения данных

        while (true)
        {
            Console.WriteLine("\nВыберите способ получения данных для паллет:");
            Console.WriteLine("1. Генерация случайных паллет");
            Console.WriteLine("2. Загрузка из файла");
            Console.WriteLine("3. Загрузка из базы данных");
            Console.WriteLine("4. Ввод вручную");
            Console.WriteLine("5. Выход");

            int choice = int.Parse(Console.ReadLine());

            // Если пользователь выбрал выход, завершает программу
            if (choice == 5)
            {
                Console.WriteLine("Выход из программы.");
                break;
            }

            // Применяем switch-выражение для выбора метода получения данных
            palletManager = choice switch
            {
                1 => new RandomPalletManager(),
                2 => new FilePalletManager(),
                3 => new DatabasePalletManager(),
                4 => new UserInputPalletManager(),
                _ => null
            };

            if (palletManager == null)
            {
                Console.WriteLine("Неверный выбор. Повторите попытку.");
                continue;
            }


            pallets = palletManager.ProcessPallets();
            Console.WriteLine("Паллеты успешно обработаны!");

            // Вход в меню действий с паллетами
            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Отобразить все паллеты");
                Console.WriteLine("2. Отобразить паллеты, сгруппированные по сроку годности и весу");
                Console.WriteLine("3. Отобразить топ-3 паллеты с коробками с наибольшим сроком годности");
                Console.WriteLine("4. Добавить текущие паллеты в базу данных");
                Console.WriteLine("5. Назад к выбору способа получения данных");

                int actionChoice = int.Parse(Console.ReadLine());

                switch (actionChoice)
                {
                    case 1:
                        // Отображаем все паллеты
                        if (pallets.Any())
                            DisplayAllPallets(pallets); 
                        else
                            Console.WriteLine("Нет доступных паллет для отображения.");
                        break;
                    case 2:
                        // Отображаем паллеты, сгруппированные по сроку годности и весу
                        if (pallets.Any())
                        {
                            Console.WriteLine("Сгруппированные паллеты по сроку годности и весу:");
                            DisplayPalletsGroupedByExpirationDate(pallets);
                        }
                        else
                            Console.WriteLine("Нет доступных паллет для отображения.");
                        break;
                    case 3:
                        // Отображаем топ-3 паллет с коробками с наибольшим сроком годности
                        if (pallets.Any())
                        {
                            Console.WriteLine("Топ-3 паллеты с коробками с наибольшим сроком годности, отсортированные по объему:");
                            DisplayTopPalletsByExpirationDate(pallets);
                        }
                        else
                            Console.WriteLine("Нет доступных паллет для отображения.");
                        break;
                    case 4:
                        // Добавляем паллеты в базу данных
                        if (pallets.Any())
                        {
                            DatabasePalletManager adding = new DatabasePalletManager();
                            adding.AddPalletsToDatabase(pallets);
                        }
                        else
                            Console.WriteLine("Нет доступных паллет для добавления в базу данных.");
                        break;
                    case 5:
                        // Возвращаемся в главное меню
                        Console.WriteLine("Возврат к выбору способа получения данных.");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Повторите попытку.");
                        continue;
                }

                if (actionChoice == 5) break; // Возврат в главное меню
            }
        }
    }

    // Метод для отображения паллет, сгруппированных по сроку годности и весу
    private static void DisplayPalletsGroupedByExpirationDate(List<Pallet> pallets)
    {
        var groupedPallets = pallets.GroupBy(p => p.Boxes.Any(b => b.ExpirationDate.HasValue) ? "Срок годности установлен" : "Срок годности не установлен");

        foreach (var group in groupedPallets)
        {
            Console.WriteLine($"\nГруппа: {group.Key}");
            foreach (var pallet in group)
                Console.WriteLine($"  Паллета ID: {pallet.ID}, " +
                    $"Вес коробок: {pallet.Boxes.Sum(b => b.Weight):F2}");
        }
    }

    // Метод для отображения топ-3 паллет по сроку годности, отсортированных по объему
    private static void DisplayTopPalletsByExpirationDate(List<Pallet> pallets)
    {
        var topPallets = pallets
            .Where(p => p.Boxes.Any(b => b.ExpirationDate.HasValue))
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpirationDate))
            .ThenByDescending(p => p.Width * p.Height * p.Depth)
            .Take(3);

        foreach (var pallet in topPallets)
            Console.WriteLine($"Паллета ID: {pallet.ID}, " +
                $"Объем: {pallet.Width * pallet.Height * pallet.Depth:F2}, " +
                $"Срок годности: {pallet.Boxes.Max(b => b.ExpirationDate):yyyy-MM-dd}");
    }
    private static void DisplayAllPallets(List<Pallet> pallets)
    {
        Console.WriteLine("\nВсе доступные паллеты:");
        foreach (var pallet in pallets)
        {
            Console.WriteLine($"Паллета ID: {pallet.ID}, " +
                $"Размеры (ШxВxГ): {pallet.Width}x{pallet.Height}x{pallet.Depth}, " +
                $"Количество коробок: {pallet.Boxes.Count}");
            foreach (var box in pallet.Boxes)
            {
                Console.WriteLine($"  Коробка ID: {box.ID}, " +
                    $"Размеры: {box.Width}x{box.Height}x{box.Depth}, " +
                    $"Вес: {box.Weight}");
                if (box.ProductionDate.HasValue)
                    Console.WriteLine($"    Дата производства: {box.ProductionDate.Value.ToShortDateString()}");
                if (box.ExpirationDate.HasValue)
                    Console.WriteLine($"    Срок годности: {box.ExpirationDate.Value.ToShortDateString()}");
            }
        }
    }

}