using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DNCEcommerceApi.Controllers;

[Route("/api/v1/product")]
[Produces("application/json")]  
[ApiController]
public class ProductControllerV1 : ControllerBase
{
    private readonly IProductService _productService;

    public ProductControllerV1(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Returns a product
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    /// <response code="200">On success</response>
    /// <response code="404">If it doesn't could be found</response>
    [HttpGet("{sku}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(type: typeof(ReadProductDto))]
    public async Task<IActionResult> GetProductBySku(long sku)
    {
        ReadProductDto? productDto = await _productService.GetProductDtoBySkuAsync(sku);
        if (productDto != null)
        {
            return Ok(productDto);
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates a product
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns></returns>
    /// <response code="201">On success</response>
    /// <response code="422">If already exists a product with the same sku</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        await _productService.CreateProductAsync(productDto);
        return CreatedAtAction(nameof(GetProductBySku), new { sku = productDto.Sku }, productDto);
    }

    /// <summary>
    /// Deletes a product by sku
    /// </summary>
    /// <param name="sku"></param>
    /// <returns></returns>
    /// <response code="204">Even if the product with sku sent exists </response>
    [HttpDelete("{sku}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProduct(long sku)
    {
        await _productService.DeleteProductBySkuAsync(sku);
        return NoContent();
    }

    /// <summary>
    /// Updates a product
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="productDto"></param>
    /// <returns></returns>
    /// <response code="200">When everything goes ok</response>
    /// <response code="422">If already exists a product with the same sku</response>
    [HttpPut("{sku}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct(long sku, [FromBody] UpdateProductDto productDto)
    {
        if (await _productService.UpdateProductBySkuaAsync(sku, productDto))
        {
            return Ok();
        }
        else
        {
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}

