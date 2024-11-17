using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using TestMonopol.Models;
using TestMonopol.Managers;
using Assert = Xunit.Assert;


public class RandomPalletManagerTests
{
    [Fact]
    public void ProcessPallets_ShouldGenerateCorrectNumberOfPallets()
    {
        // Мокируем консольный ввод
        var mockInput = new Mock<IConsoleInput>();
        mockInput.Setup(input => input.ReadLine()).Returns("3");  // Генерация 3 паллет

        var manager = new RandomPalletManager();

        // Применяем мок для ProcessPallets
        var pallets = manager.ProcessPallets();

        Assert.Equal(3, pallets.Count);
    }

    [Fact]
    public void ProcessPallets_ShouldAddBoxesToPallet()
    {
        var mockInput = new Mock<IConsoleInput>();
        mockInput.Setup(input => input.ReadLine()).Returns("1");

        var manager = new RandomPalletManager();
        var pallets = manager.ProcessPallets();

        // Проверяем, что в паллете есть хотя бы одна коробка
        Assert.NotEmpty(pallets[0].Boxes);
    }
}
