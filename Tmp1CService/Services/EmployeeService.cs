using Tmp1CService.DTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, IClientRepository clientRepository) : IEmployeeService
{
    public async Task<Dictionary<string, Employee1CDto>?> GetEmployeesFrom1C(Guid clientId)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var employeesFrom1C = await employeeRepository.GetEmployeesFrom1C(client);

        if (!employeesFrom1C.IsSuccessStatusCode)
        {
            var errorDetails = await employeesFrom1C.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Ошибка при получении данных из 1С. Код ответа: {employeesFrom1C.StatusCode}, детали: {errorDetails}");
        }

        var responseData = await employeesFrom1C.Content.ReadAsStringAsync();
        var employees = JsonResponseConvert.JsonDeserializeResponseToDictionary<Employee1CDto>(responseData);
        if (employees == null)
            throw new Exception("Данные из 1С не получены");

        return employees;
    }

    public async Task<List<Employee>> GetEmployeesFromDb()
    {
        return await employeeRepository.GetEmployeesFromDb();
    }

    public async Task SyncEmployeesFrom1C(Guid clientId)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var employeesFrom1C = await GetEmployeesFrom1C(clientId);

        if (employeesFrom1C == null)
            throw new Exception("Не удалось получить данные из 1С.");

        var transaction = await employeeRepository.CreateTransaction();

        try
        {
            foreach (var employeeFrom1C in employeesFrom1C)
            {
                employeeFrom1C.Value.Birthdate = ConverterDate.ConvertToUtc(employeeFrom1C.Value.Birthdate);

                // Обработка должности
                var positionId = Guid.Empty;
                if (employeeFrom1C.Value.Position != null)
                {
                    var positionFromDb =
                        await employeeRepository.GetPositionFromDbByCode(employeeFrom1C.Value.Position.Code!);
                    if (positionFromDb == null)
                    {
                        var newPosition = new Position
                        {
                            Code = employeeFrom1C.Value.Position.Code!,
                            Name = employeeFrom1C.Value.Position.Description!
                        };
                        await employeeRepository.AddPosition(newPosition);
                        positionId = newPosition.Id;
                    }
                    else
                    {
                        positionId = positionFromDb.Id;
                    }
                }

                var employeeFromDb = await employeeRepository.GetEmployeeByCode(employeeFrom1C.Value.Code);

                // если сотрудник не найден в БД, то добавляем его иначе обновляем
                if (employeeFromDb == null)
                {
                    var employee = new Employee
                    {
                        Code = employeeFrom1C.Value.Code,
                        Name = employeeFrom1C.Key,
                        Birthdate = employeeFrom1C.Value.Birthdate,
                        NumberOfChildren = employeeFrom1C.Value.NumberOfChildren,
                        Works = employeeFrom1C.Value.Works,
                        WorkExperience = employeeFrom1C.Value.WorkExperience,
                        PositionId = positionId
                    };

                    await employeeRepository.AddEmployee(employee);
                }
                else
                {
                    employeeFromDb.Name = employeeFrom1C.Key;
                    employeeFromDb.Birthdate = employeeFrom1C.Value.Birthdate;
                    employeeFromDb.NumberOfChildren = employeeFrom1C.Value.NumberOfChildren;
                    employeeFromDb.Works = employeeFrom1C.Value.Works;
                    employeeFromDb.WorkExperience = employeeFrom1C.Value.WorkExperience;
                    employeeFromDb.PositionId = positionId;
                }
            }

            // Помечаем отсутствующих в 1С сотрудников как неработающих
            var allCodesFrom1C = employeesFrom1C.Select(e => e.Value.Code).ToList();
            var employeesToDeactivate = await employeeRepository.GetEmployeesNotInCodes(allCodesFrom1C);

            foreach (var employee in employeesToDeactivate) employee.Works = false;

            await employeeRepository.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Ошибка при синхронизации данных: {e.Message}");
        }
    }

    public async Task<HttpResponseMessage> AddEmployee(Guid clientId, EmployeeAddDto employeeAddDto)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var transaction = await employeeRepository.CreateTransaction();

        try
        {
            var birthdate = ConverterDate.ConvertToUtc(employeeAddDto.Birthdate);

            var position = await employeeRepository.GetPositionFromDbByCode(employeeAddDto.Position!);

            if (position == null)
                throw new KeyNotFoundException("Должность не найдена.");

            var employee = new Employee
            {
                Code = employeeAddDto.Code,
                Name = employeeAddDto.Name,
                Birthdate = birthdate,
                NumberOfChildren = employeeAddDto.NumberOfChildren,
                Works = employeeAddDto.Works,
                WorkExperience = employeeAddDto.WorkExperience,
                PositionId = position.Id
            };

            await employeeRepository.AddEmployee(employee);
            await employeeRepository.SaveChangesAsync();

            // добавление данных в 1С
            var employeeDto = new EmployeeAddDto
            {
                Code = employeeAddDto.Code,
                Name = employeeAddDto.Name,
                Birthdate = birthdate,
                NumberOfChildren = employeeAddDto.NumberOfChildren,
                Works = employeeAddDto.Works,
                WorkExperience = employeeAddDto.WorkExperience,
                Position = employeeAddDto.Position
            };

            var response = await employeeRepository.AddEmployeeTo1C(client, employeeDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Ошибка при получении данных из 1С. Код ответа: {response.StatusCode}, детали: {errorDetails}");
            }

            await transaction.CommitAsync();
            return response;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Ошибка при добавлении данных: {e.Message}");
        }
    }

    public async Task<HttpResponseMessage> UpdateEmployee(Guid clientId, string empCode, EmployeeAddDto employeeAddDto)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var transaction = await employeeRepository.CreateTransaction();

        try
        {
            var position = await employeeRepository.GetPositionFromDbByCode(employeeAddDto.Position!);
            if (position == null)
                throw new KeyNotFoundException("Должность не найдена.");
            var birthdate = ConverterDate.ConvertToUtc(employeeAddDto.Birthdate);

            var updateEmployee =
                await employeeRepository.UpdateEmployeeToDb(empCode, employeeAddDto, position, birthdate);

            if (updateEmployee == 0)
                throw new KeyNotFoundException("Сотрудник с указанным кодом не найден.");

            // добавление данных в 1С
            var employeeIn1C = new EmployeeAddDto
            {
                Code = employeeAddDto.Code,
                Name = employeeAddDto.Name,
                Birthdate = birthdate,
                NumberOfChildren = employeeAddDto.NumberOfChildren,
                Works = employeeAddDto.Works,
                WorkExperience = employeeAddDto.WorkExperience,
                Position = employeeAddDto.Position
            };

            var response = await employeeRepository.UpdateEmployeeTo1C(client, empCode, employeeIn1C);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Ошибка при получении данных из 1С. Код ответа: {response.StatusCode}, детали: {errorDetails}");
            }

            await transaction.CommitAsync();
            return response;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Ошибка при изменении данных: {e.Message}");
        }
    }

    public async Task<bool> DeleteEmployee(Guid clientId, string code)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");

        var transaction = await employeeRepository.CreateTransaction();
        try
        {
            // удаление данных из БД
            await employeeRepository.DeleteEmployeeFromDb(code);

            // удаление данных из 1С
            var response = await employeeRepository.DeleteEmployeeFrom1C(client, code);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Ошибка при получении данных из 1С. Код ответа: {response.StatusCode}, детали: {errorDetails}");
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Ошибка при удалении данных: {e.Message}");
        }
    }
}