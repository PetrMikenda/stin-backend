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
            if (weatherCode == 803 || weatherCode == 804)
            {
                return "https://openweathermap.org/img/wn/04d@2x.png";
            }
            return " ";

        }

        [HttpGet("Weather/GetHistory/{locationName}")]
        public async Task<List<double>> GetWeatherHistory(string locationName)
        {
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

            DateTime now = DateTime.Now.Date.AddHours(12);
            List<WeatherHistory> weatherHistory = new List<WeatherHistory>();
            List<string> urls = new List<string>();
            for (int i = 1; i < 6; i++)
            {
                // Datum a èas i dní zpìt
                DateTime targetDate = now.AddDays(-i);

                // Pøevod na unixový èas
                long unixTime = ((DateTimeOffset)targetDate).ToUnixTimeSeconds();
                // Výpis výsledku
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        //string url = $"http://api.openweathermap.org/geo/1.0/direct?q={locationName}&limit=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a";
                        string url = $"https://history.openweathermap.org/data/2.5/history/city?lat={location[0].lat.ToString().Replace(',', '.')}&lon={location[0].lon.ToString().Replace(',', '.')}&type=day&start={unixTime}&cnt=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string body = await response.Content.ReadAsStringAsync();
                        
                        WeatherHistory weatherH = JsonSerializer.Deserialize<WeatherHistory>(body);
                        Debug.WriteLine(weatherH.list[0].main.temp);
                        weatherHistory.Add(weatherH);
                        //location = JsonSerializer.Deserialize<List<Location>>(body);
                    }
                    catch (HttpRequestException e)
                    {
                        //Debug.WriteLine(body);
                        throw new HttpRequestException();
                    }
                }
                //urls.Add($"https://history.openweathermap.org/data/2.5/history/city?lat={location[0].lat.ToString().Replace(',','.')}&lon={location[0].lon.ToString().Replace(',', '.')}&type=day&start={unixTime}&cnt=1&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric");
            }
            //return urls;
            List<double> historyTemps = new List<double>();
            foreach (WeatherHistory item in weatherHistory)
            {
                historyTemps.Add(item.list[0].main.temp);
            }

            return historyTemps;
        }

        [HttpGet("Weather/GetForecast/{locationName}")]
        public async Task<List<double>> GetWeatherForecast(string locationName)
        {
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

            DateTime now = DateTime.Now.Date.AddHours(12);
            List<Forecast> weatherForecast= new List<Forecast>();
            Forecast? weatherF = new Forecast();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://api.openweathermap.org/data/2.5/forecast/daily?lat={location[0].lat.ToString().Replace(',', '.')}&lon={location[0].lon.ToString().Replace(',', '.')}&cnt=5&appid=ab1ac6a8d5738d1dd4f46e7cae913c0a&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();

                    weatherF = JsonSerializer.Deserialize<Forecast>(body);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            List<double> forecast = new List<double>();

            foreach(ForecastList f in weatherF.list)
            {
                forecast.Add(f.temp.day);
            }

            

            return forecast;
        }

    }
}
