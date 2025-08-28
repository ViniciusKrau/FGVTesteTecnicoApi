
namespace TesteTecnicoApi.Entities.DTOs;
public class PostItensPedidoDto {
    public required int CodPedido { get; set; }
    public required int Quantidade { get; set; }
    public required decimal PrecoUnitario { get; set; }
}