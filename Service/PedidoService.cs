using TesteTecnicoApi.Repositories.Interfaces;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs;

namespace TesteTecnicoApi.Service;
public class PedidoService(IPedidoRepository pedidoRepository) {

    public async Task<Pedido> CreatePedidoAsync(PostPedidoDTO postPedidoDTO, CancellationToken cancellationToken) {
        

        var pedido = new Pedido {
            CodCliente = postPedidoDTO.CodCliente,
            ValorTotal = postPedidoDTO.ValorTotal,
            DataPedido = DateTimeOffset.UtcNow,
        };
        return await pedidoRepository.AddAsync(pedido, cancellationToken);
    }

    public async Task<IReadOnlyList<Pedido>> GetAllPedidosAsync(int codCliente, CancellationToken cancellationToken) {
        return await pedidoRepository.GetAllAsync(cancellationToken, codCliente);
    }

    public async Task<Pedido> GetPedidoByIdAsync(int codPedido, CancellationToken cancellationToken) {
        return await pedidoRepository.GetByIdAsync(codPedido, cancellationToken);
    }

    public async Task<Pedido> UpdatePedidoAsync(Pedido pedido, CancellationToken cancellationToken) {
        return await pedidoRepository.UpdateAsync(pedido, cancellationToken);
    }
}
