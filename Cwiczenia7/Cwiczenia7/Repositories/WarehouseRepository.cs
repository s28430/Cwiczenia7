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
}