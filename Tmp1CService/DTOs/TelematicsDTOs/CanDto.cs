using Newtonsoft.Json;

namespace Tmp1CService.DTOs.TelematicsDTOs;

public class CanDto
{
    [JsonProperty("СкоростьCAN")]
    public int Speed { get; set; }

    [JsonProperty("ПробегДоСервиса")]
    public int RemainingMileage { get; set; }

    [JsonProperty("УровеньТоплива")]
    public int FuelLevel { get; set; }

    [JsonProperty("РасходТоплива")]
    public int FuelConsumption { get; set; }

    [JsonProperty("ТемператураОЖ")]
    public int CoolantTemp { get; set; }

    [JsonProperty("ДавлениеМасла")]
    public int EngineOilPressure { get; set; }

    [JsonProperty("НапряжениеБортовойСети")]
    public int OnboardPowerVoltage { get; set; }

    [JsonProperty("НизкийУровеньОЖ")]
    public bool CoolantLevelLow { get; set; }

    [JsonProperty("НеисправностьГенератора")]
    public bool GeneratorMalfunction { get; set; }

    [JsonProperty("НизкийУровеньТормознойЖидкости")]
    public bool BreakFluidLowLevel { get; set; }

    [JsonProperty("МаксОборотыДвигателя")]
    public int MaxRpm { get; set; }

    [JsonProperty("УровеньГаза")]
    public int GazLevel { get; set; }

    [JsonProperty("ТипТоплива")]
    public int FuelType { get; set; }
}