using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Repositories.Interfaces;

namespace TesteTecnicoApi.Service;

public class PedidoService {

    private readonly IPedidoRepository _pedidoRepository;
    private readonly ProdutoService _produtoService;
    public PedidoService(IPedidoRepository pedidoRepository, ProdutoService produtoService) {
        _pedidoRepository = pedidoRepository;
        _produtoService = produtoService;
    }


    public async Task<Pedido> CreatePedidoAsync(PostPedidoDTO postPedidoDTO, CancellationToken cancellationToken) {
        var pedido = new Pedido {
            CodCliente = postPedidoDTO.CodCliente,
            ValorTotal = postPedidoDTO.ValorTotal,
            DataPedido = DateTimeOffset.UtcNow,
            Itens = []
        };
        var itensPedidos = await ConvertToItensPedido(postPedidoDTO.Produtos, cancellationToken);
        pedido.Itens = itensPedidos;
        var createdPedido = await _pedidoRepository.AddAsync(pedido, cancellationToken);
        return createdPedido;
    }

    public async Task<IReadOnlyList<Pedido>> GetAllPedidosAsync(int codCliente, CancellationToken cancellationToken) {
        return await _pedidoRepository.GetAllAsync(cancellationToken, codCliente);
    }

    public async Task<Pedido> GetPedidoByIdAsync(int codPedido, CancellationToken cancellationToken) {
        return await _pedidoRepository.GetByIdAsync(codPedido, cancellationToken);
    }

    public async Task<Pedido> UpdatePedidoAsync(Pedido pedido, CancellationToken cancellationToken) {
        return await _pedidoRepository.UpdateAsync(pedido, cancellationToken);
    }

    private async Task<List<ItensPedido>> ConvertToItensPedido(List<int> produtoIds, CancellationToken cancellationToken) {
        var produtos = await _produtoService.GetByIdsAsync(produtoIds, cancellationToken);
        var itensPedidos = produtos.Select(p => new ItensPedido {
            CodProduto = p.CodProduto,
            Quantidade = p.Estoque,
            PrecoUnitario = p.Preco,
        }).ToList();
        return itensPedidos;
    }
}
