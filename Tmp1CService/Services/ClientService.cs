using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Tmp1CService.DTOs;
using Tmp1CService.Models;
using Tmp1CService.Repositories.Interfaces;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    public async Task<Guid> RegisterClient(ClientDto clientDto)
    {
        try
        {
            return await clientRepository.RegisterClient(clientDto);
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
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не удалось обновить данные клиента")
                };
            }

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
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Не удалось удалить данные клиента")
                };
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось удалить клиента: {e.Message}");
        }
    }
}