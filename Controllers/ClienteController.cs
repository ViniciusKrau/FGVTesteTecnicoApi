using Microsoft.AspNetCore.Mvc;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
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

    [HttpGet("cnpj/{cnpj}")]
    public async Task<ActionResult<Cliente>> GetClienteByCNPJ(string cnpj, CancellationToken cancellationToken = default) {
        try {
            var cliente = await _service.GetByCNPJAsync(cnpj, cancellationToken);
            return Ok(cliente);
        } catch (KeyNotFoundException) {
            return NotFound();
        }
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

    [HttpPost("create")]
    public async Task<ActionResult<Cliente>> addCliente([FromBody] PostClienteDTO postclienteDto, CancellationToken cancellationToken = default) {
        if (postclienteDto == null) {
            return BadRequest("Cliente data is required.");
        }
        if (string.IsNullOrWhiteSpace(postclienteDto.CNPJ) ||
            string.IsNullOrWhiteSpace(postclienteDto.Nome) ||
            string.IsNullOrWhiteSpace(postclienteDto.Email)) {
            return BadRequest("CNPJ, Nome, and Email are required fields.");
        }
        var createdCliente = await _service.AddAsync(postclienteDto, cancellationToken);
        return CreatedAtAction(nameof(GetClienteById), new { id = createdCliente.CodCliente }, createdCliente);
    }

    [HttpPatch("patch")]
    public async Task<ActionResult<Cliente>> UpdateCliente([FromBody] PatchClienteDTO patchClienteDto, CancellationToken cancellationToken = default) {
        if (patchClienteDto.CodCliente <= 0) return BadRequest("Valid CodCliente is required.");
        if (patchClienteDto == null) return BadRequest("Patch body is required.");
        var updated = await _service.UpdateAsync(patchClienteDto, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCliente(int id, CancellationToken cancellationToken = default) {
        if (id <= 0) return BadRequest("Invalid ID.");
        var existing = await _service.GetByIdAsync(id, cancellationToken);
        if (existing == null) return NotFound();

        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}