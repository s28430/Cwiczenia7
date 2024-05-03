using Cwiczenia7.Exceptions;
using Cwiczenia7.Models;
using Cwiczenia7.Repositories;

namespace Cwiczenia7.Services;

public class WarehouseService(IWarehouseRepository repository) : IWarehouseService
{
    private readonly IWarehouseRepository _repository = repository;
    
    public async Task<int> AddProductToWarehouseAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt)
    {
        var product = await _repository.GetProductByIdAsync(idProduct)!;
        if (product is null)
        {
            throw new NoSuchProductException($"Product with id {idProduct} was not found.");
        }

        var warehouse = await _repository.GetWarehouseByIdAsync(idWarehouse)!;
        if (warehouse is null)
        {
            throw new NoSuchWarehouseException($"Warehouse with id {idWarehouse} was not found.");
        }
        return 0;
    }
}