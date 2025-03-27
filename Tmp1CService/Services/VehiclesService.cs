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
    
    public async Task<VehicleDto1C> GetVehicleFrom1C(Guid clientId, string vehicleCode1C)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var vehicleFrom1C = await vehicleRepository.GetVehicleFrom1C(client, vehicleCode1C);

        if (!vehicleFrom1C.IsSuccessStatusCode)
        {
            var errorDetails = await vehicleFrom1C.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Ошибка при получении данных из 1С. Код ответа: {vehicleFrom1C.StatusCode}, детали: {errorDetails}");
        }

        var responseData = await vehicleFrom1C.Content.ReadAsStringAsync();
        var vehicles = JsonConvert.DeserializeObject<VehicleDto1C>(responseData);
        if (vehicles == null)
            throw new Exception("Данные из 1С не получены");

        return vehicles;
    }
    
    public async Task<VehicleDto1C> CreateVehicleIn1C(Guid clientId, VehicleDto1C vehicleDto1C)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var createdVehicle = await vehicleRepository.CreateVehicleIn1C(client, vehicleDto1C);

        if (!createdVehicle.IsSuccessStatusCode)
        {
            var errorDetails = await createdVehicle.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Ошибка при добавлении данных в 1С. Код ответа: {createdVehicle.StatusCode}, детали: {errorDetails}");
        }

        var responseData = await createdVehicle.Content.ReadAsStringAsync();
        var vehicles = JsonConvert.DeserializeObject<VehicleDto1C>(responseData);
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