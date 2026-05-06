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

    // Added: GET by ID endpoint
    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDTO>> GetBookById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> CreateBook([FromBody] BookRequestDTO input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _bookService.CreateBookAsync(input);
        // Updated: routes to GetBookById now that the endpoint exists
        return CreatedAtAction(nameof(GetBookById), new { id = result.Id }, result);
    }

    // Added: PUT endpoint to update an existing book
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDTO input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try {
            await _bookService.UpdateBookAsync(id, input);
            return NoContent();
        } catch (KeyNotFoundException) {
            return NotFound();
        }
    }

    // Added: DELETE endpoint to remove a book by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        } catch (KeyNotFoundException) {
            return NotFound();
        }
    }
}
