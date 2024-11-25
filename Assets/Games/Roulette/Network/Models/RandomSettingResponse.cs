using Newtonsoft.Json;

namespace USEN.Games.Roulette
{
    public class RandomSettingResponse: Response
    {
        [JsonProperty("random")]
        public int random;
        
        public bool IsRandom => random == 1;
    }
}