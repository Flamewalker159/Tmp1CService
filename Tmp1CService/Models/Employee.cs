using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tmp1CService.Models;

public class Employee
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] [MaxLength(9)] public string Code { get; set; } = string.Empty;

    [Required] [MaxLength(30)] public string? Name { get; set; } = string.Empty;

    public DateTime Birthdate { get; set; }

    public int NumberOfChildren { get; set; }

    public bool Works { get; set; } = true;

    [MaxLength(30)] public string WorkExperience { get; set; } = string.Empty;

    [ForeignKey("Position")] public Guid PositionId { get; set; }

    public Position? Position { get; set; }
}