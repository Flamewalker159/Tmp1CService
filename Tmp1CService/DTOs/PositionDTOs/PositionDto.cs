using Newtonsoft.Json;

namespace Tmp1CService.DTOs;

public class PositionDto
{
    [JsonProperty("Код")] public string? Code { get; set; }

    [JsonProperty("Наименование")] public string? Description { get; set; }
}