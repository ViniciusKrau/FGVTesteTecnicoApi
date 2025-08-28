using Microsoft.AspNetCore.Mvc;
using TesteTecnicoApi.Models;
using TesteTecnicoApi.Repositories;
using TesteTecnicoApi.DTOs;
using TesteTecnicoApi.Service;

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
        CancellationToken cancellationToken = default) {
        var result = await _repo.GetPageAsync(page, pageSize, cancellationToken);

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Produto>> GetProdutoById(int id, CancellationToken cancellationToken = default) {
        var produto = await _repo.GetByIdAsync(id, cancellationToken);
        if (produto == null) {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<Produto>>> GetAllProdutos(CancellationToken cancellationToken = default) {
        var produtos = await _repo.GetAllAsync(cancellationToken);
        return Ok(produtos);
    }

    [HttpGet("sorted")]
    public async Task<ActionResult<object>> GetProdutosSorted(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "CodProduto",
        [FromQuery] decimal? minPreco = null,
        [FromQuery] decimal? maxPreco = null,
        [FromQuery] int? minEstoque = null,
        [FromQuery] string? nomeContains = null,
        CancellationToken cancellationToken = default) {
        var result = await _repo.GetPageAsync(
            page,
            pageSize,
            cancellationToken,
            sort,
            minPreco,
            maxPreco,
            minEstoque,
            nomeContains
        );

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
    [HttpPost("create")]
    public async Task<ActionResult<Produto>> CreateProduto([FromBody] PostProdutoDTO produtoDto, CancellationToken cancellationToken = default) {
        if (produtoDto == null) {
            return BadRequest("Produto cannot be null.");
        }
        SqlSortNormalizer.NormalizeSort<Produto>("");
        var createdProduto = await _repo.AddAsync(produtoDto, cancellationToken);
        return CreatedAtAction(nameof(GetProdutoById), new { id = createdProduto.CodProduto }, createdProduto);
    }

}
