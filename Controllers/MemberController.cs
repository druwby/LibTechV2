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
        return CreatedAtAction(nameof(GetMembers), new { id = created.Id }, created);
    }
}