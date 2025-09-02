using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace TesteTecnicoApi.Repositories;

public class SqlItensPedidosRepository(IConfiguration config) : IItensPedidosRepository {

    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;
    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    public Task<ItensPedido> AddAsync(ItensPedido itemPedido, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ItensPedido>> GetAllAsync(CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<ItensPedido?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ItensPedido>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken,
                                    string sort = "CodItemPedido",
                                    int? minQuantidade = null,
                                    decimal? minPrecoUnitario = null,
                                    decimal? maxPrecoUnitario = null) {
        throw new NotImplementedException();
    }

    public async Task<List<ItensPedido>> GetByCodProdutoAsync(int codProduto, CancellationToken cancellationToken) {
        const string sql = @"SELECT CodItemPedido, CodProduto, Nome, PrecoUnitario, Quantidade
                             FROM ItensPedido
                             WHERE CodProduto = @codProduto";
        await using var conn = CreateConnection();
        var itens = await conn.QueryAsync<ItensPedido>(
            new CommandDefinition(sql, new { codProduto }, cancellationToken: cancellationToken));
        return itens.AsList();
    }
}
