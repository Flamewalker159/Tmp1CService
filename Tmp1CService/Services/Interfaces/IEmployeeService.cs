using Tmp1CService.DTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Services.Interfaces;

public interface IEmployeeService
{
    Task<Dictionary<string, Employee1CDto>?> GetEmployeesFrom1C(Guid clientId);
    Task<List<Employee>> GetEmployeesFromDb();
    Task SyncEmployeesFrom1C(Guid clientId);
    Task<HttpResponseMessage> AddEmployee(Guid clientId, EmployeeAddDto employeeAddDto);
    Task<HttpResponseMessage> UpdateEmployee(Guid clientId, string empCode, EmployeeAddDto employeeAddDto);
    Task<bool> DeleteEmployee(Guid clientId, string code);
}