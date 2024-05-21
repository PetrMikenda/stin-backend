using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using stin_api.Models;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Policy;
using System.Linq;

namespace stin_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // GET: api/Weather/Location
        [HttpGet("Weather/{locationName}")]
        public async Task<ActionResult<double>> Get(string locationName)
        {
            LocationsWeather? weather;

            List<Location>? location;
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"http://api.openweathermap.org/geo/1.0/direct?q={locationName}&limit=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();

                     location = JsonSerializer.Deserialize<List<Location>>(body);
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException();
                }
            }
          
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://api.openweathermap.org/data/2.5/weather?lat={location.FirstOrDefault().lat}&lon={location.FirstOrDefault().lon}&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();

                    weather = JsonSerializer.Deserialize<LocationsWeather>(body);
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException();
                }
            }

            return weather.main.temp;
        }

        // GET: api/Weather/Location
        [HttpGet("Weather/GetIcon/{locationName}")]
        public async Task<string> GetIcon(string locationName)
        {
            LocationsWeather? weather;

            List<Location>? location;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"http://api.openweathermap.org/geo/1.0/direct?q={locationName}&limit=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();

                    location = JsonSerializer.Deserialize<List<Location>>(body);
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException();
                }
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://api.openweathermap.org/data/2.5/weather?lat={location.FirstOrDefault().lat}&lon={location.FirstOrDefault().lon}&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();

                    weather = JsonSerializer.Deserialize<LocationsWeather>(body);
                }
                catch (HttpRequestException e)
                {
                    throw new HttpRequestException();
                }
            }

            int weatherCode = weather.weather[0].id;
            if (weatherCode > 199 && weatherCode < 233)
            {
                return "https://openweathermap.org/img/wn/11d@2x.png";
            }
            if (weatherCode > 299 && weatherCode < 322)
            {
                return "https://openweathermap.org/img/wn/09d@2x.png";
            }
            if(weatherCode > 499 && weatherCode < 505)
            {
                return "https://openweathermap.org/img/wn/10d@2x.png";
            }
            //if (weatherCode == 511)
            //{
            //    return "https://openweathermap.org/img/wn/13d@2x.png";
            //}
            if (weatherCode > 519 && weatherCode < 532)
            {
                return "https://openweathermap.org/img/wn/09d@2x.png";
            }
            if (weatherCode > 599 && weatherCode < 623 || weatherCode == 511)
            {
                return "https://openweathermap.org/img/wn/13d@2x.png";
            }
            if (weatherCode > 700 && weatherCode < 782)
            {
                return "https://openweathermap.org/img/wn/50d@2x.png";
            }
            if(weatherCode == 800)
            {
                return "https://openweathermap.org/img/wn/01d@2x.png";
            }
            if(weatherCode == 801)
            {
                return "https://openweathermap.org/img/wn/02d@2x.png";
            }
            if (weatherCode == 802)
            {
                return "https://openweathermap.org/img/wn/03d@2x.png";
            }
            if (weatherCode == 803 && weatherCode == 804)
            {
                return "https://openweathermap.org/img/wn/04d@2x.png";
            }
            return " ";

        }


    }
}
