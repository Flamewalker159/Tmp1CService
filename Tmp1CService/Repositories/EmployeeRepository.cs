using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Tmp1CService.DTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Repositories;

public class EmployeeRepository(AppDbContext db, IHttpClientFactory httpClient) : IEmployeeRepository
{
    private readonly HttpClient _httpClient = httpClient.CreateClient();

    public async Task<HttpResponseMessage> GetEmployeesFrom1C(Client client)
    {
        var url = UrlConverter.GetEmployeesUrl(client);
        UrlConverter.GetHeaders(client, _httpClient);
        var response = await _httpClient.GetAsync(url);

        return response;
    }

    public async Task<List<Employee>> GetEmployeesFromDb()
    {
        return await db.Employees
            .Include(p => p.Position)
            .OrderBy(e => e.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Position?> GetPositionFromDbByCode(string code)
    {
        return await db.Positions.FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task AddPosition(Position position)
    {
        await db.Positions.AddAsync(position);
    }

    public async Task<Employee?> GetEmployeeByCode(string code)
    {
        return await db.Employees.FirstOrDefaultAsync(e => e.Code == code);
    }

    public async Task AddEmployee(Employee employee)
    {
        await db.Employees.AddAsync(employee);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> CreateTransaction()
    {
        return await db.Database.BeginTransactionAsync();
    }

    public async Task<List<Employee>> GetEmployeesNotInCodes(List<string> allCodesFrom1C)
    {
        return await db.Employees
            .Where(e => !allCodesFrom1C.Contains(e.Code))
            .ToListAsync();
    }

    public async Task<HttpResponseMessage> AddEmployeeTo1C(Client client, EmployeeAddDto employeeDto)
    {
        var settings = JsonSettings.GetSettingsWithDayFirstFormat();
        var url = UrlConverter.GetEmployeesUrl(client);
        UrlConverter.GetHeaders(client, _httpClient);
        var response = await _httpClient.PostAsync(
            url,
            new StringContent(JsonConvert.SerializeObject(employeeDto, settings),
                Encoding.UTF8, "application/json"));
        return response;
    }

    public async Task<int> UpdateEmployeeToDb(string empCode, EmployeeAddDto employeeAddDto, Position position,
        DateTime birthdate)
    {
        return await db.Employees
            .Where(e => e.Code == empCode)
            .ExecuteUpdateAsync(s => s
                .SetProperty(e => e.Code, employeeAddDto.Code)
                .SetProperty(e => e.Name, employeeAddDto.Name)
                .SetProperty(e => e.Birthdate, birthdate)
                .SetProperty(e => e.NumberOfChildren, employeeAddDto.NumberOfChildren)
                .SetProperty(e => e.Works, employeeAddDto.Works)
                .SetProperty(e => e.WorkExperience, employeeAddDto.WorkExperience)
                .SetProperty(e => e.PositionId, position.Id));
    }

    public async Task<HttpResponseMessage> UpdateEmployeeTo1C(Client client, string empCode, EmployeeAddDto employeeIn1C)
    {
        var settings = JsonSettings.GetSettingsWithMonthFirstFormat();
        var url = UrlConverter.GetEmployeeUrl(client, empCode);
        UrlConverter.GetHeaders(client, _httpClient);
        return await _httpClient.PutAsync(
            url,
            new StringContent(JsonConvert.SerializeObject(employeeIn1C, settings),
                Encoding.UTF8, "application/json"));
    }

    public async Task DeleteEmployeeFromDb(string code)
    {
        await db.Employees
            .Where(e => e.Code == code)
            .ExecuteDeleteAsync();
    }

    public async Task<HttpResponseMessage> DeleteEmployeeFrom1C(Client client, string code)
    {
        var url = UrlConverter.GetEmployeeUrl(client, code);
        UrlConverter.GetHeaders(client, _httpClient);
        return await _httpClient.DeleteAsync(url);
    }
}