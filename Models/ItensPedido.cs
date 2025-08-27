using System.Text.Json.Serialization;

namespace TesteTecnicoApi.Models;

public class ItensPedido {
    public int CodItemPedido { get; set; }
    public required int CodPedido { get; set; }
    public required int Quantidade { get; set; }
    public required decimal PrecoUnitario { get; set; }

    [JsonIgnore]
    public Pedido Pedido { get; set; } = null!;
    public Produto? Produto { get; set; }

}