using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Repositories.Interfaces;

namespace TesteTecnicoApi.Repositories;

public class SqlItensPedidosRepository : IItensPedidosRepository {
    public Task<ItensPedido> AddAsync(ItensPedido itemPedido, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ItensPedido>> GetAllAsync(CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<ItensPedido?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ItensPedido>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
                                    string sort = "CodItemPedido",
                                    int? minQuantidade = null,
                                    decimal? minPrecoUnitario = null,
                                    decimal? maxPrecoUnitario = null) {
        throw new NotImplementedException();
    }
}
    