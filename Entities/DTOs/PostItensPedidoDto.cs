
using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs;

public class PostItensPedidoDto {

    [Required]
    public int CodPedido { get; set; }
    [Required]
    public int Quantidade { get; set; }
    [Required]
    public decimal PrecoUnitario { get; set; }
}