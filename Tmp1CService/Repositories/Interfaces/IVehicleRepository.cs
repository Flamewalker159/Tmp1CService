using Tmp1CService.DTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<HttpResponseMessage> GetVehiclesFrom1C(Client client);
    Task<HttpResponseMessage> UpdateVehicleIn1C(Client client, string vehicleCode1C, VehicleUpdateDto updateDto);
}