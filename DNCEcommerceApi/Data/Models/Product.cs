using System.ComponentModel.DataAnnotations;

namespace DNCEcommerceApi.Data.Models;

public class Product
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    public long Sku { get; set; }

    [Required]
    [MaxLength(250)]
    public string Name { get; set; }

    public ICollection<Warehouse> Warehouses { get; set; }

}

    