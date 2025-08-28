using TesteTecnicoApi.Entities;

namespace TesteTecnicoApi.Repositories;

public interface IProdutoRepository {
    Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct);
    Task<Produto?> GetByIdAsync(int id, CancellationToken ct);
    Task<PagedResult<Produto>> GetPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken,
        string sort = "CodProduto",
        decimal? minPreco = null,
        decimal? maxPreco = null,
        int? minEstoque = null,
        string? nomeContains = null);

    Task<Produto> AddAsync(Produto produto, CancellationToken ct);
}