using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chibegir.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductSourcesController : ControllerBase
{
    private readonly IProductSourceService _productSourceService;
    private readonly ILogger<ProductSourcesController> _logger;

    public ProductSourcesController(IProductSourceService productSourceService, ILogger<ProductSourcesController> logger)
    {
        _productSourceService = productSourceService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductSourceDto>>> GetAllProductSources(CancellationToken cancellationToken)
    {
        var productSources = await _productSourceService.GetAllProductSourcesAsync(cancellationToken);
        return Ok(productSources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductSourceDto>> GetProductSourceById(int id, CancellationToken cancellationToken)
    {
        var productSource = await _productSourceService.GetProductSourceByIdAsync(id, cancellationToken);
        if (productSource == null)
            return NotFound();

        return Ok(productSource);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<ProductSourceDto>>> GetProductSourcesByProductId(int productId, CancellationToken cancellationToken)
    {
        var productSources = await _productSourceService.GetProductSourcesByProductIdAsync(productId, cancellationToken);
        return Ok(productSources);
    }

    [HttpGet("source/{sourceId}")]
    public async Task<ActionResult<IEnumerable<ProductSourceDto>>> GetProductSourcesBySourceId(int sourceId, CancellationToken cancellationToken)
    {
        var productSources = await _productSourceService.GetProductSourcesBySourceIdAsync(sourceId, cancellationToken);
        return Ok(productSources);
    }

    [HttpPost]
    public async Task<ActionResult<ProductSourceDto>> CreateProductSource([FromBody] ProductSourceDto productSourceDto, CancellationToken cancellationToken)
    {
        var createdProductSource = await _productSourceService.CreateProductSourceAsync(productSourceDto, cancellationToken);
        return CreatedAtAction(nameof(GetProductSourceById), new { id = createdProductSource.Id }, createdProductSource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductSourceDto>> UpdateProductSource(int id, [FromBody] ProductSourceDto productSourceDto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedProductSource = await _productSourceService.UpdateProductSourceAsync(id, productSourceDto, cancellationToken);
            return Ok(updatedProductSource);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductSource(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productSourceService.DeleteProductSourceAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
