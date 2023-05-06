using System.ComponentModel.DataAnnotations;

namespace DNCEcommerceApi.Data.Dtos;

public class UpdateProductWarehouseDto
{
    [Required]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "O campo locality deve conter no mínimo 2 caracteres e no máximo 150")]
    public string Locality { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo type pode conter no máximo 50 caracteres.")]
    public string Type { get; set; }

}

