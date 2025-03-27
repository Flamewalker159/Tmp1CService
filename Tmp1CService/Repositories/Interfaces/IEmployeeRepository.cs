using Microsoft.EntityFrameworkCore.Storage;
using Tmp1CService.DTOs.EmployeesDTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<HttpResponseMessage> GetEmployeesFrom1C(Client client);
    Task<List<Employee>> GetEmployeesFromDb();
    Task<Position?> GetPositionFromDbByCode(string code);
    Task AddPosition(Position position);
    Task<Employee?> GetEmployeeByCode(string code);
    Task AddEmployee(Employee employee);
    Task<List<Employee>> GetEmployeesNotInCodes(List<string> allCodesFrom1C);
    Task SaveChangesAsync();
    Task<IDbContextTransaction> CreateTransaction();
    Task<HttpResponseMessage> AddEmployeeTo1C(Client client, EmployeeAddDto employeeDto);
    Task<int> UpdateEmployeeToDb(string empCode, EmployeeAddDto employeeAddDto, Position position, DateTime birthdate);
    Task<HttpResponseMessage> UpdateEmployeeTo1C(Client client, string empCode, EmployeeAddDto employeeIn1C);
    Task DeleteEmployeeFromDb(string code);
    Task<HttpResponseMessage> DeleteEmployeeFrom1C(Client client, string code);
}