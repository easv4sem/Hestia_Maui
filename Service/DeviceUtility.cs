using Hestia_Maui.Models;
using Hestia_Maui.Models.DTO;
using Hestia_Maui.Models.Enums;


namespace Hestia_Maui.Service
{
    public class DeviceUtility
    {

        /// <summary>
        /// Converts and filters a list of DeviceDTO to an ObservableCollection of DeviceDisplayProps,
        /// excluding devices in Setup mode (not active devices). Sets HasHeartbeat based on device mode.
        /// </summary>
        /// <param name="allDevices">List of DeviceDTO</param>
        /// <returns>Filtered ObservableCollection of DeviceDisplayProps</returns>
        public static ObservableCollection<DeviceDisplayProps> ConvertAndSortActiveDevices(List<DeviceDTO> allDevices) {
            
            ObservableCollection<DeviceDisplayProps> tempCollection = new ObservableCollection<DeviceDisplayProps>();

            foreach (DeviceDTO device in allDevices)
            {

                // excludes devices in setup mode (not active devices)
                if ((EDeviceModes)device.Mode != EDeviceModes.Setup)
                {
                    bool hasHeartBeat = false;

                    // marks device as having heartbeat if it's online or in alert mode
                    if ((EDeviceModes)device.Mode == EDeviceModes.Online || (EDeviceModes)device.Mode == EDeviceModes.Alert)
                    {
                        hasHeartBeat = true;
                    }


                    tempCollection.Add(new DeviceDisplayProps
                    {
                        Name = device.PIDisplayName,
                        MacId = device.Mac,
                        Latitude = (float)device.Latitude,
                        Longitude = (float)device.Longitude,
                        HasHeartbeat = hasHeartBeat
                    });


                }
            }

            return tempCollection;

        }

    }
}
