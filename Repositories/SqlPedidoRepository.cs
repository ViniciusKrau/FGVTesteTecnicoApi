using Dapper;
using Microsoft.Data.SqlClient;
using TesteTecnicoApi.Entities;

namespace TesteTecnicoApi.Repositories.Interfaces;

public class SqlPedidoRepository(IConfiguration config) : IPedidoRepository {
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;
    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    public async Task<Pedido> AddAsync(Pedido pedido, CancellationToken cancellationToken) {
        const string sql = @"INSERT INTO Pedido (CodCliente, DataPedido, ValorTotal)
                                 VALUES (@CodCliente, @DataPedido, @ValorTotal);
                                 SELECT CAST(SCOPE_IDENTITY() as int);";
        await using var conn = CreateConnection();
        var id = await conn.QuerySingleAsync<int>(
            new CommandDefinition(sql, new {
                pedido.CodCliente,
                pedido.DataPedido,
                pedido.ValorTotal
            }, cancellationToken: cancellationToken));
        return new Pedido {
            CodCliente = pedido.CodCliente,
            DataPedido = pedido.DataPedido,
            ValorTotal = pedido.ValorTotal
        };
    }

    public async Task<IReadOnlyList<Pedido>> GetAllAsync(CancellationToken cancellationToken, int codCliente) {
        const string sql = @"SELECT * FROM Pedido
                                WHERE CodCliente = @CodCliente
                                ORDER BY DataPedido DESC;";
        await using var conn = CreateConnection();
        var pedidos = await conn.QueryAsync<Pedido>(
            new CommandDefinition(sql, new { CodCliente = codCliente }, cancellationToken: cancellationToken));
        return pedidos.AsList();
    }

    public async Task<Pedido> GetByIdAsync(int codPedido, CancellationToken cancellationToken) {
        const string sql = @"SELECT * FROM Pedido WHERE CodPedido = @CodPedido;";
        await using var conn = CreateConnection();
        var pedido = await conn.QuerySingleOrDefaultAsync<Pedido>(
            new CommandDefinition(sql, new { codPedido }, cancellationToken: cancellationToken));
        if (pedido == null) {
            throw new KeyNotFoundException($"Pedido with CodPedido {codPedido} not found.");
        }
        return pedido;
    }

    public async Task<Pedido> UpdateAsync(Pedido pedido, CancellationToken cancellationToken) {
        const string sql = @"UPDATE Pedido
                                 SET CodCliente = @CodCliente,
                                     DataPedido = @DataPedido,
                                     ValorTotal = @ValorTotal,
                                 WHERE CodPedido = @CodPedido;";
        await using var conn = CreateConnection();
        var affectedRows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new {
                pedido.CodCliente,
                pedido.DataPedido,
                pedido.ValorTotal,
                pedido.CodPedido
            }, cancellationToken: cancellationToken));
        if (affectedRows == 0) {
            throw new KeyNotFoundException($"Pedido with CodPedido {pedido.CodPedido} not found.");
        }
        return pedido;
    }
}
