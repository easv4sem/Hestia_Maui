using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hestia_Maui.PlatformSpecific
{
    public class PlatformSpecificGreeting
    {

        /// <summary>
        /// Returns a platform-specific greeting using compile-time directives
        /// </summary>
        /// <returns>A greeting string based on the target platform</returns>
        public string GetGreetingCompile()
        {

#if ANDROID
            return "Hej fra Android!";
#elif IOS
            return "Hej fra iOS!";
#else
            return "Hej fra en anden platform!";
#endif
        }


        /// <summary>
        /// Returns a platform-specific greeting using runtime platform detection
        /// </summary>
        /// <returns>A greeting string based on the platform detected at runtime</returns>
        public string GetGreetingRuntime()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
                return "Hej fra Android!";

            else if (DeviceInfo.Platform == DevicePlatform.iOS)
                return "Hej fra iOS!";

            else
                return "Hej fra en anden platform!";
        }
    }
}




