using Newtonsoft.Json;

namespace Tmp1CService.DTOs.VehicleDTOs;

public class VehicleUpdateDto
{
    [JsonProperty("ГосНомер")] public string LicensePlate { get; set; } = string.Empty;

    [JsonProperty("ТипВладенияТС")] public string OwnershipType { get; set; } = string.Empty;

    [JsonProperty("Масса")] public int Mass { get; set; }

    [JsonProperty("Габариты")] public string Dimensions { get; set; } = string.Empty;

    [JsonProperty("НомерШасси")] public string ChassisNumber { get; set; } = string.Empty;

    [JsonProperty("НомерДвигателя")] public string EngineNumber { get; set; } = string.Empty;

    [JsonProperty("МодельДвигателя")] public string EngineModel { get; set; } = string.Empty;

    [JsonProperty("МощностьДвигателяЛС")] public int PowerEngineInLs { get; set; }

    [JsonProperty("МощностьДвигателяКвт")] public int PowerEngineInKvt { get; set; }
}