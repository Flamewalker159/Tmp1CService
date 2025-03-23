using System.Globalization;
using Newtonsoft.Json;

namespace Tmp1CService.Utils;

public static class JsonSettings
{
    public static JsonSerializerSettings GetSettingsWithDayFirstFormat()
    {
        return new JsonSerializerSettings
        {
            DateFormatString = "dd.MM.yyyy H:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };
    }

    public static JsonSerializerSettings GetSettingsWithMonthFirstFormat()
    {
        return new JsonSerializerSettings
        {
            DateFormatString = "MM.dd.yyyy HH:mm:ss",
            Culture = CultureInfo.InvariantCulture
        };
    }
}