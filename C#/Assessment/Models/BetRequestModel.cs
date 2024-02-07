using Newtonsoft.Json;

namespace Assessment.Models
{
    public class BetRequestModel
    {
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("betNumber")]
        public int BetNumber { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}