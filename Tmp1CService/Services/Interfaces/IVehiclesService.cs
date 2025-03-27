using Tmp1CService.DTOs.VehicleDTOs;

namespace Tmp1CService.Services.Interfaces;

public interface IVehiclesService
{
    Task<List<VehicleDto1C>> GetVehiclesFrom1C(Guid clientId);
    Task<VehicleDto1C> GetVehicleFrom1C(Guid clientId, string vehicleCode1C);
    Task<VehicleDto1C> CreateVehicleIn1C(Guid clientId, VehicleDto1C vehicleDto1C);
    Task<HttpResponseMessage> UpdateVehicle1C(Guid clientId, string vehicleCode1C, VehicleUpdateDto updateDto);
}