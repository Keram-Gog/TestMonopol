using Xunit;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using TestMonopol.Models;
using TestMonopol.Managers;
using Assert = Xunit.Assert;


public class FilePalletManagerTests
{
    [Fact]
    public void ProcessPallets_ShouldLoadDataFromFile()
    {
        // Устанавливаем путь к тестовому файлу
        string testFilePath = "test_pallets.json";

        // Создаем тестовые данные для JSON
        var testJson = new List<PalletData>
        {
            new PalletData
            {
                PalletID = "Pallet-1",
                Width = 120,
                Height = 150,
                Depth = 130,
                Boxes = new List<BoxData>
                {
                    new BoxData
                    {
                        BoxID = "Pallet-1-Box-1",
                        Width = 40,
                        Height = 20,
                        Depth = 30,
                        Weight = 15
                    }
                }
            }
        };

        // Записываем тестовые данные в файл
        File.WriteAllText(testFilePath, JsonSerializer.Serialize(testJson));

        // Создаем объект менеджера
        var manager = new FilePalletManager();

        // Тестируем загрузку паллет из файла
        var pallets = manager.ProcessPallets();

        // Проверяем, что данные были загружены корректно
        Assert.Single(pallets);
        Assert.Equal("Pallet-1", pallets[0].ID);
        Assert.Single(pallets[0].Boxes);

        // Удаляем файл после завершения теста, чтобы избежать остатков
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }
}
