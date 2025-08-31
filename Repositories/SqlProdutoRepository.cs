using Microsoft.Data.SqlClient;
using Dapper;
using TesteTecnicoApi.Entities;
using TesteTecnicoApi.Service;
using TesteTecnicoApi.Entities.DTOs.PostDTOs;

namespace TesteTecnicoApi.Repositories;

public class SqlProdutoRepository(IConfiguration config) : IProdutoRepository {
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    public async Task<IReadOnlyList<Produto>> GetAllAsync(CancellationToken cancellationToken) {
        const string sql = "SELECT CodProduto, Nome, Preco, Estoque FROM Produto";
        await using var conn = CreateConnection();
        var rows = await conn.QueryAsync<Produto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));
        return rows.AsList();
    }
    public async Task<List<Produto>> GetByIdsAsync(IEnumerable<int> codProdutos, CancellationToken cancellationToken) {
        const string sql = @"SELECT CodProduto, Nome, Preco, Estoque
                             FROM Produto
                             WHERE CodProduto IN @codProdutos";
        await using var conn = CreateConnection();
        var produtos = await conn.QueryAsync<Produto>(
            new CommandDefinition(sql, new { codProdutos }, cancellationToken: cancellationToken));
        return produtos.AsList();
    }

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        const string sql = @"SELECT CodProduto, Nome, Preco, Estoque
                             FROM Produto
                             WHERE CodProduto = @id";
        await using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Produto>(
            new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken));
    }

    public async Task<PagedResult<Produto>> GetPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken,
        string sort = "CodProduto",
        decimal? minPreco = null,
        decimal? maxPreco = null,
        int? minEstoque = null,
        string? nomeContains = null) {

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (minPreco.HasValue && maxPreco.HasValue && minPreco > maxPreco)
            throw new ArgumentException("minPreco cannot be greater than maxPreco.");

        await using var conn = CreateConnection();
        var orderBy = SqlSortNormalizer.NormalizeSort(typeof(Produto), sort);

        var conditions = new List<string>();
        var parameters = new DynamicParameters();

        if (minPreco.HasValue) {
            conditions.Add("Preco >= @minPreco");
            parameters.Add("minPreco", minPreco.Value);
        }
        if (maxPreco.HasValue) {
            conditions.Add("Preco <= @maxPreco");
            parameters.Add("maxPreco", maxPreco.Value);
        }
        if (minEstoque.HasValue) {
            conditions.Add("Estoque >= @minEstoque");
            parameters.Add("minEstoque", minEstoque.Value);
        }
        if (!string.IsNullOrWhiteSpace(nomeContains)) {
            conditions.Add("Nome LIKE @nomeLike");
            parameters.Add("nomeLike", $"%{nomeContains}%");
        }

        var where = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

        var sql = $@"
            SELECT CodProduto, Nome, Preco, Estoque
            FROM Produto
            {where}
            ORDER BY {orderBy}
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

            SELECT COUNT(*) FROM Produto
            {where};";

        var offset = (page - 1) * pageSize;
        parameters.Add("offset", offset);
        parameters.Add("pageSize", pageSize);

        var multi = await conn.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<Produto>()).AsList();
        var totalItems = await multi.ReadFirstAsync<int>();
        return new PagedResult<Produto>(items, page, pageSize, totalItems);
    }
    public async Task<Produto> AddAsync(Produto produto, CancellationToken cancellationToken) {
        const string sql = @"
            INSERT INTO Produto (Nome, Preco, Estoque)
            OUTPUT INSERTED.CodProduto, INSERTED.Nome, INSERTED.Preco, INSERTED.Estoque
            VALUES (@Nome, @Preco, @Estoque);";

        await using var conn = CreateConnection();
        return await conn.QuerySingleAsync<Produto>(
            new CommandDefinition(sql, produto, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken) {
        const string sql = @"DELETE FROM Produto WHERE CodProduto = @id;";
        await using var conn = CreateConnection();
        var affectedRows = await conn.ExecuteAsync(
            new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken));
        if (affectedRows == 0) {
            throw new KeyNotFoundException($"Produto with CodProduto {id} not found.");
        }
    }

    public async Task<Produto> UpdateAsync(Produto produto, CancellationToken cancellationToken) {
        const string sql = @"UPDATE Produto
                             SET Nome = @Nome,
                                 Preco = @Preco,
                                 Estoque = @Estoque
                             WHERE CodProduto = @CodProduto;";
        await using var conn = CreateConnection();
        var affectedRows = await conn.ExecuteAsync(
            new CommandDefinition(sql, produto, cancellationToken: cancellationToken));
        if (affectedRows == 0) {
            throw new KeyNotFoundException($"Produto with CodProduto {produto.CodProduto} not found.");
        }
        return produto;
    }
}