using System;

namespace Inventory.Application.Features.Kardex.DTOs;

public record KardexMovementDto(string MovementType, DateTime MovementDate, decimal Quantity, decimal Balance, string? Reason);
