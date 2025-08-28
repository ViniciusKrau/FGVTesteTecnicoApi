using TesteTecnicoApi.Models;

namespace TesteTecnicoApi.Repositories;

public interface IClienteRepository {
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken ct);
    Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken ct,
        string sort = "CodCliente",
        string? nomeContains = null,
        string? cnpjContains = null,
        string? emailContains = null,
        DateTime? minDataCadastro = null,
        DateTime? maxDataCadastro = null);

    Task<Cliente?> GetByIdAsync(int id, CancellationToken ct);

    Task<Cliente?> GetByNameAsync(string name, CancellationToken ct);
}