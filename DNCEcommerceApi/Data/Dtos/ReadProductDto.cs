namespace DNCEcommerceApi.Data.Dtos;

public class ReadProductDto
{
    public long Sku { get; set; }
    public string Name { get; set; }
    public ReadProductInventoryDto Inventory { get; set; } = new();
    public bool IsMarketable
    {
        get => Inventory.Quantity > 0;
    }


}

