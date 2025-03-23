using Tmp1CService.DTOs;
using Tmp1CService.Models;

namespace Tmp1CService.Repositories.Interfaces;

public interface IClientRepository
{
    Task<Guid> RegisterClient(ClientDto clientDto);
    Task<bool> TestConnection(ClientDto clientDto);
    Task<Client?> CheckClientInDb(Guid clientId);
    Task<int> UpdateClient(ClientDto clientDto, Guid clientId);
    Task<int> DeleteClient(Guid clientId);
}