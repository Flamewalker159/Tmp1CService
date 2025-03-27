using System.Text;
using Newtonsoft.Json;
using Tmp1CService.DTOs.TelematicsDTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Repositories;

public class TelematicsRepository(HttpClient httpClient) : ITelematicsRepository
{
    public async Task<HttpResponseMessage> SendingTelematicsData(Client client, string vehicleCode1C,
        TelematicsDataDto telematicsDataDto)
    {
        var url = UrlConverter.GetTelematicsDataUrl(client, vehicleCode1C);
        UrlConverter.GetHeaders(client, httpClient);
        
        var jsonSettings = JsonSettings.GetSettingsWithMonthFirstFormat();
        var content = new StringContent(JsonConvert.SerializeObject(telematicsDataDto), Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(url, content);

        return response;
    }
}