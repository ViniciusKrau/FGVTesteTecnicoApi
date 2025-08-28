using TesteTecnicoApi.Entities.DTOs;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Repositories;

namespace TesteTecnicoApi.Service;

public class ProdutoService {

    private readonly IProdutoRepository _repo;
    public ProdutoService(IProdutoRepository repo) {
        _repo = repo;
    }

    public async Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        return await _repo.GetPageAsync(page, pageSize, cancellationToken);
    }
    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
        return await _repo.GetByIdAsync(id, cancellationToken);
    }
    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _repo.GetAllAsync(cancellationToken);
    }
    public async Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default,
        string sort = "CodProduto",
        decimal? minPreco = null,
        decimal? maxPreco = null,
        int? minEstoque = null,
        string? nomeContains = null) {
        return await _repo.GetPageAsync(page, pageSize, cancellationToken, sort, minPreco, maxPreco, minEstoque, nomeContains);
    }

    public async Task<Produto> AddAsync(PostProdutoDTO produtoDto, CancellationToken cancellationToken = default) {
        var produto = new Produto {
            Nome = produtoDto.Nome,
            Preco = produtoDto.Preco,
            Estoque = produtoDto.Estoque,
        };
        return await _repo.AddAsync(produto, cancellationToken);
    }

    
    
}