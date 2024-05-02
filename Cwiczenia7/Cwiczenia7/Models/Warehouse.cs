using System.ComponentModel.DataAnnotations;

namespace Cwiczenia7.Models;

public class Warehouse(int id, string name, string address)
{
    [Required]
    [Key]
    public int WarehouseId { get; } = id;
    
    [Required]
    [MaxLength(200)]
    public string WarehouseName { get; set; } = name;
    
    [Required]
    [MaxLength(200)]
    public string WarehouseAddress { get; set; } = address;
}