using Moq;
using Npgsql;
using Xunit;
using TestMonopol.Models;
using TestMonopol.Managers;
using System.Collections.Generic;
using Assert = Xunit.Assert;


public class DatabasePalletManagerTests
{
    [Fact]
    public void ProcessPallets_ShouldLoadDataFromDatabase()
    {
        var mockConnection = new Mock<NpgsqlConnection>("Host=localhost;Username=postgres;Password=1234;Database=monopol_test");
        var manager = new DatabasePalletManager();

        var pallets = manager.ProcessPallets();

        // Проверка что данные загружены
        Assert.NotEmpty(pallets);
    }
}
