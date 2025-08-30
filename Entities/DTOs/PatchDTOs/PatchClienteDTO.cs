using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs.PatchDTOs;

public class PatchClienteDTO {
    [Required]
    public int CodCliente { get; set; }

    public string? CNPJ { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
}
