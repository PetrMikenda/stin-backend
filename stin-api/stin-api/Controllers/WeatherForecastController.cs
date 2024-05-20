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

            //Location? location;
            LocationsWeather? weather;
            //string body = "";
            //using (HttpClient c = new HttpClient()) 
            //{

            //    //string url = $"http://api.openweathermap.org/geo/1.0/direct?q={locationName}&limit=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a";
            //    string url = "http://api.openweathermap.org/geo/1.0/direct?q=London&limit=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a";
            //    HttpResponseMessage response = await c.GetAsync(url);



            //    body = await response.Content.ReadAsStringAsync();

            //    Debug.WriteLine(body);
            //    location = JsonSerializer.Deserialize<Location>(body);


            //}
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
            //return location.FirstOrDefault().lon;
            //return $"https://api.openweathermap.org/data/2.5/weather?lat={location.FirstOrDefault().lat}&lon={location.FirstOrDefault().lon}.99&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
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


            //location = JsonConvert.DeserializeObject<Location>(body);

            //using (HttpClient c = new HttpClient())
            //{
            //    string url = $"https://api.openweathermap.org/data/2.5/weather?lat={location.lat}&lon={location.lon}.99&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
            //    HttpResponseMessage response = await c.GetAsync(url);
            //    body = await response.Content.ReadAsStringAsync();
            //    weather = JsonSerializer.Deserialize<LocationsWeather>(body);
            //}

            ////weather = JsonConvert.DeserializeObject<LocationsWeather>(body);

            //if (weather == null)
            //{
            //    return NotFound();
            //}

            return weather.main.temp;
        }
    }
}
