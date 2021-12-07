using Newtonsoft.Json;

namespace Cinema
{
    public struct ApiConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}