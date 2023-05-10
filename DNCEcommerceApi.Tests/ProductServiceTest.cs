using DNCEcommerceApi.Data;
using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using AutoMapper;
using DNCEcommerceApi.Profiles;
using DNCEcommerceApi.Data.Models;
using DNCEcommerceApi.Exceptions;
using DNCEcommerceApi.Tests.Mocks;

namespace DNCEcommerceApi.Tests;

public class ProductServiceTest : IDisposable
{
    private readonly AppDbContext _appDbContext;
    private IMapper _mapper;

    public ProductServiceTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options);
        _appDbContext.Database.EnsureCreated();


        initMapper();
        initDataBase();

    }

    private void initMapper()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductProfile());
        });
        _mapper = mapperConfig.CreateMapper();
    }

    private void initDataBase()
    {
        _appDbContext.Add(ProductDataMock.BuildProduct());
        _appDbContext.SaveChanges();

    }

    public void Dispose()
    {
        _appDbContext.Database.EnsureDeleted();
        _appDbContext.Dispose();
    }

    [Fact]
    public async Task ShouldCreateProduct()
    {
        //Arrange
        int actualTableSize = _appDbContext.Products.Count();
        var sut = new ProductService(_appDbContext, _mapper);

        //Act
        await sut.CreateProductAsync(ProductDataMock.BuildCreateProductDto(666));

        //Assert
        _appDbContext.Products.Count().Should().Be(actualTableSize + 1);
    }

    [Fact]
    public async Task ShouldFailCreateProductSkuConstraint()
    {
        //Arrange
        long sku = 123;
        int actualTableSize = _appDbContext.Products.Count();
        var sut = new ProductService(_appDbContext, _mapper);

        //Act
        Func<Task> act = async () => await sut.CreateProductAsync(ProductDataMock.BuildCreateProductDto(123));

        //Assert
        _appDbContext.Products.Count().Should().Be(actualTableSize);
        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ShouldReturnProductBySku()
    {
        //Arrange
        long sku = 123;
        var sut = new ProductService(_appDbContext, _mapper);

        //Act
        ReadProductDto? readProductDto = await sut.GetProductDtoBySkuAsync(sku);

        //Assert
        readProductDto.Should().NotBeNull();
        readProductDto.Inventory.Should().NotBeNull();
        readProductDto.Inventory.Warehouses.Should().NotBeNullOrEmpty();
        readProductDto.Inventory.Warehouses.Count.Should().Be(2);
    }

    [Fact]
    public async Task ShouldUpdateProductBySku()
    {
        //Arrange
        long sku = 123;
        string name = "Produto Alterado";

        var sut = new ProductService(_appDbContext, _mapper);

        //Act
        bool updated = await sut.UpdateProductBySkuaAsync(sku, ProductDataMock.BuildUpdateProductDto(sku, name));

        //Assert
        Product productUpdated = _appDbContext.Products
            .Where(p => p.Sku == sku)
            .Single();

        updated.Should().BeTrue();
        productUpdated.Name.Should().Be(name);
        productUpdated.Warehouses.Count().Should().Be(1);
    }

    [Fact]
    public async Task ShouldDeleteProductBySku()
    {
        //Arrange
        long sku = 123;
        var sut = new ProductService(_appDbContext, _mapper);

        //Act
        var deleted = await sut.DeleteProductBySkuAsync(sku);

        //Assert
        Product? product = _appDbContext.Products
            .Where(p => p.Sku == sku)
            .SingleOrDefault();
        deleted.Should().BeTrue();
        product.Should().BeNull();
    }
}
