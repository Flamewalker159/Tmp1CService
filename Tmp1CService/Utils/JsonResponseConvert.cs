using Newtonsoft.Json;

namespace Tmp1CService.Utils;

public static class JsonResponseConvert
{
    public static string JsonResponse(string message)
    {
        return JsonConvert.SerializeObject(new { Message = message });
    }

    public static List<T> JsonDeserializeResponse<T>(string responseObject)
    {
        var settings = JsonSettings.GetSettingsWithDayFirstFormat();
        return JsonConvert.DeserializeObject<List<T>>(responseObject, settings)!;
    }
    
    public static Dictionary<string, T> JsonDeserializeResponseToDictionary<T>(string responseObject)
    {
        var settings = JsonSettings.GetSettingsWithDayFirstFormat();
        return JsonConvert.DeserializeObject<Dictionary<string, T>>(responseObject, settings)!;
    }
}