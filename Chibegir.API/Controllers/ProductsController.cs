using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chibegir.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllProductsAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
            return NotFound();

        return Ok(product);
    }


    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAvailableProducts(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAvailableProductsAsync(cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateProductAsync(productDto, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPost("CreateProductWithHtmlAsync")]
    public async Task<ActionResult<ProductDto>> CreateProductWithHtmlAsync([FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateProductWithHtmlAsync(productDto, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] ProductDto productDto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, productDto, cancellationToken);
            return Ok(updatedProduct);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteProductAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

