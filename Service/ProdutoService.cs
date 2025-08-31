using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Repositories;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Repositories.Interfaces;
using TesteTecnicoApi.Entities.DTOs.Utils;

namespace TesteTecnicoApi.Service;

public class ProdutoService {

    private readonly IProdutoRepository _produtoRepo;

    private readonly IItensPedidosRepository _itensPedidosRepository;
    public ProdutoService(IProdutoRepository produtoRepo, IItensPedidosRepository itensPedidosRepository) {
        _produtoRepo = produtoRepo;
        _itensPedidosRepository = itensPedidosRepository;
    }

    public async Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        return await _produtoRepo.GetPageAsync(page, pageSize, cancellationToken);
    }
    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
        return await _produtoRepo.GetByIdAsync(id, cancellationToken);
    }

    public async Task<List<Produto>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
        return await _produtoRepo.GetByIdsAsync(ids, cancellationToken);
    }
    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _produtoRepo.GetAllAsync(cancellationToken);
    }
    public async Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default,
        string sort = "CodProduto",
        decimal? minPreco = null,
        decimal? maxPreco = null,
        int? minEstoque = null,
        string? nomeContains = null) {
        return await _produtoRepo.GetPageAsync(page, pageSize, cancellationToken, sort, minPreco, maxPreco, minEstoque, nomeContains);
    }

    public async Task<Produto> AddAsync(PostProdutoDTO produtoDto, CancellationToken cancellationToken = default) {
        var produto = new Produto {
            Nome = produtoDto.Nome,
            Preco = produtoDto.Preco,
            Estoque = produtoDto.Estoque,
        };
        return await _produtoRepo.AddAsync(produto, cancellationToken);
    }

    public async Task DeleteAsync(int codProduto, CancellationToken cancellationToken) {
        var itensPedidos = await _itensPedidosRepository.GetByCodProdutoAsync(codProduto, cancellationToken);
        if (itensPedidos.Any()) {
            throw new InvalidOperationException("Cannot delete Produto because it is referenced by ItensPedido.");
        }
        await _produtoRepo.DeleteAsync(codProduto, cancellationToken);
    }

    public async Task<Produto> UpdateAsync(PatchProdutoDTO patchProdutoDto, CancellationToken cancellationToken) {
        Produto produto = await _produtoRepo.GetByIdAsync(patchProdutoDto.CodProduto, cancellationToken) ?? throw new KeyNotFoundException("Produto not found");
        produto.Nome = !string.IsNullOrWhiteSpace(patchProdutoDto.Nome) ? patchProdutoDto.Nome : produto.Nome;
        produto.Preco = patchProdutoDto.Preco.HasValue ? patchProdutoDto.Preco.Value : produto.Preco;
        produto.Estoque = patchProdutoDto.Estoque.HasValue ? patchProdutoDto.Estoque.Value : produto.Estoque;

        return await _produtoRepo.UpdateAsync(produto, cancellationToken);
    }

    public async Task<decimal> GetSumOfProdutos(List<ProdutoQuantidade> produtosQuantidades, CancellationToken cancellationToken) {
        var produtos = await GetByIdsAsync(produtosQuantidades.Select(p => p.CodProduto).ToList(), cancellationToken);
        decimal total = 0;
        foreach (var pq in produtosQuantidades) {
            var produto = produtos.FirstOrDefault(p => p.CodProduto == pq.CodProduto);
            if (produto != null) {
                total += produto.Preco * pq.Quantidade;
            }
        }
        return total;
    }
}