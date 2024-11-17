using Xunit;
using System;
using TestMonopol.Models;
using Assert = Xunit.Assert;

namespace TestMonopol.Tests
{
    public class BoxTests
    {
        [Fact]
        public void Box_ShouldCalculateVolumeCorrectly()
        {
            // Arrange
            var box = new Box("Box1", 2, 3, 4, 5);

            // Act
            var volume = box.Volume;

            // Assert
            Assert.Equal(2 * 3 * 4, volume);
        }

        [Fact]
        public void Box_ShouldThrowException_WhenNoExpirationDateOrProductionDate()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Box("Box2", 1, 1, 1, 1, null, null));
            Assert.Equal("Должен быть указан либо срок годности, либо дата производства.", exception.Message);
        }

        [Fact]
        public void Box_ShouldReturnExpirationDate_WhenExpirationDateIsSet()
        {
            // Arrange
            var expirationDate = new DateTime(2025, 12, 31);
            var box = new Box("Box3", 1, 1, 1, 1, expirationDate);

            // Act
            var boxExpirationDate = box.GetExpirationDate();

            // Assert
            Assert.Equal(expirationDate, boxExpirationDate);
        }

        [Fact]
        public void Box_ShouldSetExpirationDate_WhenOnlyProductionDateIsSet()
        {
            // Arrange
            var productionDate = new DateTime(2025, 1, 1);
            var box = new Box("Box4", 2, 2, 2, 2, null, productionDate);

            // Act
            var boxExpirationDate = box.GetExpirationDate();

            // Assert
            Assert.Equal(productionDate.AddDays(100), boxExpirationDate);
        }
    }
}
