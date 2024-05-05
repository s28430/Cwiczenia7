using Cwiczenia7.Models;

namespace Cwiczenia7.Repositories;

public interface IWarehouseRepository
{ 
    Task<Product?>? GetProductByIdAsync(int id);
    Task<Warehouse?>? GetWarehouseByIdAsync(int id);
    Task<Order?>? GetOrderByProductIdAndAmount(int idProduct, int amount);
    Task<bool> IsOrderFulfilledAsync(int idOrder);
    Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price);
    Task<int> FulfillOrderProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime createdAt);
}