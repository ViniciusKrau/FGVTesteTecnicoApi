using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Identity.Client;
using TesteTecnicoApi.Models;

namespace TesteTecnicoApi.Service;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SortableAttribute : Attribute {
    public string? ColumnName { get; }
    public SortableAttribute(string? columnName = null) => ColumnName = columnName;
}

public static class SqlSortNormalizer {

    public static string NormalizeSort(Type type, string sort) {
        switch (type) {
            case Type t when t == typeof(Produto):
                return NormalizeSortProduto(sort);
            case Type t when t == typeof(Cliente):
                return NormalizeSortCliente(sort);
            default:
                throw new ArgumentException("Unsupported type for sorting.");
        }
    }

    public static string NormalizeSort<T>(string sort) where T : BaseEntity {
        Type tipo = typeof(T);
        PropertyInfo[] properties = tipo.GetProperties();
        string[] propertiesNames = properties.Select(p => p.Name).ToArray();
        foreach (string propName in propertiesNames) {
            if (sort.Equals(propName, StringComparison.OrdinalIgnoreCase)) {
                return propName;
            }
        }
        return T.GetDefaultSort();
    }

    public static string NormalizeSortProduto(string? sort) {
        return (sort ?? "").Trim().ToLower() switch {
            "preco" => "Preco",
            "estoque" => "Estoque",
            "nome" => "Nome",
            _ => "CodProduto"
        };
    }
    private static string NormalizeSortCliente(string? sort) {
        return (sort ?? "").Trim().ToLower() switch {
            "nome" => "Nome",
            "cnpj" => "CNPJ",
            "email" => "Email",
            "datacadastro" => "DataCadastro",
            _ => "CodCliente"
        };
    }
}