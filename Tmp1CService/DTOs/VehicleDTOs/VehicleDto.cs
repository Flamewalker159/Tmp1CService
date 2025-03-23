namespace Tmp1CService.DTOs;

public class VehicleDto
{
    public string Code1C { get; set; } = string.Empty;
    
    public int IdTmp { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string LicensePlate { get; set; } = string.Empty;

    public string Vin { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string GroupId { get; set; } = string.Empty;

    public DateTime YearOfIssue { get; set; }

    public int Mass { get; set; }

    public string Dimensions { get; set; } = string.Empty;

    public string OwnershipType { get; set; } = string.Empty;

    public string ChassisNumber { get; set; } = string.Empty;

    public string EngineNumber { get; set; } = string.Empty;

    public string EngineModel { get; set; } = string.Empty;

    public string PowerEngineInLs { get; set; } = string.Empty;

    public string PowerEngineInKvt { get; set; } = string.Empty;
}