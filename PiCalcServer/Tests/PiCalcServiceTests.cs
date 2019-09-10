using System;
using Extreme.Mathematics;
using FluentAssertions;
using PiCalcServer.Services;
using Xunit;

namespace PiCalcServer.Tests
{
    public class PiCalcServiceTests
    {
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        public void TestExtremeMathPi(int precision)
        {
            var pi = BigFloat.GetPi(AccuracyGoal.Absolute(precision));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void CalculateHundedThousandDigitsOfPi(int precision)
        {
            // Arrange
            var picalcService = new PiCalcService();
            var expectedPi = BigFloat.GetPi(
                AccuracyGoal.Absolute(precision),
                new RoundingMode());
            // Act
            var pi = picalcService.Calculate(precision, () => false);

            // Assert
            pi.Should().Be(expectedPi);
        }

        [Fact]
        public void CalculateFirstDigitOfPi()
        {
            // Arrange
            var picalcService = new PiCalcService();

            // Act
            var pi = picalcService.Calculate(15, () => false);

            // Assert
            pi.Should().Be(Math.PI);
        }
    }
}