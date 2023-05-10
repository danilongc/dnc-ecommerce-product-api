using System.ComponentModel.DataAnnotations;

namespace DNCEcommerceApi.Data.Dtos;

public class UpdateProductInventoryDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Precisa ser informado no mínimo um item.")]
    public List<UpdateProductWarehouseDto> Warehouses { get; set; }
}

