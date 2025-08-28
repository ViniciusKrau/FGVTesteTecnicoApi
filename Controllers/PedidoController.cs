using Microsoft.AspNetCore.Mvc;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Service;

namespace TesteTecnicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase {
    private readonly PedidoService _pedidoService;

    public PedidoController(PedidoService pedidoService) {
        _pedidoService = pedidoService;
    }

    [HttpGet("{codPedido}")]
    public async Task<ActionResult<Pedido>> GetPedidoById(int codPedido, CancellationToken cancellationToken) {
        var pedido = await _pedidoService.GetPedidoByIdAsync(codPedido, cancellationToken);
        if (pedido == null) {
            return NotFound();
        }
        return Ok(pedido);
    }

    [HttpGet("cliente/{codCliente}")]
    public async Task<ActionResult<IReadOnlyList<Pedido>>> GetAllPedidos(int codCliente, CancellationToken cancellationToken) {
        var pedidos = await _pedidoService.GetAllPedidosAsync(codCliente, cancellationToken);
        return Ok(pedidos);
    }

    [HttpPatch("{codPedido}")]
    public async Task<ActionResult<Pedido>> UpdatePedido([FromBody] PatchPedidoDTO patchPedidoDTO, CancellationToken cancellationToken) {
        var updatedPedido = await _pedidoService.UpdatePedidoAsync(patchPedidoDTO, cancellationToken);
        return Ok(updatedPedido);
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> CreatePedido([FromBody] PostPedidoDTO postPedidoDTO, CancellationToken cancellationToken) {
        var createdPedido = await _pedidoService.CreatePedidoAsync(postPedidoDTO, cancellationToken);
        return CreatedAtAction(nameof(GetPedidoById), new { codPedido = createdPedido.CodPedido }, createdPedido);
    }

}