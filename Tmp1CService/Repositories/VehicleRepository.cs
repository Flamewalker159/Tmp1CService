using System.Text;
using Newtonsoft.Json;
using Tmp1CService.DTOs.VehicleDTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Repositories;

public class VehicleRepository(HttpClient httpClient) : IVehicleRepository
{
    public async Task<HttpResponseMessage> GetVehiclesFrom1C(Client client)
    {
        var url = UrlConverter.GetVehiclesUrl(client);
        UrlConverter.GetHeaders(client, httpClient);
        var response = await httpClient.GetAsync(url);

        return response;
    }
    
    public async Task<HttpResponseMessage> GetVehicleFrom1C(Client client, string vehicleCode1C)
    {
        var url = UrlConverter.GetVehicleUrl(client, vehicleCode1C);
        UrlConverter.GetHeaders(client, httpClient);
        var response = await httpClient.GetAsync(url);

        return response;
    }
    
    public async Task<HttpResponseMessage> CreateVehicleIn1C(Client client, VehicleDto1C vehicleDto1C)
    {
        var url = UrlConverter.GetVehiclesUrl(client);
        UrlConverter.GetHeaders(client, httpClient);

        var jsonSettings = JsonSettings.GetSettingsWithDayFirstFormat();
        var content = new StringContent(JsonConvert.SerializeObject(vehicleDto1C, jsonSettings), Encoding.UTF8,
            "application/json");
        
        var response = await httpClient.PostAsync(url, content);

        return response;
    }
    
    public async Task<HttpResponseMessage> UpdateVehicleIn1C(Client client, string vehicleCode1C,
        VehicleUpdateDto updateDto)
    {
        var url = UrlConverter.GetVehicleUrl(client, vehicleCode1C);
        UrlConverter.GetHeaders(client, httpClient);

        var jsonSettings = JsonSettings.GetSettingsWithMonthFirstFormat();
        var content = new StringContent(JsonConvert.SerializeObject(updateDto, jsonSettings), Encoding.UTF8,
            "application/json");

        var response = await httpClient.PutAsync(url, content);

        return response;
    }
}