namespace Cwiczenia7.Services;

public interface IWarehouseService
{
    Task<int> AddProductToWarehouseAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt);
}