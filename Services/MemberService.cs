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

    // Added: Get member by ID
    public async Task<MemberResponseDTO?> GetMemberByIdAsync(int id)
    {
        var member = await _repository.GetByIdAsync(id);
        if (member == null) return null;
        return new MemberResponseDTO 
        { 
            Id = member.Id, 
            FullName = member.FullName, 
            Email = member.Email, 
            MembershipDate = member.MembershipDate 
        };
    }

    // Added: Update member
    public async Task UpdateMemberAsync(int id, MemberRequestDTO request)
    {
        var member = await _repository.GetByIdAsync(id);
        if (member == null) throw new KeyNotFoundException();
        member.FullName = request.FullName;
        member.Email = request.Email;
        await _repository.UpdateAsync(member);
        await _repository.SaveChangesAsync();
    }

    // Added: Delete member
    public async Task DeleteMemberAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}