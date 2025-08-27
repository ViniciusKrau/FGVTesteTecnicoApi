using Microsoft.Data.SqlClient;
using TesteTecnicoApi.Models;
using TesteTecnicoApi.Service;

namespace TesteTecnicoApi.Repositories;

public class SqlProdutoRepository(IConfiguration config) : IProdutoRepository {
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken cancellationToken) {
        var list = new List<Produto>();
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        const string sql = "SELECT CodProduto, Nome, Preco, Estoque FROM Produto";
        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken)) {
            list.Add(new Produto {
                CodProduto = reader.GetInt32(reader.GetOrdinal("CodProduto")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                Estoque = reader.GetInt32(reader.GetOrdinal("Estoque"))
            });
        }

        return list;
    }

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        const string sql = "SELECT CodProduto, Nome, Preco, Estoque FROM Produto WHERE CodProduto = @Id";
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken)) {
            return new Produto {
                CodProduto = reader.GetInt32(reader.GetOrdinal("CodProduto")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                Estoque = reader.GetInt32(reader.GetOrdinal("Estoque"))
            };
        }
        return null;
    }

    public async Task<PagedResult<Produto>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken) {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        const string baseSql = "SELECT CodProduto, Nome, Preco, Estoque FROM Produto";
        return await conn.QueryPagedAsync(
            baseSql,
            "CodProduto",
            page,
            pageSize,
            r => new Produto {
                CodProduto = r.GetInt32(r.GetOrdinal("CodProduto")),
                Nome = r.GetString(r.GetOrdinal("Nome")),
                Preco = r.GetDecimal(r.GetOrdinal("Preco")),
                Estoque = r.GetInt32(r.GetOrdinal("Estoque"))
            },
            cancellationToken);
    }

}