using TesteTecnicoApi.Entities;

namespace TesteTecnicoApi.Repositories.Interfaces;

public interface IPedidoRepository {
    Task<IReadOnlyList<Pedido>> GetAllAsync(CancellationToken cancellationToken, int codCliente);

    Task<Pedido> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Pedido> AddAsync(Pedido pedido, CancellationToken cancellationToken);

    Task<Pedido> UpdateAsync(Pedido pedido, CancellationToken cancellationToken);
    Task DeleteAsync(int codPedido, CancellationToken cancellationToken);
}