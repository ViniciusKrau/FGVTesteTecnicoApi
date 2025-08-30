using Microsoft.Data.SqlClient;
using Dapper;
using TesteTecnicoApi.Entities;
using System.Data;
using TesteTecnicoApi.Service;

namespace TesteTecnicoApi.Repositories;

public class SqlClienteRepository(IConfiguration config) : IClienteRepository {
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken cancellationToken) {
        const string sql = @"SELECT CodCliente, CNPJ, Nome, Email, DataCadastro
                             FROM Cliente";
        await using var conn = CreateConnection();
        var rows = await conn.QueryAsync<Cliente>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken) {
        const string sql = @"SELECT CodCliente, CNPJ, Nome, Email, DataCadastro
                             FROM Cliente
                             WHERE CodCliente = @id";
        await using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Cliente>(
            new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken));
    }

    public async Task<Cliente?> GetByNameAsync(string name, CancellationToken cancellationToken) {
        const string sql = @"SELECT CodCliente, CNPJ, Nome, Email, DataCadastro
                             FROM Cliente
                             WHERE Nome = @name";
        await using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Cliente>(
            new CommandDefinition(sql, new { name }, cancellationToken: cancellationToken));
    }

    public async Task<PagedResult<Cliente>> GetPageAsync(
                            int page,
                            int pageSize,
                            CancellationToken cancellationToken,
                            string sort = "CodCliente",
                            string? nomeContains = null,
                            string? cnpjContains = null,
                            string? emailContains = null,
                            DateTime? minDataCadastro = null,
                            DateTime? maxDataCadastro = null
                            ) {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (minDataCadastro.HasValue && maxDataCadastro.HasValue && minDataCadastro > maxDataCadastro)
            throw new ArgumentException("minDataCadastro cannot be greater than maxDataCadastro.");
        var orderBy = SqlSortNormalizer.NormalizeSort(typeof(Cliente), sort);
        var conditions = new List<string>();
        var parameters = new DynamicParameters();
        if (!string.IsNullOrWhiteSpace(nomeContains)) {
            conditions.Add("Nome LIKE @nomeContains");
            parameters.Add("nomeContains", $"%{nomeContains}%");
        }
        const string sql = @"SELECT CodCliente, CNPJ, Nome, Email, DataCadastro
                             FROM Cliente
                             ORDER BY CodCliente
                             OFFSET @offset ROWS
                             FETCH NEXT @pageSize ROWS ONLY;
                             SELECT COUNT(*) FROM Cliente;";
        var offset = (page - 1) * pageSize;
        await using var conn = CreateConnection();
        var multi = await conn.QueryMultipleAsync(
            new CommandDefinition(sql, new { offset, pageSize }, cancellationToken: cancellationToken));
        var items = (await multi.ReadAsync<Cliente>()).AsList();
        var totalItems = await multi.ReadFirstAsync<int>();
        return new PagedResult<Cliente>(items, totalItems, page, pageSize);
    }

    public async Task<Cliente> AddAsync(Cliente cliente, CancellationToken cancellationToken) {
        const string sql = @"INSERT INTO Cliente (CNPJ, Nome, Email, DataCadastro)
                             VALUES (@CNPJ, @Nome, @Email, @DataCadastro);
                             SELECT CAST(SCOPE_IDENTITY() as int);";
        await using var conn = CreateConnection();
        var id = await conn.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, cliente, cancellationToken: cancellationToken));
        cliente.CodCliente = id;
        return cliente;
    }

    public async Task<Cliente> UpdateAsync(Cliente cliente, CancellationToken cancellationToken) {
        const string sql = @"UPDATE Cliente
                             SET CNPJ = @CNPJ,
                                 Nome = @Nome,
                                 Email = @Email
                             WHERE CodCliente = @CodCliente;
                             SELECT CodCliente, CNPJ, Nome, Email, DataCadastro
                             FROM Cliente
                             WHERE CodCliente = @CodCliente;";
        await using var conn = CreateConnection();
        var updated = await conn.QuerySingleAsync<Cliente>(
            new CommandDefinition(sql, cliente, cancellationToken: cancellationToken));
        return updated;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken) {
        const string sql = @"DELETE FROM Cliente WHERE CodCliente = @id;";
        await using var conn = CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken));
    }
}