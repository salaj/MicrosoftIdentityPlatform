using System;
using Newtonsoft.Json;

namespace WebApi4OAuthCallApp
{
    public class User
    {
        [JsonProperty("displayName")]
        public string Name { get; set; }
        [JsonProperty("mail")]
        public string Email { get; set; }
    }
}
