namespace Cwiczenia7.Services;

public interface IWarehouseService
{
    Task<int> FulfillOrderAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt);
}