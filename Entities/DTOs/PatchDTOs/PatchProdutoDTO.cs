using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs.PatchDTOs;

public class PatchProdutoDTO {
    [Required]
    public int CodProduto { get; set; }

    public string? Nome { get; set; }
    public decimal? Preco { get; set; }
    public int? Estoque { get; set; }
}
