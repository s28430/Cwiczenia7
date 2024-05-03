using System.Data.SqlClient;
using Cwiczenia7.Models;

namespace Cwiczenia7.Repositories;

public class WarehouseRepository(IConfiguration configuration) : IWarehouseRepository
{
    public async Task<Product?>? GetProductByIdAsync(int id)
    {
        await using var conn = new SqlConnection(configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT idProduct, name, description, price " +
                          "FROM product " +
                          "WHERE idProduct = @id";
        cmd.Parameters.AddWithValue("id", id);
        await using var dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            var product = new Product(
                id: (int)dr["idProduct"],
                name: dr["name"].ToString() ?? string.Empty,
                description: dr["description"].ToString() ?? null,
                price: (double)dr["price"]
            );
            return product;
        }

        return null;
    }
}