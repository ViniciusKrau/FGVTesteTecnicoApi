using System.Data;
using Microsoft.Data.SqlClient;
using TesteTecnicoApi.Entities;

namespace TesteTecnicoApi.Service;

public static class SqlPaginationExtensions {
    public static async Task<PagedResult<T>> QueryPagedAsync<T>(
        this SqlConnection conn,
        string baseSelectSql,
        string orderByClause,
        int page,
        int pageSize,
        Func<SqlDataReader, T> map,
        CancellationToken ct) {

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var countSql = $"SELECT COUNT(*) FROM ({baseSelectSql}) AS CountScope";
        await using var countCmd = new SqlCommand(countSql, conn);
        var total = (int)await countCmd.ExecuteScalarAsync(ct);

        if (total == 0)
            return new PagedResult<T>(Array.Empty<T>(), page, pageSize, 0);

        var offset = (page - 1) * pageSize;

        var pageSql = $"""
                        {baseSelectSql}
                        ORDER BY {orderByClause}
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                        """;

        var items = new List<T>();
        await using var pageCmd = new SqlCommand(pageSql, conn);
        pageCmd.Parameters.Add("@Offset", SqlDbType.Int).Value = offset;
        pageCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

        await using var reader = await pageCmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct)) {
            items.Add(map(reader));
        }

        return new PagedResult<T>(items, page, pageSize, total);
    }
}