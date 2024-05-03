using System.ComponentModel.DataAnnotations;

namespace Cwiczenia7.Models;

public class Order
{
    [Required]
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int ProductAmount { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? FulfilledAt { get; set; }
}