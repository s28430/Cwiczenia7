using System.ComponentModel.DataAnnotations;

namespace Cwiczenia7.Models;

public class Order(int id, int amount, DateTime createdAt, DateTime fulfilledAt, int idProduct)
{
    [Required]
    [Key]
    public int OrderId { get; } = id;

    [Required]
    public int ProductId { get; set; } = idProduct;

    [Required]
    public int ProductAmount { get; set; } = amount;

    [Required]
    public DateTime CreatedAt { get; set; } = createdAt;

    public DateTime FulfilledAt { get; set; } = fulfilledAt;
}