using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Repositories;
using TesteTecnicoApi.Repositories.Interfaces;

namespace TesteTecnicoApi.Service;

public class ClienteService {

    private readonly IClienteRepository _clienterepo;
    private readonly IPedidoRepository _pedidoRepository;
    public ClienteService(IClienteRepository repo, IPedidoRepository pedidoRepository) {
        _clienterepo = repo;
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PagedResult<Cliente>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
                            string sort = "CodCliente",
                            string? nomeContains = null,
                            string? cnpjContains = null,
                            string? emailContains = null,
                            DateTime? minDataCadastro = null,
                            DateTime? maxDataCadastro = null
                            ) {
        return await _clienterepo.GetPageAsync(page, pageSize, cancellationToken,
                            sort,
                            nomeContains,
                            cnpjContains,
                            emailContains,
                            minDataCadastro,
                            maxDataCadastro);
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
        return await _clienterepo.GetByIdAsync(id, cancellationToken);
    }
    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _clienterepo.GetAllAsync(cancellationToken);
    }
    public async Task<Cliente?> GetByNameAsync(string name, CancellationToken cancellationToken = default) {
        return await _clienterepo.GetByNameAsync(name, cancellationToken);
    }

    public async Task<Cliente> AddAsync(PostClienteDTO postclienteDTO, CancellationToken cancellationToken = default) {
        var cliente = new Cliente {
            CNPJ = postclienteDTO.CNPJ,
            Nome = postclienteDTO.Nome,
            Email = postclienteDTO.Email,
            DataCadastro = DateTimeOffset.Now
        };
        return await _clienterepo.AddAsync(cliente, cancellationToken);
    }

    public async Task<Cliente> UpdateAsync(PatchClienteDTO patchClienteDTO, CancellationToken cancellationToken = default) {

        Cliente? existing = await _clienterepo.GetByIdAsync(patchClienteDTO.CodCliente, cancellationToken) ?? throw new KeyNotFoundException($"Cliente with ID {patchClienteDTO.CodCliente} not found.");
        existing.CNPJ = !string.IsNullOrWhiteSpace(patchClienteDTO.CNPJ) ? patchClienteDTO.CNPJ : existing.CNPJ;
        existing.Nome = !string.IsNullOrWhiteSpace(patchClienteDTO.Nome) ? patchClienteDTO.Nome : existing.Nome;
        existing.Email = !string.IsNullOrWhiteSpace(patchClienteDTO.Email) ? patchClienteDTO.Email : existing.Email;

        return await _clienterepo.UpdateAsync(existing, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken) {

        var existing = await _clienterepo.GetByIdAsync(id, cancellationToken);
        if (existing == null) throw new KeyNotFoundException($"Cliente with ID {id} not found.");

        await _clienterepo.DeleteAsync(id, cancellationToken);

    }

    internal async Task<Cliente?> GetByCNPJAsync(string cnpj, CancellationToken cancellationToken) {
        Cliente cliente = await _clienterepo.GetByCNPJAsync(cnpj, cancellationToken) ?? throw new KeyNotFoundException($"Cliente with CNPJ {cnpj} not found.");
        IReadOnlyList<Pedido> pedidos = await _pedidoRepository.GetAllByClienteAsync(cliente.CodCliente, cancellationToken);
        cliente.Pedidos = [.. pedidos];
        return cliente;
    }
}