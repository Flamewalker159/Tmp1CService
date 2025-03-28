﻿using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tmp1CService.DTOs.ClientDTOs;
using Tmp1CService.Services.Interfaces;

namespace Tmp1CService.Controllers;

[Route("api/clients")]
[ApiController]
[Authorize(AuthenticationSchemes = "ApiKeyScheme")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        try
        {
            var response = await clientService.GetClients();
            return Ok(response);
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Произошла ошибка на сервере: " + e.Message });
        }
    }
    
    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetClient(Guid clientId)
    {
        if (clientId == Guid.Empty)
            return Unauthorized(new { message = "Некорректный идентификатор клиента" });
        
        try
        {
            var response = await clientService.GetClient(clientId);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Произошла ошибка на сервере: " + e.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterClients(ClientDto client)
    {
        try
        {
            var clientsToDb = await clientService.RegisterClient(client);
            return StatusCode(201, new { Id1c = clientsToDb });
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("test")]
    public async Task<IActionResult> TestConnection(ClientDto clientDto)
    {
        if (!Uri.TryCreate(clientDto.Url1C, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            return BadRequest(new { message = "Неверный URL-адрес" });

        var response = await clientService.TestConnection(clientDto);

        return response ? Ok(new { message = "Соединение установлено." }) : BadRequest(new { message = "Bad Request" });
    }

    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> UpdateClient(ClientDto? clientDto, Guid clientId)
    {
        if (clientDto == null) return BadRequest("Данные клиента не предоставлены");

        try
        {
            var response = await clientService.UpdateClient(clientDto, clientId);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(new { message = "Данные клиента успешно обновлены" });
                case HttpStatusCode.BadRequest:
                    return BadRequest(await response.Content.ReadAsStringAsync());
            }

            return StatusCode((int)response.StatusCode, "Неизвестная ошибка при обновлении клиента");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, details = ex.Message });
        }
    }

    [HttpDelete("{clientId:guid}")]
    public async Task<IActionResult> DeleteClient(Guid clientId)
    {
        try
        {
            var response = await clientService.DeleteClient(clientId);
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return NoContent();
                case HttpStatusCode.BadRequest:
                    return BadRequest(await response.Content.ReadAsStringAsync());
            }

            return StatusCode((int)response.StatusCode, "Неизвестная ошибка при удалении клиента");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}