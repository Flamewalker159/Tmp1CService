using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tmp1CService.DTOs.TelematicsDTOs;

public class ErrorCodeDto
{
    [JsonProperty("ИдентификаторЛампы")]
    public int Id { get; set; }

    [JsonProperty("СостояниеЛампы")]
    public bool State { get; set; }

    [JsonProperty("КодыНеисправностей")]
    public List<string>? ErrorCodes { get; set; }
}