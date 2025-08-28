using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs;

public class PostClienteDTO {

    [Required]
    public required string CNPJ { get; set; }
    [Required]
    public required string Nome { get; set; }
    [Required]
    public required string Email { get; set; }
}
