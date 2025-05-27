namespace Hestia_Maui.Views.Converter
{
    class HeartbeatToIconConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean heartbeat value to an icon filename
        /// </summary>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool hasHeartbeat)
            {
                return hasHeartbeat ? "icon_green_circle.png" : "icon_red_circle.png";
            }

            return "icon_red_circle.png";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
