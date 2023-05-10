using System.ComponentModel.DataAnnotations;

namespace DNCEcommerceApi.Data.Dtos;

public class CreateProductInventoryDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Precisa ser informado no mínimo um item.")]
    public List<CreateProductWarehouseDto> Warehouses { get; set; }
}

