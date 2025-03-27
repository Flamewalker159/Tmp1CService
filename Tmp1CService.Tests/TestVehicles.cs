using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Tmp1CService.DTOs.VehicleDTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;

namespace Tmp1CService.Tests;

public class TestVehiclesController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly AppDbContext _db;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TestVehiclesController(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _db = new AppDbContext(options);
    }

    [Fact]
    public async Task Get_Vehicles_ReturnSuccess()
    {
        //arrange
        var client1C = new Client
            { Id = Guid.NewGuid(), Login = "Web", Password = "", Url1C = "http://localhost/InfoBase" };
        await _db.Clients.AddAsync(client1C);
        await _db.SaveChangesAsync();

        var mockRepo = new Mock<IVehicleRepository>();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "VehiclesFrom1C.json");
        var carsJson = await File.ReadAllTextAsync(path);
        mockRepo.Setup(repo => repo.GetVehiclesFrom1C(It.Is<Client>(u => u.Id == client1C.Id))).ReturnsAsync(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(carsJson, Encoding.UTF8, "application/json")
            });

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IVehicleRepository>();
                services.AddScoped(_ => mockRepo.Object);
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        //act
        var result = await client.GetAsync($"api/clients/{client1C.Id}/vehicles/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        mockRepo.Verify(repo => repo.GetVehiclesFrom1C(It.IsAny<Client>()), Times.Once());

        var responseContent = await result.Content.ReadAsStringAsync();
        var vehicles = JsonConvert.DeserializeObject<List<VehicleDto>>(responseContent);
        var expectedVehicles = JsonConvert.DeserializeObject<List<VehicleDto1C>>(carsJson)!;

        Assert.NotNull(vehicles);
        Assert.NotEmpty(vehicles);

        Assert.Equal(expectedVehicles.Count, vehicles.Count);

        // Проверка соответствия данных между ожидаемым и фактическим списком
        for (var i = 0; i < expectedVehicles.Count; i++)
        {
            var expected = expectedVehicles[i];
            var actual = vehicles[i];

            Assert.Equal(expected.Brand, actual.Brand);
            Assert.Equal(expected.ChassisNumber, actual.ChassisNumber);
            Assert.Equal(expected.Dimensions, actual.Dimensions);
            Assert.Equal(expected.EngineModel, actual.EngineModel);
            Assert.Equal(expected.EngineNumber, actual.EngineNumber);
        }
    }

    [Fact]
    public async Task Get_Vehicles_ReturnsBadRequest_WhenClientIdIsEmpty()
    {
        // Arrange
        var clientId = Guid.Empty;
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.GetAsync($"api/clients/{clientId}/vehicles/");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Get_Vehicles_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var nonExistentClientId = Guid.NewGuid();
        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.GetAsync($"api/clients/{nonExistentClientId}/vehicles/");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}