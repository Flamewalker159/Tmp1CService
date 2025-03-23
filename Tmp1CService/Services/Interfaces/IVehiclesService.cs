using Tmp1CService.DTOs;

namespace Tmp1CService.Services.Interfaces;

public interface IVehiclesService
{
    Task<List<VehicleDto1C>> GetVehiclesFrom1C(Guid clientId);
    Task<HttpResponseMessage> UpdateVehicle1C(Guid clientId, string vehicleCode1C, VehicleUpdateDto updateDto);
}