using Newtonsoft.Json;

namespace WeatherApi.Models
{
    public class WeatherDescription
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
