using Microsoft.AspNetCore.Mvc;
using Libtech.Api.DTOs;
using Libtech.Api.Services;

namespace Libtech.Api.Controllers;

[ApiController]
[Route("api/borrowing")]
public class BorrowController : ControllerBase {
    private readonly IBorrowService _service;
    public BorrowController(IBorrowService service) => _service = service;

    [HttpPost("borrow")]
    public async Task<IActionResult> Borrow([FromBody] BorrowRequestDTO req) {
        try {
            var result = await _service.BorrowBookAsync(req);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { error = ex.Message });
        }
    }

    // Added: POST endpoint to return a book
    [HttpPost("return")]
    public async Task<IActionResult> Return([FromBody] BorrowRequestDTO req) {
        try {
            await _service.ReturnBookAsync(req.BookId, req.MemberId);
            return Ok(new { message = "Book returned successfully." });
        } catch (Exception ex) {
            return BadRequest(new { error = ex.Message });
        }
    }

    // Added: GET endpoint to view all borrow records
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BorrowResponseDTO>>> GetAllBorrowRecords() {
        var records = await _service.GetAllBorrowRecordsAsync();
        return Ok(records);
    }

    // Added: GET endpoint to view borrow history for a specific member
    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<BorrowResponseDTO>>> GetMemberHistory(int memberId) {
        var records = await _service.GetMemberBorrowHistoryAsync(memberId);
        return Ok(records);
    }
}