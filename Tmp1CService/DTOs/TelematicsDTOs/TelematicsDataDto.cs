using System.ComponentModel.DataAnnotations;

namespace Tmp1CService.DTOs.TelematicsDTOs;

public class TelematicsDataDto
{
    [Required]
    public GpsDto Gps { get; set; } = new GpsDto();

    [Required]
    public CanDto Can { get; set; } = new CanDto();
}