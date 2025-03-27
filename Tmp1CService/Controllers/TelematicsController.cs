using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tmp1CService.DTOs.TelematicsDTOs;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Controllers;

[Route("api/clients/{clientId:guid}/telematicsData")]
[ApiController]
[Authorize(AuthenticationSchemes = "ApiKeyScheme")]
public class TelematicsController(ITelematicsService telematicsService) : ControllerBase
{
    [HttpPost("{vehicleCode1C}")]
    public async Task<IActionResult> SendingTelematicsData(Guid clientId, string vehicleCode1C,
        TelematicsDataDto telematicsDataDto)
    {
        if (clientId == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var response = await telematicsService.SendingTelematicsData(clientId, vehicleCode1C, telematicsDataDto);

            var detailsResponse = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode,
                    new { message = "Ошибка при добавлении телематических данных", details = detailsResponse });
            }

            return Ok(new { message = "Данные успешно сохранены" });
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(new { message = knfEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при обновлении данных ТС", details = ex.Message });
        }
    }
}