using Newtonsoft.Json;
using Tmp1CService.DTOs.TelematicsDTOs;
using Tmp1CService.DTOs.VehicleDTOs;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Services;

public class TelematicsService(ITelematicsRepository telematicsRepository, IClientRepository clientRepository)
    : ITelematicsService
{
    public async Task<HttpResponseMessage> SendingTelematicsData(Guid clientId, string vehicleCode1C,
        TelematicsDataDto telematicsDataDto)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var response = await telematicsRepository.SendingTelematicsData(client, vehicleCode1C, telematicsDataDto);

        return response;
    }
}