using System.Net.Http.Headers;
using Tmp1CService.Models;

namespace Tmp1CService.Utils;

public static class UrlConverter
{
    public static string GetEmployeesUrl(Client client)
    {
        return $"{client.Url1C}/hs/employees";
    }

    public static string GetEmployeeUrl(Client client, string empCode)
    {
        return $"{client.Url1C}/hs/employees/code/{empCode}";
    }

    public static string GetVehiclesUrl(Client client)
    {
        return $"{client.Url1C}/hs/vehicles";
    }
    
    public static string GetVehicleUrl(Client client, string vehicleCode1C)
    {
        return $"{client.Url1C}/hs/vehicles/{vehicleCode1C}";
    }
    
    public static string GetTelematicsDataUrl(Client client, string vehicleCode1C)
    {
        return $"{client.Url1C}/hs/telematics/{vehicleCode1C}";
    }

    public static void GetHeaders(Client client, HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
        var credentials = Base64Converter.ConvertToBase64(client.Login, client.Password);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
    }
}