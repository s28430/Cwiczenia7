using System.Data;
using System.Data.Common;
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

    public async Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price)
    {
        var result = -1;
        const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        
        var now = DateTime.Now;
        
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        await using var transaction = (SqlTransaction) await conn.BeginTransactionAsync();
        cmd.Transaction = transaction;

        cmd.CommandText = "UPDATE order " +
                          "SET fulfilledAt = @fulfilledAt " +
                          "WHERE idOrder = @idOrder";
        cmd.Parameters.AddWithValue("fulfilledAt", now.ToString(dateTimeFormat));
        cmd.Parameters.AddWithValue("idOrder", idOrder);

        cmd.ExecuteNonQuery();
        
        cmd.Parameters.Clear();

        cmd.CommandText = "INSERT INTO product_warehouse(idWarehouse, idProduct, idOrder, amount, price, createdAd) " +
                          "VALUES(@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAd)";
        cmd.Parameters.AddWithValue("idWarehouse", idWarehouse);
        cmd.Parameters.AddWithValue("idProduct", idProduct);
        cmd.Parameters.AddWithValue("idOrder", idOrder);
        cmd.Parameters.AddWithValue("amount", amount);
        cmd.Parameters.AddWithValue("price", price);
        cmd.Parameters.AddWithValue("createdAt", now.ToString(dateTimeFormat));
        
        await cmd.ExecuteNonQueryAsync();
        
        cmd.Parameters.Clear();

        cmd.CommandText = "SELECT SCOPE_IDENTITY()";
        
        try
        {
            var obj = await cmd.ExecuteScalarAsync();
            if (obj is not null)
            {
                result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }
        catch (SqlException)
        {
            transaction.Rollback();
        }
        transaction.Commit();

        return result;
    }

    public async Task<int> FulfillOrderProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime createdAt)
    {
        var result = -1;
        
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@idProduct", idProduct);
        cmd.Parameters.AddWithValue("@idWarehouse", idWarehouse);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@createdAt", createdAt);
        
        var obj = await cmd.ExecuteScalarAsync();

        if (obj is not null)
        {
            result = Convert.ToInt32(obj);
        }


        return result;
    }
}