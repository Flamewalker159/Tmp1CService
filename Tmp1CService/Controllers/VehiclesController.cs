using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tmp1CService.DTOs;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Controllers;

[Route("api/clients/{clientId:guid}/vehicles")]
[ApiController]
[Authorize(AuthenticationSchemes = "ApiKeyScheme")]
public class VehiclesController(IVehiclesService vehiclesService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVehiclesFrom1C(Guid clientId)
    {
        if (clientId == Guid.Empty)
            return BadRequest(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var vehicles = await vehiclesService.GetVehiclesFrom1C(clientId);
            return Ok(vehicles);
        }
        catch (UnauthorizedAccessException uaEx)
        {
            return Unauthorized(new { message = uaEx.Message });
        }
        catch (KeyNotFoundException keyEx)
        {
            return NotFound(new { message = keyEx.Message });
        }
        catch (BadHttpRequestException bhrex)
        {
            return BadRequest(new { message = bhrex.Message });
        }
        catch (HttpRequestException httpEx)
        {
            return StatusCode(502, new { message = httpEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Внутренняя ошибка сервера", details = ex.Message });
        }
    }
    
    [HttpPut("{vehicleCode1C}")]
    public async Task<IActionResult> UpdateVehicle1C(Guid clientId, string vehicleCode1C, VehicleUpdateDto vehicleUpdateDto)
    {
        if (clientId == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var response = await vehiclesService.UpdateVehicle1C(clientId, vehicleCode1C, vehicleUpdateDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode,
                    new { message = "Ошибка при обновлении данных ТС", details = errorDetails });
            }

            return Ok(new { message = "Данные ТС успешно обновлены" });
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