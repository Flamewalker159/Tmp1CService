using System.Net;
using Tmp1CService.DTOs.ClientDTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    public async Task<List<Client>> GetClients()
    {
        try
        {
            return await clientRepository.GetClients();
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось получить список клиентов: {e.Message}");
        }
    }
    
    public async Task<Client?> GetClient(Guid clientId)
    {
        var client = await clientRepository.CheckClientInDb(clientId);
        if (client == null)
            throw new KeyNotFoundException("Клиент не найден");
        
        try
        {
            return await clientRepository.GetClient(clientId);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось получить информацию об клиенте: {e.Message}");
        }
    }
    public async Task<Guid> RegisterClient(ClientDto clientDto)
    {
        try
        {
            return await clientRepository.RegisterClient(clientDto);
        }
        catch(BadHttpRequestException bhre)
        {
            throw new BadHttpRequestException(bhre.Message);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось создать профиль: {e.Message}");
        }
    }

    public async Task<bool> TestConnection(ClientDto clientDto)
    {
        try
        {
            return await clientRepository.TestConnection(clientDto);
        }
        catch (Exception e)
        {
            throw new Exception($"Не проверить соединение: {e.Message}");
        }
    }

    public async Task<HttpResponseMessage> UpdateClient(ClientDto clientDto, Guid clientId)
    {
        try
        {
            var updateUser = await clientRepository.UpdateClient(clientDto, clientId);
            if (updateUser == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не удалось обновить данные клиента")
                };

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось обновить данные клиента: {e.Message}");
        }
    }

    public async Task<HttpResponseMessage> DeleteClient(Guid clientId)
    {
        try
        {
            var deleteUser = await clientRepository.DeleteClient(clientId);
            if (deleteUser == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не удалось удалить данные клиента")
                };

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось удалить клиента: {e.Message}");
        }
    }
}