using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Tmp1CService.Controllers;
using Tmp1CService.DTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;

namespace Tmp1CService.Tests;

public class TestClientsController : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly AppDbContext _db;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TestClientsController(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        _db = new AppDbContext(options);
    }

    [Fact]
    public async Task RegisterClient_ReturnsCreatedAndAddsUserToDb()
    {
        // Arrange
        var client = new ClientDto { Login = "Web", Password = "", Url1C = "http://localhost/InfoBase" };
        using StringContent clientJsonContent = new(
            JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
        
        _client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var response = await _client.PostAsync("api/clients", clientJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedValue =
            JsonConvert.DeserializeObject<Dictionary<string, Guid>>(responseContent);
        
        Assert.NotNull(returnedValue);
        Assert.True(returnedValue.ContainsKey("id1c"));
        var createdClient = returnedValue["id1c"];

        var checkClient = await _db.Clients.FirstOrDefaultAsync(u => u.Id == createdClient);
        
        Assert.NotNull(checkClient);
        Assert.Equal(client.Login, checkClient.Login);
        Assert.Equal(client.Password, checkClient.Password);
        Assert.Equal(client.Url1C, checkClient.Url1C);
    }
    
    [Fact]
    public async Task RegisterClient_ReturnsBadRequest_WhenClientDtoIsInvalid()
    {
        // Arrange
        var invalidClient = new ClientDto();
        using StringContent clientJsonContent = new(
            JsonConvert.SerializeObject(invalidClient), Encoding.UTF8, "application/json");
        
        _client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await _client.PostAsync("api/clients", clientJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task TestConnections_ReturnsOk_WhenConnectionIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IClientRepository>();
        mockRepo.Setup(repo => repo.TestConnection(It.IsAny<ClientDto>())).ReturnsAsync(true);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IClientRepository>();
                services.AddScoped(_ => mockRepo.Object);
            });
        }).CreateClient();

        var client1C = new ClientDto { Login = "Web", Password = "pass123", Url1C = "http://localhost/InfoBase" };
        using StringContent clientJsonContent = new(
            JsonConvert.SerializeObject(client1C), Encoding.UTF8, "application/json");
        
        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.PostAsync("api/clients/test", clientJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task TestConnection_ReturnsBadRequest_WhenUrlIsInvalid()
    {
        // Arrange
        var client = new ClientDto { Login = "Web", Password = "123", Url1C = "invalid-url" };
        using StringContent userJsonContent = new(
            JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");

        _client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await _client.PostAsync("api/clients/test", userJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task TestConnections_ReturnsBadRequest_WhenConnectionFails()
    {
        // Arrange
        var mockRepo = new Mock<IClientRepository>();
        mockRepo.Setup(repo => repo.TestConnection(It.IsAny<ClientDto>())).ReturnsAsync(false);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IClientRepository>();
                services.AddScoped(_ => mockRepo.Object);
            });
        }).CreateClient();

        var client1C = new ClientDto { Login = "Web", Password = "", Url1C = "http://localhost/InfoBase" };
        using StringContent userJsonContent = new(
            JsonConvert.SerializeObject(client1C), Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("X-API-KEY", "qweasd");

        // Act
        var result = await client.PostAsync("api/clients/test", userJsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}