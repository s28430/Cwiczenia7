using Cwiczenia7.Models;

namespace Cwiczenia7.Repositories;

public interface IWarehouseRepository
{ 
    Task<Product?>? GetProductByIdAsync(int id);
    Task<Warehouse?>? GetWarehouseByIdAsync(int id);
}