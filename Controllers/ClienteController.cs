using Microsoft.AspNetCore.Mvc;
using TesteTecnicoApi.Models;
using TesteTecnicoApi.Service;

namespace TesteTecnicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase {
    private readonly ClienteService _service;
    public ClienteController(ClienteService service) {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetClientes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default) {
        var result = await _service.GetPageAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
        Response.Headers["X-Total-Pages"] = result.TotalPages.ToString();

        return Ok(new {
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages,
            Data = result.Items
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetClienteById(int id, CancellationToken cancellationToken = default) {
        var cliente = await _service.GetByIdAsync(id, cancellationToken);
        if (cliente == null) {
            return NotFound();
        }
        return Ok(cliente);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<Cliente>>> GetAllClientes(CancellationToken cancellationToken = default) {
        var clientes = await _service.GetAllAsync(cancellationToken);
        return Ok(clientes);
    }
}