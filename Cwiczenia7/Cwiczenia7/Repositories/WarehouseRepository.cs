using System.Data.SqlClient;
using Cwiczenia7.Models;

namespace Cwiczenia7.Repositories;

public class WarehouseRepository(IConfiguration configuration) : IWarehouseRepository
{
    private readonly IConfiguration _configuration = configuration;
    public async Task<Product?>? GetProductByIdAsync(int id)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT idProduct, name, description, price " +
                          "FROM product " +
                          "WHERE idProduct = @id";
        cmd.Parameters.AddWithValue("id", id);
        await using var dr = await cmd.ExecuteReaderAsync();

        if (!await dr.ReadAsync())
        {
            return null;
        }
        
        var product = new Product(
            id: (int)dr["idProduct"],
            name: dr["name"].ToString() ?? string.Empty,
            description: dr["description"].ToString() ?? null,
            price: (double)dr["price"]
        );
        return product;

    }

    public async Task<Warehouse?>? GetWarehouseByIdAsync(int id)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT idWarehouse, name, address " +
                          "FROM warehouse " +
                          "WHERE idWarehouse = @id";
        cmd.Parameters.AddWithValue("id", id);

        await using var dr = await cmd.ExecuteReaderAsync();

        if (!await dr.ReadAsync())
        {
            return null;
        }

        var warehouse = new Warehouse(
            id: (int)dr["idWarehouse"],
            name: dr["name"].ToString() ?? string.Empty,
            address: dr["address"].ToString() ?? string.Empty
        );

        return warehouse;
    }

    public async Task<Order?>? GetOrderByProductIdAndAmount(int idProduct, int amount)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT idOrder, idProduct, amount, createdAt, fulfilledAt " +
                          "FROM order " +
                          "WHERE idProduct = @idProduct AND amount = @amount";
        cmd.Parameters.AddWithValue("idProduct", idProduct);
        cmd.Parameters.AddWithValue("amount", amount);

        await using var dr = await cmd.ExecuteReaderAsync();
        if (!await dr.ReadAsync()) return null;

        var order = new Order
        {
            OrderId = (int)dr["idOrder"],
            ProductId = (int)dr["idProduct"],
            ProductAmount = (int)dr["amount"],
            CreatedAt = DateTime.Parse(dr["createdAt"].ToString() ?? string.Empty),
            FulfilledAt = DateTime.Parse(dr["fulfilledAt"].ToString() ?? string.Empty)
        };
        return order;
    }

    public async Task<bool> IsOrderFulfilledAsync(int idOrder)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT COUNT(1) AS count " +
                          "FROM product_warehouse " +
                          "WHERE idOrder = @idOrder";
        cmd.Parameters.AddWithValue("idOrder", idOrder);
        
        await using var dr = await cmd.ExecuteReaderAsync();
        if (await dr.ReadAsync())
        {
            return (int)dr["count"] > 0;
        }

        return false;
    }
}