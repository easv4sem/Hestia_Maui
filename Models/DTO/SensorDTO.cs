using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hestia_Maui.Models.DTO
{
    public class SensorDTO
    {
        public string UniqueIdentifier { get; set; }
        public string DisplayName { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string Type { get; set; }
    }
}
