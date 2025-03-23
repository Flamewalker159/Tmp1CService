using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tmp1CService.DTOs.TelematicsDTOs;

public class GpsDto
{
    [Required]
    [Range(-360000, 360000, ErrorMessage = "Долгота должна быть в диапазоне -360000...360000")]
    [JsonProperty("Долгота")]
    public long Longitude { get; set; }

    [Required]
    [Range(-360000, 360000, ErrorMessage = "Широта должна быть в диапазоне -360000...360000")]
    [JsonProperty("Широта")]
    public long Latitude { get; set; }

    [JsonProperty("Высота")]
    public int Altitude { get; set; }
    
    [JsonProperty("Курс")]
    public int Course { get; set; }
    
    [JsonProperty("ЧислоСпутников")]
    public int SatellitesAmount { get; set; }
    
    [JsonProperty("Валидность")]
    public int Validity { get; set; }
    
    [JsonProperty("ВремяПоСпутнику")]
    public long GnssTimestamp { get; set; }
    
    [JsonProperty("ВремяСообщения")]
    public long Timestamp { get; set; }
    
    [JsonProperty("УровеньСигналаGSM")]
    public int? GsmCellMonitor { get; set; }
    
    [JsonProperty("СкоростьGPS")]
    public int Speed { get; set; }
    
    [JsonProperty("НапряжениеБатареи")]
    public int AccVoltage { get; set; }
    
    [JsonProperty("ЗажиганиеВключено")]
    public bool IgnitionStatus { get; set; }
    
    [JsonProperty("СостояниеБатареи")]
    public bool PowerStatus { get; set; }
    
    [JsonProperty("СработалAirbag")]
    public bool AirbagFired { get; set; }
    
    [JsonProperty("Одометр")]
    public int Odometer { get; set; }
    
    [JsonProperty("КодыОшибок")]
    public List<ErrorCodeDto> ErrorCodes { get; set; } = [];
}