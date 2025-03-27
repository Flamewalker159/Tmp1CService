using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Tmp1CService.DTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Utils;

namespace Tmp1CService.Repositories;

public class ClientRepository(AppDbContext db, HttpClient httpClient) : IClientRepository
{
    public async Task<Guid> RegisterClient(ClientDto clientDto)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Login = clientDto.Login,
            Password = clientDto.Password,
            Url1C = clientDto.Url1C
        };
        await db.AddAsync(client);
        await db.SaveChangesAsync();

        return client.Id;
    }

    public async Task<Client?> CheckClientInDb(Guid clientId)
    {
        return await db.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
    }

    public async Task<bool> TestConnection(ClientDto clientDto)
    {
        var credentials = Base64Converter.ConvertToBase64(clientDto.Login, clientDto.Password);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var response = await httpClient.PostAsync(clientDto.Url1C, null);

        return response.IsSuccessStatusCode;
    }

    public async Task<int> UpdateClient(ClientDto clientDto, Guid clientId)
    {
        return await db.Clients
            .Where(u => u.Id == clientId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Login, clientDto.Login)
                .SetProperty(u => u.Password, clientDto.Password)
                .SetProperty(u => u.Url1C, clientDto.Url1C));
    }

    public async Task<int> DeleteClient(Guid clientId)
    {
        return await db.Clients.Where(u => u.Id == clientId).ExecuteDeleteAsync();
    }
}