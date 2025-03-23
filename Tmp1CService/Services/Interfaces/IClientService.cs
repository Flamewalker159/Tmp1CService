using Tmp1CService.DTOs;

namespace Tmp1CService.Services.Interfaces;

public interface IClientService
{
    Task<Guid> RegisterClient(ClientDto clientDto);
    Task<bool> TestConnection(ClientDto clientDto);
    Task<HttpResponseMessage> UpdateClient(ClientDto clientDto, Guid clientId);
    Task<HttpResponseMessage> DeleteClient(Guid clientId);
}