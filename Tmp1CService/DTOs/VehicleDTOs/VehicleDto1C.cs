using Newtonsoft.Json;

namespace Tmp1CService.DTOs;

public class VehicleDto1C
{
    [JsonProperty("Код")] public string Code1C { get; set; } = string.Empty;
    
    [JsonProperty("Наименование")] public string Name { get; set; } = string.Empty;

    [JsonProperty("ГосНомер")] public string LicensePlate { get; set; } = string.Empty;

    [JsonProperty("VIN")] public string Vin { get; set; } = string.Empty;
    [JsonProperty("Марка")] public string Brand { get; set; } = string.Empty;

    [JsonProperty("Модель")] public string Model { get; set; } = string.Empty;

    [JsonProperty("ГруппаТС")] public string GroupId { get; set; } = string.Empty;

    [JsonProperty("ГодВыпуска")] public DateTime YearOfIssue { get; set; }

    [JsonProperty("Масса")] public int Mass { get; set; }

    [JsonProperty("Габариты")] public string Dimensions { get; set; } = string.Empty;

    [JsonProperty("ТипВладенияТС")] public string OwnershipType { get; set; } = string.Empty;

    [JsonProperty("НомерШасси")] public string ChassisNumber { get; set; } = string.Empty;

    [JsonProperty("НомерДвигателя")] public string EngineNumber { get; set; } = string.Empty;

    [JsonProperty("МодельДвигателя")] public string EngineModel { get; set; } = string.Empty;

    [JsonProperty("МощностьДвигателяЛС")] public string PowerEngineInLs { get; set; } = string.Empty;

    [JsonProperty("МощностьДвигателяКвт")] public string PowerEngineInKvt { get; set; } = string.Empty;
}