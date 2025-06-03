using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hestia_Maui.Models
{
    public class DeviceData
    {
        [JsonPropertyName("Mac")]
        public string Id { get; set; }

        [JsonPropertyName("PIDisplayName")]
        public string DisplayName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
}
