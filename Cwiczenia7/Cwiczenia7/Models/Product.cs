namespace Cwiczenia7.Models;

public class Product(int id, string name, string description, double price)
{
    public int ProductId { get; } = id;
    public string ProductName { get; set; } = name;
    public string? ProductDescription { get; set; } = description;
    public double ProductPrice { get; set; } = price;
}