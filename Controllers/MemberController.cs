using Microsoft.AspNetCore.Mvc;
using Libtech.Api.DTOs;
using Libtech.Api.Services;

namespace Libtech.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetMembers()
    {
        var members = await _memberService.GetAllMembersAsync();
        return Ok(members);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDTO>> CreateMember([FromBody] MemberRequestDTO input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _memberService.CreateMemberAsync(input);
        // Updated: routes to GetMemberById now that the endpoint exists
        return CreatedAtAction(nameof(GetMemberById), new { id = created.Id }, created);
    }

    // Added: GET by ID endpoint
    [HttpGet("{id}")]
    public async Task<ActionResult<MemberResponseDTO>> GetMemberById(int id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member == null) return NotFound();
        return Ok(member);
    }

    // Added: PUT endpoint to update an existing member
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberRequestDTO input)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try {
            await _memberService.UpdateMemberAsync(id, input);
            return NoContent();
        } catch (KeyNotFoundException) {
            return NotFound();
        }
    }

    // Added: DELETE endpoint to remove a member by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try {
            await _memberService.DeleteMemberAsync(id);
            return NoContent();
        } catch (KeyNotFoundException) {
            return NotFound();
        }
    }
}