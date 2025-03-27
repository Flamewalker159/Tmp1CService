using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Tmp1CService.DTOs.EmployeesDTOs;
using Tmp1CService.Models;
using Tmp1CService.Services.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Tests;

public class TestEmployeeController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly AppDbContext _db;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TestEmployeeController(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _db = new AppDbContext(options);
    }

    [Fact]
    public async Task Get_AllEmployeesFrom1C_ReturnsSuccess()
    {
        // Arrange
        var client1C = new Client
            { Id = Guid.NewGuid(), Login = "Web", Password = "", Url1C = "http://localhost/InfoBase" };
        await _db.Clients.AddAsync(client1C);
        await _db.SaveChangesAsync();

        var mockRepo = new Mock<IEmployeeService>();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "EmployeesFrom1C.json");
        var employeesJson = await File.ReadAllTextAsync(path);

        var settings = JsonSettings.GetSettingsWithDayFirstFormat();
        var employees = JsonConvert.DeserializeObject<Dictionary<string, Employee1CDto>>(employeesJson, settings);

        mockRepo.Setup(repo => repo.GetEmployeesFrom1C(client1C.Id)).ReturnsAsync(employees);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IEmployeeService>();
                services.AddScoped(_ => mockRepo.Object);
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.GetAsync($"api/Employee/EmployeesFrom1C?id={client1C.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var responseContent = await result.Content.ReadAsStringAsync();
        var returnedEmployees = JsonConvert.DeserializeObject<Dictionary<string, Employee1CDto>>(responseContent);
        Assert.NotNull(returnedEmployees);
        Assert.NotEmpty(returnedEmployees);
        mockRepo.Verify(repo => repo.GetEmployeesFrom1C(client1C.Id), Times.Once());
    }

    [Fact]
    public async Task Get_AllEmployeesFromDb_ReturnsSuccess()
    {
        // Arrange
        var employeesFromDb = new List<Employee>
        {
            new() { Id = Guid.NewGuid(), Name = "Иванов Иван", Code = "123" }
        };

        var mockRepo = new Mock<IEmployeeService>();
        mockRepo.Setup(repo => repo.GetEmployeesFromDb()).ReturnsAsync(employeesFromDb);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IEmployeeService>();
                services.AddScoped(_ => mockRepo.Object);
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.GetAsync("api/Employee/EmployeesFromDb");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var responseContent = await result.Content.ReadAsStringAsync();
        var returnedEmployees = JsonConvert.DeserializeObject<List<Employee>>(responseContent);
        Assert.NotNull(returnedEmployees);
        Assert.NotEmpty(returnedEmployees);
    }
}