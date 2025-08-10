using Newtonsoft.Json;

namespace WeatherApi.Models
{
    public class MainInfo
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
}
