using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tmp1CService.Models;

public class Position
{
    [Key] public Guid Id { get; set; }

    [MaxLength(9)] public string Code { get; set; } = string.Empty;

    [MaxLength(50)] public string Name { get; set; } = string.Empty;

    [JsonIgnore] public List<Employee> Employees { get; set; } = [];
}