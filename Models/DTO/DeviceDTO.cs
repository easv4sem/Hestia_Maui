using Hestia_Maui.Models.DTO;


namespace Hestia_Maui.Models.DTO
{
    public class DeviceDTO
    {
        public string PIUniqueIdentifier { get; set; }
        public string PIDisplayName { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Mac { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Version { get; set; }
        public List<SensorDTO> Sensors { get; set; }
        public int Mode { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
}
