using Newtonsoft.Json;

namespace WebApiOAuthCallApp
{
    public class User
    {
        [JsonProperty("displayName")]
        public string Name { get; set; }
        [JsonProperty("mail")]
        public string Email { get; set; }
    }
}
