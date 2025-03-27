using Tmp1CService.DTOs.ClientDTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Services.Interfaces;

public interface IClientService
{
    Task<List<Client>> GetClients();
    Task<Client?> GetClient(Guid clientId);
    Task<Guid> RegisterClient(ClientDto clientDto);
    Task<bool> TestConnection(ClientDto clientDto);
    Task<HttpResponseMessage> UpdateClient(ClientDto clientDto, Guid clientId);
    Task<HttpResponseMessage> DeleteClient(Guid clientId);
}