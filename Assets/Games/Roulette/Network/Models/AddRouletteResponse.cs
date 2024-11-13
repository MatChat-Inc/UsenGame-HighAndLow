using Newtonsoft.Json;

namespace Usen.Models
{
    public class AddRouletteResponse: Response
    {
        [JsonProperty("roulette_id")]
        public string id;
    }
}