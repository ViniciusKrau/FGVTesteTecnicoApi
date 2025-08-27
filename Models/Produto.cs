using System.Text.Json.Serialization;

namespace TesteTecnicoApi.Models;

public class Produto {
    public int CodProduto { get; set; }
    public required string Nome { get; set; }
    public required decimal Preco { get; set; }
    public required int Estoque { get; set; }

    [JsonIgnore]
    public List<ItensPedido>? ItensPedidos { get; set; } = new();
}