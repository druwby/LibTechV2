using Libtech.Api.DTOs;
using Libtech.Api.Models;
using Libtech.Api.Repositories;
namespace Libtech.Api.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository) => _repository = repository;

    public async Task<IEnumerable<MemberResponseDTO>> GetAllMembersAsync()
    {
        var members = await _repository.GetAllAsync();
        return members.Select(m => new MemberResponseDTO 
        { 
            Id = m.Id, 
            FullName = m.FullName, 
            Email = m.Email, 
            MembershipDate = m.MembershipDate 
        });
    }

    public async Task<MemberResponseDTO> CreateMemberAsync(MemberRequestDTO request)
    {
        var member = new Member { FullName = request.FullName, Email = request.Email };
        await _repository.AddAsync(member);
        await _repository.SaveChangesAsync();
        return new MemberResponseDTO { Id = member.Id, FullName = member.FullName };
    }
}