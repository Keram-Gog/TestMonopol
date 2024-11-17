using Xunit;
using Moq;
using TestMonopol.Models;
using TestMonopol.Managers;
using System.Collections.Generic;
using Assert = Xunit.Assert;


public class UserInputPalletManagerTests
{
    [Fact]
    public void ProcessPallets_ShouldAcceptUserInputForPallets()
    {
        var mockInput = new Mock<IConsoleInput>();
        mockInput.SetupSequence(input => input.ReadLine())
                 .Returns("2")  // Количество паллет
                 .Returns("Pallet-1")  // ID первой паллеты
                 .Returns("100")  // Ширина первой паллеты
                 .Returns("150")  // Высота первой паллеты
                 .Returns("120")  // Глубина первой паллеты
                 .Returns("Pallet-2")  // ID второй паллеты
                 .Returns("120")  // Ширина второй паллеты
                 .Returns("160")  // Высота второй паллеты
                 .Returns("130");  // Глубина второй паллеты

        var manager = new UserInputPalletManager();
        var pallets = manager.ProcessPallets();

        Assert.Equal(2, pallets.Count);
        Assert.Equal("Pallet-1", pallets[0].ID);
        Assert.Equal("Pallet-2", pallets[1].ID);
    }
}
