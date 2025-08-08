using Newtonsoft.Json;

namespace WeatherApi.Models
{
    public class WeatherApiResponse
    {
        [JsonProperty("name")]
        public string CityName { get; set; }

        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherDescription> Weather { get; set; }
    }
}
