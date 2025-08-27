using Microsoft.AspNetCore.Mvc;
using TesteTecnicoApi.Models;
using TesteTecnicoApi.Repositories;

namespace TesteTecnicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase {
    private readonly IProdutoRepository _repo;

    public ProdutoController(IProdutoRepository repo) {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetProdutos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default) {
        var result = await _repo.GetPageAsync(page, pageSize, ct);

        Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
        Response.Headers["X-Total-Pages"] = result.TotalPages.ToString();

        return Ok(new {
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages,
            Data = result.Items
        });
    }
}
