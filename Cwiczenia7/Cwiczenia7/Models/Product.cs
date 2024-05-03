using System.ComponentModel.DataAnnotations;

namespace Cwiczenia7.Models;

public class Product(int id, string name, string? description, double price)
{
    [Required]
    [Key]
    public int ProductId { get; } = id;
    
    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = name;
    
    [MaxLength(200)]
    public string? ProductDescription { get; set; } = description;
    
    [Required]
    public double ProductPrice { get; set; } = price;
}