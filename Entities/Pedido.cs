using System.Text.Json.Serialization;

namespace TesteTecnicoApi.Entities;

public class Pedido : BaseEntity {
    public int CodPedido { get; set; }
    public required int CodCliente { get; set; }
    public required decimal ValorTotal { get; set; }
    public required DateTimeOffset DataPedido { get; set; }
    [JsonIgnore]
    public Cliente Cliente { get; set; } = null!;
    public List<ItensPedido> Itens { get; set; } = [];

    public static string GetDefaultSort() {
        return "CodPedido";
    }
}