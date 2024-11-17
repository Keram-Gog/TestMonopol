using Xunit;
using System;
using System.Linq;
using TestMonopol.Models;
using Assert = Xunit.Assert;


namespace TestMonopol.Tests
{
    public class PalletTests
    {
        [Fact]
        public void Pallet_ShouldCalculateVolumeCorrectly()
        {
            // Arrange
            var pallet = new Pallet("Pallet1", 5, 5, 5);
            var box1 = new Box("Box1", 2, 2, 2, 5);
            var box2 = new Box("Box2", 2, 2, 2, 5);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            var volume = pallet.Volume;

            // Assert
            Assert.Equal(5 * 5 * 5 + (2 * 2 * 2 + 2 * 2 * 2), volume);
        }

        [Fact]
        public void Pallet_ShouldThrowException_WhenBoxExceedsDimensions()
        {
            // Arrange
            var pallet = new Pallet("Pallet2", 5, 5, 5);
            var largeBox = new Box("Box3", 6, 6, 6, 10);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => pallet.AddBox(largeBox));
            Assert.Equal("Коробка превышает размеры паллеты по ширине или глубине.", exception.Message);
        }

        [Fact]
        public void Pallet_ShouldCalculateWeightCorrectly()
        {
            // Arrange
            var pallet = new Pallet("Pallet3", 5, 5, 5);
            var box1 = new Box("Box1", 2, 2, 2, 5);
            var box2 = new Box("Box2", 2, 2, 2, 5);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            var weight = pallet.Weight;

            // Assert
            Assert.Equal(30 + 5 + 5, weight); // Паллеты вес 30 кг, плюс вес коробок
        }

        [Fact]
        public void Pallet_ShouldReturnNull_WhenNoBoxes()
        {
            // Arrange
            var pallet = new Pallet("Pallet4", 5, 5, 5);

            // Act
            var expirationDate = pallet.GetExpirationDate();

            // Assert
            Assert.Null(expirationDate);
        }

        [Fact]
        public void Pallet_ShouldReturnMinExpirationDate_WhenBoxesHaveDifferentExpirationDates()
        {
            // Arrange
            var pallet = new Pallet("Pallet5", 5, 5, 5);
            var box1 = new Box("Box1", 2, 2, 2, 5, new DateTime(2025, 12, 31));
            var box2 = new Box("Box2", 2, 2, 2, 5, new DateTime(2024, 6, 30));

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            var expirationDate = pallet.GetExpirationDate();

            // Assert
            Assert.Equal(new DateTime(2024, 6, 30), expirationDate);
        }
    }
}
