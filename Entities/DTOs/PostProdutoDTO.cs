using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs;

public class PostProdutoDTO {

    [Required]
    public string Nome { get; set; } = null!;

    [Required]
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}