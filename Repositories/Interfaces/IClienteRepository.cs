using TesteTecnicoApi.Entities;

namespace TesteTecnicoApi.Repositories;

public interface IClienteRepository {

    Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
        string sort = "CodCliente",
        string? nomeContains = null,
        string? cnpjContains = null,
        string? emailContains = null,
        DateTime? minDataCadastro = null,
        DateTime? maxDataCadastro = null);
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken);

    Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Cliente?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<Cliente> AddAsync(Cliente cliente, CancellationToken cancellationToken);

    Task<Cliente> UpdateAsync(Cliente cliente, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}