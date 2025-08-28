using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;
using TesteTecnicoApi.Repositories.Interfaces;

namespace TesteTecnicoApi.Service;

public class ItensPedidoService {
    private readonly IItensPedidosRepository _repo;

    public ItensPedidoService(IItensPedidosRepository itensPedidosRepository) {
        _repo = itensPedidosRepository;
    }

    public Task<ItensPedido> AddAsync(PostItensPedidoDto itemPedidoDto, CancellationToken cancellationToken) {
        var itemPedido = new ItensPedido {
            CodPedido = itemPedidoDto.CodPedido,
            Quantidade = itemPedidoDto.Quantidade,
            PrecoUnitario = itemPedidoDto.PrecoUnitario
        };
        return _repo.AddAsync(itemPedido, cancellationToken);
    }

    public Task<IReadOnlyList<ItensPedido>> GetAllAsync(CancellationToken cancellationToken) {
        return _repo.GetAllAsync(cancellationToken);
    }

    public Task<ItensPedido?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        return _repo.GetByIdAsync(id, cancellationToken);
    }

    public Task<PagedResult<ItensPedido>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
                                    string sort = "CodItemPedido",
                                    int? minQuantidade = null,
                                    decimal? minPrecoUnitario = null,
                                    decimal? maxPrecoUnitario = null) {
        return _repo.GetPageAsync(page, pageSize, cancellationToken, sort, minQuantidade, minPrecoUnitario, maxPrecoUnitario);
    }
}
