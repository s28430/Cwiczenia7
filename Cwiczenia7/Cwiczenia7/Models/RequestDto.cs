namespace Cwiczenia7.Models;

public record RequestDto(int ProductId, int WarehouseId, int Amount, DateTime CreatedAt)
{
}