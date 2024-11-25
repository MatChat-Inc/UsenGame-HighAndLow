using Newtonsoft.Json;

namespace USEN.Roulette
{
    public class AddRouletteResponse: Response
    {
        [JsonProperty("roulette_id")]
        public string id;
    }
}