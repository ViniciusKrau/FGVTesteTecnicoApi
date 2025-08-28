using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PatchDTOs;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Entities.DTOs.Utils;
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
        var itensPedidos = await ConvertToItensPedido(postPedidoDTO.ProdutosQuantidades, cancellationToken);
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

    public async Task<Pedido> UpdatePedidoAsync(PatchPedidoDTO patchPedidoDto, CancellationToken cancellationToken) {
        Pedido pedido = await _pedidoRepository.GetByIdAsync(patchPedidoDto.CodPedido, cancellationToken) ?? throw new KeyNotFoundException("Pedido not found");

        pedido.CodCliente = patchPedidoDto.CodCliente != 0 ? patchPedidoDto.CodCliente : pedido.CodCliente;
        pedido.ValorTotal = patchPedidoDto.ValorTotal != 0 ? patchPedidoDto.ValorTotal : pedido.ValorTotal;
        pedido.DataPedido = patchPedidoDto.DataPedido != default ? patchPedidoDto.DataPedido : pedido.DataPedido;
        
        pedido = await _pedidoRepository.UpdateAsync(pedido, cancellationToken);
        return pedido;
    }

    private async Task<List<ItensPedido>> ConvertToItensPedido(List<ProdutoQuantidade> produtosQuantidades, CancellationToken cancellationToken) {
        var produtos = await _produtoService.GetByIdsAsync(produtosQuantidades.Select(p => p.CodProduto).ToList(), cancellationToken);
        var itensPedidos = produtos.Select(p => new ItensPedido {
            CodProduto = p.CodProduto,
            Quantidade = produtosQuantidades.First(pq => pq.CodProduto == p.CodProduto).Quantidade,
            PrecoUnitario = p.Preco,
        }).ToList();
        return itensPedidos;
    }
}
