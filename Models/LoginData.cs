using System.Text.Json.Serialization;

namespace Hestia_Maui.Models
{
    class LoginData
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
