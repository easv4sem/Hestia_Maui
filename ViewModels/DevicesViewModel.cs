using Hestia_Maui.Models;

namespace Hestia_Maui.ViewModels
{
    public partial class DevicesViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceDisplayProps> Devices { get; set; }

        public DevicesViewModel()
        {
            InitializeDevices();
            
            
        }


        private void InitializeDevices() {
            TestDevices();
        }


        private void TestDevices()
        { 
            Devices = new ObservableCollection<DeviceDisplayProps>
            {
                new DeviceDisplayProps { Name = "Lottenhoi", MacId ="AA:BB:CC:00:00:11", Longitude = 12.34567f, Latitude = 55.67891f, HasHeartbeat = true },
                new DeviceDisplayProps { Name = "Skydebane", MacId = "AA:BB:CC:00:00:11", Longitude = 12.45678f, Latitude = 55.78901f, HasHeartbeat = false  },
                new DeviceDisplayProps { Name = "Byskoven" , MacId = "AA:BB:CC:00:00:11",Longitude = 12.56789f, Latitude = 55.89012f, HasHeartbeat = false  },
                new DeviceDisplayProps { Name = "Lottenhoi", MacId ="AA:BB:CC:00:00:11", Longitude = 12.34567f, Latitude = 55.67891f, HasHeartbeat = true  },
                new DeviceDisplayProps { Name = "Skydebane", MacId = "AA:BB:CC:00:00:11", Longitude = 12.45678f, Latitude = 55.78901f, HasHeartbeat = true  },
                new DeviceDisplayProps { Name = "Byskoven" , MacId = "AA:BB:CC:00:00:11",Longitude = 12.56789f, Latitude = 55.89012f, HasHeartbeat = true  },
                new DeviceDisplayProps { Name = "Lottenhoi", MacId ="AA:BB:CC:00:00:11", Longitude = 12.34567f, Latitude = 55.67891f, HasHeartbeat = true  },
                new DeviceDisplayProps { Name = "Skydebane", MacId = "AA:BB:CC:00:00:11", Longitude = 12.45678f, Latitude = 55.78901f, HasHeartbeat = true  },
                new DeviceDisplayProps { Name = "Byskoven" , MacId = "AA:BB:CC:00:00:11",Longitude = 12.56789f, Latitude = 55.89012f, HasHeartbeat = false  }
            };
        }
    }
}
