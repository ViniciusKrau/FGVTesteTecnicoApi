using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs;

public class PostPedidoDTO {

    [Required]
    public int CodCliente { get; set; }

    [Required]
    public decimal ValorTotal { get; set; }

    public List<int> Itens { get; set; } = [];
}
