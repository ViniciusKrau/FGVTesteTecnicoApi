using TesteTecnicoApi.Models;

namespace TesteTecnicoApi.Repositories;

public interface IClienteRepository {
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken);
    Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
        string sort = "CodCliente",
        string? nomeContains = null,
        string? cnpjContains = null,
        string? emailContains = null,
        DateTime? minDataCadastro = null,
        DateTime? maxDataCadastro = null);

    Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Cliente?> GetByNameAsync(string name, CancellationToken cancellationToken);
}