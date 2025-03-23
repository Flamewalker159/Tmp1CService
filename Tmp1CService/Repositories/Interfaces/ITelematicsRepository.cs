using Tmp1CService.DTOs.TelematicsDTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Repositories.Interfaces;

public interface ITelematicsRepository
{
    Task<HttpResponseMessage> SendingTelematicsData(Client client, string vehicleCode1C,
        TelematicsDataDto telematicsDataDto);
}