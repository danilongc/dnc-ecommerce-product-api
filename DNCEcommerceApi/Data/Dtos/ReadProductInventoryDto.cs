namespace DNCEcommerceApi.Data.Dtos;

public class ReadProductInventoryDto
{
    public int Quantity {
        get => Warehouses.Sum(c => c.Quantity);
    }
    public List<ReadProductWarehouseDto> Warehouses { get; set; }
}

