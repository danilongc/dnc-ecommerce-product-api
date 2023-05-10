using DNCEcommerceApi.Data.Dtos;

namespace DNCEcommerceApi.Services;

public interface IProductService
{
    Task<ReadProductDto?> GetProductDtoBySkuAsync(long sku);
    Task CreateProductAsync(CreateProductDto productDto);
    Task<bool> UpdateProductBySkuaAsync(long sku, UpdateProductDto productDto);
    Task<bool> DeleteProductBySkuAsync(long sku);
}

