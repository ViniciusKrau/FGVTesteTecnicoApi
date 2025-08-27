using TesteTecnicoApi.Models;

namespace TesteTecnicoApi.Repositories;

public interface IProdutoRepository {
    Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken ct);
    Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken ct);
}