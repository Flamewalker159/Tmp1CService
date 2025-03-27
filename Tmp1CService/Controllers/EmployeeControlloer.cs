using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tmp1CService.DTOs.EmployeesDTOs;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "ApiKeyScheme")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet("EmployeesFrom1C")]
    public async Task<IActionResult> GetEmployeesFrom1C(Guid id)
    {
        if (id == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var employees = await employeeService.GetEmployeesFrom1C(id);
            return Ok(employees);
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(new { message = knfEx.Message });
        }
        catch (HttpRequestException httpEx)
        {
            return StatusCode((int)HttpStatusCode.BadGateway, new { message = httpEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Внутренняя ошибка сервера", details = ex.Message });
        }
    }

    [HttpGet("EmployeesFromDb")]
    public async Task<IActionResult> GetEmployeesFromDb()
    {
        try
        {
            var employees = await employeeService.GetEmployeesFromDb();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка получения данных из БД", details = ex.Message });
        }
    }

    [HttpPost("SyncEmployeesFrom1C")]
    public async Task<IActionResult> SyncEmployeesFrom1C(Guid id)
    {
        if (id == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });
        try
        {
            await employeeService.SyncEmployeesFrom1C(id);
            return Ok(new { message = "Синхронизация прошла успешно" });
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(new { message = knfEx.Message });
        }
        catch (HttpRequestException httpEx)
        {
            return StatusCode((int)HttpStatusCode.BadGateway, new { message = httpEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка синхронизации", details = ex.Message });
        }
    }

    [HttpPost("AddEmployee")]
    public async Task<IActionResult> AddEmployee(Guid id, [FromBody] EmployeeAddDto employeeAddDto)
    {
        if (id == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var response = await employeeService.AddEmployee(id, employeeAddDto);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { message = "Должность не найдена." });

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode,
                    new { message = "Ошибка при добавлении сотрудника", details = errorDetails });
            }

            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, JsonConvert.DeserializeObject(content));
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(new { message = knfEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при добавлении сотрудника", details = ex.Message });
        }
    }

    [HttpPut("UpdateEmployee/{empCode}/")]
    public async Task<IActionResult> UpdateEmployee(Guid id, string empCode, [FromBody] EmployeeAddDto employeeAddDto)
    {
        if (id == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });

        try
        {
            var response = await employeeService.UpdateEmployee(id, empCode, employeeAddDto);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { message = "Сотрудник или должность не найдены." });

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode,
                    new { message = "Ошибка при обновлении сотрудника", details = errorDetails });
            }

            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, JsonConvert.DeserializeObject(content));
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(new { message = knfEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при обновлении сотрудника", details = ex.Message });
        }
    }

    [HttpDelete("DeleteEmployee/{empCode}")]
    public async Task<IActionResult> DeleteEmployee(Guid id, string empCode)
    {
        if (id == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });
        try
        {
            var result = await employeeService.DeleteEmployee(id, empCode);
            if (!result)
                return NotFound(new { message = "Сотрудник не найден" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при удалении сотрудника", details = ex.Message });
        }
    }
}