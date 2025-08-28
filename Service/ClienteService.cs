

using TesteTecnicoApi.Models;
using TesteTecnicoApi.Repositories;

namespace TesteTecnicoApi.Service;

public class ClienteService {

    private readonly IClienteRepository _repo;
    public ClienteService(IClienteRepository repo) {
        _repo = repo;
    }

    public async Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        return await _repo.GetPageAsync(page, pageSize, cancellationToken);
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
        return await _repo.GetByIdAsync(id, cancellationToken);
    }
    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _repo.GetAllAsync(cancellationToken);
    }
    public async Task<Cliente?> GetByNameAsync(string name, CancellationToken cancellationToken = default) {
        return await _repo.GetByNameAsync(name, cancellationToken);
    }
    

}