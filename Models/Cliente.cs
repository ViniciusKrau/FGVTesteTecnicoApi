namespace TesteTecnicoApi.Models;

public class Cliente : BaseModel {
    public int CodCliente { get; set; }
    public required string CNPJ { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required DateTimeOffset DataCadastro { get; set; }
    public List<Pedido> Pedidos { get; set; } = new();
    public string GetDefaultSort() {
        return "CodCliente";
    }
}