using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Hestia_Maui.Models
{
    public class PutDeviceRequest
    {
        [JsonPropertyName("device")]
        public required DeviceData Device { get; set; }
    }
}
