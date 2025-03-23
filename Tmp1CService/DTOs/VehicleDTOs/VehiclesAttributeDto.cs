namespace Tmp1CService.DTOs;

public class VehiclesAttributeDto
{
    public Guid Id { get; set; }
    public bool Reference { get; set; }
    public bool Hidden { get; set; }
    public bool Editable { get; set; }
    public string? Name { get; set; }
    public string? ExternalId { get; set; }
    public string? Section { get; set; }
    public string? Tag { get; set; }
}