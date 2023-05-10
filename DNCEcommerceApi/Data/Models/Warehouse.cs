using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNCEcommerceApi.Data.Models;

public class Warehouse
{
    [Key]
    [Required]
    public long Id { get; set; }

    [ForeignKey(nameof(Product))]
    public long ProductKey { get; set; }

    public Product Product { get; init; }

    [MaxLength(150)]
    public string Locality { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }
}