using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs.PostDTOs;

public class PostPedidoDTO {

    [Required]
    public int CodCliente { get; set; }

    [Required]
    public decimal ValorTotal { get; set; }

    public List<int> Produtos { get; set; } = [];
}
