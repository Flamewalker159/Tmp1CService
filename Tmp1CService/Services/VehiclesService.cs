using System.Text;
using Newtonsoft.Json;
using Tmp1CService.DTOs.VehicleDTOs;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Services;

public class VehiclesService(IVehicleRepository vehicleRepository, IClientRepository clientRepository)
    : IVehiclesService
{
    public async Task<List<VehicleDto1C>> GetVehiclesFrom1C(Guid clientId)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var vehiclesFrom1C = await vehicleRepository.GetVehiclesFrom1C(client);

        if (!vehiclesFrom1C.IsSuccessStatusCode)
        {
            var errorDetails = await vehiclesFrom1C.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Ошибка при получении данных из 1С. Код ответа: {vehiclesFrom1C.StatusCode}, детали: {errorDetails}");
        }

        var responseData = await vehiclesFrom1C.Content.ReadAsStringAsync();
        var vehicles = JsonResponseConvert.JsonDeserializeResponse<VehicleDto1C>(responseData);
        if (vehicles == null)
            throw new Exception("Данные из 1С не получены");

        return vehicles;
    }

    public async Task<HttpResponseMessage> UpdateVehicle1C(Guid clientId, string vehicleCode1C,
        VehicleUpdateDto updateDto)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var response = await vehicleRepository.UpdateVehicleIn1C(client, vehicleCode1C, updateDto);
        
        return response;
    }
}