namespace TesteTecnicoApi.DTOs;

public class PostProdutoDTO {
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}