using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs;

public class PostCartDTO {

    public required List<int> ProdutoIds { get; set; } = [];

}
