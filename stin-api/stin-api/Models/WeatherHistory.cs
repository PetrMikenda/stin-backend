namespace stin_api.Models
{
    public class List
    {
        public int dt { get; set; }
        public MainHistory main { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class MainHistory
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class WeatherHistory
    {
        public string message { get; set; }
        public string cod { get; set; }
        public int city_id { get; set; }
        public double calctime { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
    }
}
