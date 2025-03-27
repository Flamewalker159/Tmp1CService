using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Tmp1CService.DTOs.PositionDTOs;

namespace Tmp1CService.DTOs.EmployeesDTOs;

public class Employee1CDto
{
    [JsonProperty("Код")] [Required] public string Code { get; set; } = string.Empty;

    [JsonProperty("Наименование")]
    [Required]
    public string? Name { get; set; }

    [JsonProperty("ДатаРождения")] public DateTime Birthdate { get; set; }

    [JsonProperty("КоличествоДетей")] public int NumberOfChildren { get; set; }

    [JsonConverter(typeof(BoolConverter))]
    [JsonProperty("Работает")]
    public bool Works { get; set; }

    [JsonProperty("Стаж")] public string WorkExperience { get; set; } = string.Empty;

    [JsonProperty("Должность")] public PositionDto? Position { get; set; }

    private class BoolConverter : JsonConverter<bool>
    {
        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            return value == "Да";
        }

        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            writer.WriteValue(value ? "Да" : "Нет");
        }
    }
}