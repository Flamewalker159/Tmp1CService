using System.ComponentModel.DataAnnotations;

namespace Tmp1CService.Models;

public class Client
{
    [Key] public Guid Id { get; set; }

    [Required] [MaxLength(50)] public string Login { get; set; } = string.Empty;

    [MaxLength(50)] [Required] public string Password { get; set; } = string.Empty;

    [Required] [MaxLength(50)] [Url] public string Url1C { get; set; } = string.Empty;
}