using System.ComponentModel.DataAnnotations;

namespace TesteTecnicoApi.Entities.DTOs.PostDTOs;

public class PostCartDTO {

    public required List<int> ProdutoIds { get; set; } = [];

}
