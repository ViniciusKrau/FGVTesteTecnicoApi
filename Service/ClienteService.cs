using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Repositories;

namespace TesteTecnicoApi.Service;

public class ClienteService {

    private readonly IClienteRepository _repo;
    public ClienteService(IClienteRepository repo) {
        _repo = repo;
    }

    public async Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
                            string sort = "CodCliente",
                            string? nomeContains = null,
                            string? cnpjContains = null,
                            string? emailContains = null,
                            DateTime? minDataCadastro = null,
                            DateTime? maxDataCadastro = null
                            ) {
        return await _repo.GetPageAsync(page, pageSize, cancellationToken,
                            sort,
                            nomeContains,
                            cnpjContains,
                            emailContains,
                            minDataCadastro,
                            maxDataCadastro);
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

    public async Task<Cliente> AddAsync(PostClienteDTO postclienteDTO, CancellationToken cancellationToken = default) {
        var cliente = new Cliente {
            CNPJ = postclienteDTO.CNPJ,
            Nome = postclienteDTO.Nome,
            Email = postclienteDTO.Email,
            DataCadastro = DateTimeOffset.Now
        };
        return await _repo.AddAsync(cliente, cancellationToken);
    }

    public async Task<Cliente> UpdateAsync(PatchClienteDTO patchClienteDTO, CancellationToken cancellationToken = default) {

        Cliente? existing = await _repo.GetByIdAsync(patchClienteDTO.CodCliente, cancellationToken) ?? throw new KeyNotFoundException($"Cliente with ID {patchClienteDTO.CodCliente} not found.");
        existing.CNPJ = !string.IsNullOrWhiteSpace(patchClienteDTO.CNPJ) ? patchClienteDTO.CNPJ : existing.CNPJ;
        existing.Nome = !string.IsNullOrWhiteSpace(patchClienteDTO.Nome) ? patchClienteDTO.Nome : existing.Nome;
        existing.Email = !string.IsNullOrWhiteSpace(patchClienteDTO.Email) ? patchClienteDTO.Email : existing.Email;

        return await _repo.UpdateAsync(existing, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken) {

        var existing = await _repo.GetByIdAsync(id, cancellationToken);
        if (existing == null) throw new KeyNotFoundException($"Cliente with ID {id} not found.");

        await _repo.DeleteAsync(id, cancellationToken);

    }
}