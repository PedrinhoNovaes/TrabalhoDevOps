using Microsoft.Extensions.Logging;
using Moq;
using TrabalhoDevOps.Controllers;

namespace TrabalhoDevOps.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_ReturnsFiveWeatherForecasts()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            var forecasts = Assert.IsType<WeatherForecast[]>(result);
            Assert.Equal(5, forecasts.Length);
        }

        [Fact]
        public void Get_ReturnsValidWeatherForecasts()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            var forecasts = Assert.IsType<WeatherForecast[]>(result);
            Assert.Equal(5, forecasts.Length);
            foreach (var forecast in forecasts)
            {
                Assert.InRange(forecast.TemperatureC, -20, 55); // Temperature range check
                Assert.Contains(forecast.Summary, WeatherForecastController.Summaries); // Valid summary check
            }
        }
    }
}
