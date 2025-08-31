using System.ComponentModel.DataAnnotations;
using TesteTecnicoApi.Entities.DTOs.Utils;

namespace TesteTecnicoApi.Entities.DTOs.PatchDTOs;

public class PatchPedidoDTO{

    [Required]
    public int CodPedido { get; set; }

    public int CodCliente { get; set; }

    public DateTimeOffset DataPedido { get; set; }

    public List<ProdutoQuantidade> ProdutosQuantidades { get; set; } = [];

}
