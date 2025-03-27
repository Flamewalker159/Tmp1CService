using System.ComponentModel.DataAnnotations;

namespace Tmp1CService.DTOs.ClientDTOs;

public class ClientDto
{
    [Required] public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    [Required] public string Url1C { get; set; } = string.Empty;
}