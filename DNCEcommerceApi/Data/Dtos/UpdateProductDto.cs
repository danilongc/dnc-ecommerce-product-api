using System.ComponentModel.DataAnnotations;

namespace DNCEcommerceApi.Data.Dtos;

public class UpdateProductDto
{
    [StringLength(250, MinimumLength = 3, ErrorMessage = "O nome do produto deve conter no mínimo 3 caracteres e no máximo 250.")]
    public string Name { get; set; }

    public UpdateProductInventoryDto? Inventory { get; set; }

}

