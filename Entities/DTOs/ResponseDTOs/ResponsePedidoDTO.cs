using System.Text.Json.Serialization;

namespace TesteTecnicoApi.Entities;

public class ResponsePedidoDTO {
    public int CodPedido { get; set; }
    public int CodCliente { get; set; }

    public decimal ValorTotal { get; set; }
    public DateTimeOffset DataPedido { get; set; }
    public string Nome { get; set; } = null!;
    public string CNPJ { get; set; } = null!;
    
}