using Cwiczenia7.Exceptions;
using Cwiczenia7.Repositories;

namespace Cwiczenia7.Services;

public class WarehouseService(IWarehouseRepository repository) : IWarehouseService
{
    private readonly IWarehouseRepository _repository = repository;
    
    public async Task<int> FulfillOrderAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt)
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

        var order = await _repository.GetOrderByProductIdAndAmount(idProduct, amount)!;
        if (order is null)
        {
            throw new NoOrderForProductException($"There is no order for product with id {idProduct}.");
        }

        if (createdAt > order.CreatedAt)
        {
            throw new IllegalDateOfCreationException($"The product  with id {idProduct} " +
                                                     $"was created later than the order of it.");
        }

        if (amount < 0)
        {
            throw new IllegalProductAmountException("Amount has to be more than zero.");
        }

        if (await _repository.IsOrderFulfilledAsync(order.OrderId))
        {
            throw new OrderAlreadyFulfilledException($"Order with id {order.OrderId} is already filfilled");
        }
        
        return await _repository.FulfillOrderAsync(idWarehouse, idProduct, order.OrderId, amount, product.ProductPrice);
    }
}