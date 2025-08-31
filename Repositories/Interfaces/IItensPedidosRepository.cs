namespace TesteTecnicoApi.Repositories.Interfaces;

using TesteTecnicoApi.Entities;

public interface IItensPedidosRepository {
    Task<IReadOnlyList<ItensPedido>> GetAllAsync(CancellationToken cancellationToken);
    Task<ItensPedido?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PagedResult<ItensPedido>> GetPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken,
        string sort = "CodItemPedido",
        int? minQuantidade = null,
        decimal? minPrecoUnitario = null,
        decimal? maxPrecoUnitario = null);

    Task<ItensPedido> AddAsync(ItensPedido itemPedido, CancellationToken cancellationToken);
    Task<List<ItensPedido>> GetByCodProdutoAsync(int codProduto, CancellationToken cancellationToken);
}