
using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs.PostDTOs;

public class PostItensPedidoDto {

    [Required]
    public int CodProduto { get; set; }
    [Required]
    public int CodPedido { get; set; }
    [Required]
    public int Quantidade { get; set; }
    [Required]
    public decimal PrecoUnitario { get; set; }
}