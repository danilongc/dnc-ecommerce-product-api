using System;
using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Data.Models;

namespace DNCEcommerceApi.Tests.Mocks;

public class ProductDataMock
{

    public static CreateProductDto BuildCreateProductDto(long sku)
    {
        CreateProductDto dto = new CreateProductDto
        {
            Name = "Produto X",
            Sku = sku,
            Inventory = new CreateProductInventoryDto
            {
                Warehouses = new List<CreateProductWarehouseDto>{
                    new CreateProductWarehouseDto
                    {
                        Locality = "MG",
                        Quantity = 99,
                        Type = "ECOMMERCE"
                    },
                    new CreateProductWarehouseDto
                    {
                        Locality = "SP",
                        Quantity = 99,
                        Type = "PHYSICAL_STORE"
                    }

                }
            }
        };

        return dto;
    }

    public static UpdateProductDto BuildUpdateProductDto(long sku, string name)
    {
        UpdateProductDto dto = new UpdateProductDto
        {
            Name = name,
            Inventory = new UpdateProductInventoryDto
            {
                Warehouses = new List<UpdateProductWarehouseDto>{
                    new UpdateProductWarehouseDto
                    {
                        Locality = "MG",
                        Quantity = 99,
                        Type = "ECOMMERCE"
                    }
                }
            }
        };

        return dto;
    }

    public static Product BuildProduct()
    {
        return new Product()
        {
            Id = 1,
            Name = "Product name",
            Sku = 123,
            Warehouses = new List<Warehouse>()
            {
                new Warehouse()
                {
                    Locality = "BR",
                    Quantity = 60,
                    ProductKey = 1,
                    Type = "ECOMMERCE",
                    Id =  1
                },
                new Warehouse()
                {
                    Locality = "SP",
                    Quantity = 10,
                    ProductKey = 1,
                    Type = "PHYSICAL_STORE",
                    Id =  2
                }
            }
        };
    }
}

