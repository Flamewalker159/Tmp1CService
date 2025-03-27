using Tmp1CService.DTOs.VehicleDTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<HttpResponseMessage> GetVehiclesFrom1C(Client client);
    Task<HttpResponseMessage> GetVehicleFrom1C(Client client, string vehicleCode1C);

    Task<HttpResponseMessage> CreateVehicleIn1C(Client client, VehicleDto1C vehicleDto1C);
    Task<HttpResponseMessage> UpdateVehicleIn1C(Client client, string vehicleCode1C, VehicleUpdateDto updateDto);
}