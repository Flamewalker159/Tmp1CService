using Tmp1CService.DTOs.TelematicsDTOs;

namespace Tmp1CService.Services.Interfaces;

public interface ITelematicsService
{
    Task<HttpResponseMessage> SendingTelematicsData(Guid clientId, string vehicleCode1C,
        TelematicsDataDto telematicsDataDto);
}