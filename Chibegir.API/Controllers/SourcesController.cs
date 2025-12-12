using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chibegir.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SourcesController : ControllerBase
{
    private readonly ISourceService _sourceService;
    private readonly ILogger<SourcesController> _logger;

    public SourcesController(ISourceService sourceService, ILogger<SourcesController> logger)
    {
        _sourceService = sourceService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SourceDto>>> GetAllSources(CancellationToken cancellationToken)
    {
        var sources = await _sourceService.GetAllSourcesAsync(cancellationToken);
        return Ok(sources);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SourceDto>>> GetActiveSources(CancellationToken cancellationToken)
    {
        var sources = await _sourceService.GetActiveSourcesAsync(cancellationToken);
        return Ok(sources);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SourceDto>> GetSourceById(int id, CancellationToken cancellationToken)
    {
        var source = await _sourceService.GetSourceByIdAsync(id, cancellationToken);
        if (source == null)
            return NotFound();

        return Ok(source);
    }

    [HttpPost]
    public async Task<ActionResult<SourceDto>> CreateSource([FromBody] SourceDto sourceDto, CancellationToken cancellationToken)
    {
        var createdSource = await _sourceService.CreateSourceAsync(sourceDto, cancellationToken);
        return CreatedAtAction(nameof(GetSourceById), new { id = createdSource.Id }, createdSource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SourceDto>> UpdateSource(int id, [FromBody] SourceDto sourceDto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedSource = await _sourceService.UpdateSourceAsync(id, sourceDto, cancellationToken);
            return Ok(updatedSource);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSource(int id, CancellationToken cancellationToken)
    {
        var deleted = await _sourceService.DeleteSourceAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

