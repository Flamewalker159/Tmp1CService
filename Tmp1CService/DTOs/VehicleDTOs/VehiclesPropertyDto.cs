﻿namespace Tmp1CService.DTOs.VehicleDTOs;

public class VehiclesPropertyDto
{
    public Guid Id { get; set; }
    public VehiclesAttributeDto? AttributeDto { get; set; }
    public string? StringValue { get; set; }
    public int ValueId { get; set; }
}