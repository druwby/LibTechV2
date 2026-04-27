using Microsoft.AspNetCore.Mvc;
using Libtech.Api.DTOs;
using Libtech.Api.Services;
namespace Libtech.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService) => _bookService = bookService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetBooks()
    {
        return Ok(await _bookService.GetAllBooksAsync());
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> CreateBook([FromBody] BookRequestDTO input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _bookService.CreateBookAsync(input);
        return CreatedAtAction(nameof(GetBooks), new { id = result.Id }, result);
    }
}