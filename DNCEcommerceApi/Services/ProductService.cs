using AutoMapper;
using DNCEcommerceApi.Data;
using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Exceptions;
using DNCEcommerceApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DNCEcommerceApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    #region Public Members

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public Task CreateProductAsync(CreateProductDto productDto)
    {
        ValidateProduct(productDto);

        var warehouses = _mapper.Map<List<Warehouse>>(productDto.Inventory.Warehouses);
        var product = _mapper.Map<Product>(productDto);
        product.Warehouses = warehouses;
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<bool> UpdateProductBySkuaAsync(long sku, UpdateProductDto productDto)
    {
        if (ProductSkuExists(sku))
        {
            var productToUpdate = (from p in _dbContext.Products where p.Sku == sku select p)
                   .Include(p => p.Warehouses)
                   .SingleOrDefault();

            this._mapper.Map(productDto, productToUpdate);

            if (productDto.Inventory != null
                && productDto.Inventory.Warehouses != null
                && productDto.Inventory.Warehouses.Count > 0
              )
            {
                this._mapper.Map(productDto.Inventory.Warehouses, productToUpdate.Warehouses);
            }

            _dbContext.Products.Update(productToUpdate);
            _dbContext.SaveChanges();

            return Task.FromResult<bool>(true);
        }

        return Task.FromResult(false);
    }

    public Task<ReadProductDto?> GetProductDtoBySkuAsync(long sku)
    {
        DbSet<Product> products = _dbContext.Products;
        DbSet<Warehouse> warehouses = _dbContext.Warehouses;

        var product = (from p in _dbContext.Products where p.Sku == sku select p)
                     .Include(p => p.Warehouses)
                     .SingleOrDefault();

        if (product == null)
        {
            return Task.FromResult<ReadProductDto?>(null);
        }

        ReadProductDto readProductDto = _mapper.Map<ReadProductDto>(product);
        readProductDto.Name=$"Nome: {readProductDto.Name}";
        return Task.FromResult<ReadProductDto?>(readProductDto);
    }

    public Task<bool> DeleteProductBySkuAsync(long sku)
    {
        Product? product = GetProductBySku(sku);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    #endregion

    #region Private Members

    private Product? GetProductBySku(long sku)
    {
        return _dbContext.Products
            .Where(p => p.Sku == sku)
            .SingleOrDefault();
    }

    #region Validations

    private void ValidateProduct(CreateProductDto productDto)
    {
        if (ProductSkuExists(productDto.Sku))
        {
            throw new BusinessException()
            {
                Code = ErrorCodeEnum.PRODUCT_ALREADY_EXISTS,
                Message = $"Já existe um produto cadastrado com o sku informado: {productDto.Sku}"
            };
        }
    }

    private bool ProductSkuExists(long sku)
    {
        return _dbContext.Products
            .Where(p => p.Sku == sku)
            .Any();
    }

    #endregion

    #endregion


}

