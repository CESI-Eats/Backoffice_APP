using Newtonsoft.Json;

namespace Backoffice_APP.Models.Requests
{
    public class LoginRequest
    {
        [JsonProperty("mail")]
        public string? Mail { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
