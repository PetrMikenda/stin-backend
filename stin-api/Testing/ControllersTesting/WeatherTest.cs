using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq.Protected;
using Moq;
using stin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using stin_api.Models;
using stin_api.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using stin_api;
using System.Net.Http.Headers;

namespace Testing.ControllersTesting
{
    public class WeatherTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a new database for each test
                .Options;
        }

        private Mock<HttpMessageHandler> CreateHttpMessageHandler(string urlContains, HttpResponseMessage responseMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains(urlContains)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);
            return handlerMock;
        }

        private static HttpResponseMessage CreateHttpResponseMessage(object content)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(content))
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        private WeatherForecastController CreateControllerWithMockedHttpClient(HttpResponseMessage locationResponse, HttpResponseMessage weatherResponse)
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var locationHandlerMock = CreateHttpMessageHandler("geo", locationResponse);
            var weatherHandlerMock = CreateHttpMessageHandler("weather", weatherResponse);

            var locationHttpClient = new HttpClient(locationHandlerMock.Object);
            var weatherHttpClient = new HttpClient(weatherHandlerMock.Object);

            httpClientFactoryMock.SetupSequence(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(locationHttpClient)
                .Returns(weatherHttpClient);

            var controller = new WeatherForecastController(loggerMock.Object);

            return controller;
        }

        [Fact]
        public async Task GetWeatherForecast_ShouldReturnWeatherForecast_WhenCalled()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            result.Should().HaveCount(5);
            result.Should().AllBeOfType<WeatherForecast>();
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_WhenLocationExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().BeOfType<string>();
            result.Should().StartWith("https://openweathermap.org/img/wn/");
        }

        [Fact]
        public async Task GetWeatherHistory_ShouldReturnTemperatureList_WhenLocationExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var locationName = "London";

            // Act
            var result = await controller.GetWeatherHistory(locationName);

            // Assert
            result.Should().HaveCount(5);
            result.Should().AllBeOfType<double>();
        }

        [Fact]
        public async Task GetWeatherForecastForLocation_ShouldReturnTemperatureList_WhenLocationExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var locationName = "London";

            // Act
            var result = await controller.GetWeatherForecast(locationName);

            // Assert
            result.Should().HaveCount(5);
            result.Should().AllBeOfType<double>();
        }


        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForThunderstorm()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 210 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForDrizzle()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 310 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForRain()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 500 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForSnow()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 600 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForClearSky()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 800 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task GetIcon_ShouldReturnIconUrl_ForClouds()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                weather = new List<Weather>
            {
                new Weather { id = 803 }
            }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.GetIcon(locationName);

            // Assert
            result.Should().Be("https://openweathermap.org/img/wn/03d@2x.png");
        }

        [Fact]
        public async Task Get_ShouldReturnTemperature_WhenLocationExists()
        {
            // Arrange
            var locationResponse = CreateHttpResponseMessage(new List<Location>
        {
            new Location { lat = 51.5074, lon = -0.1278 }
        });

            var weatherResponse = CreateHttpResponseMessage(new LocationsWeather
            {
                main = new Main { temp = 20.0 }
            });

            var controller = CreateControllerWithMockedHttpClient(locationResponse, weatherResponse);
            var locationName = "London";

            // Act
            var result = await controller.Get(locationName);

            // Assert
            result.Value.Should().Be(12.81);
        }
    }
}
