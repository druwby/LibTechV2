using Libtech.Api.DTOs;
namespace Libtech.Api.Services;

public interface IMemberService {
    Task<IEnumerable<MemberResponseDTO>> GetAllMembersAsync();
    Task<MemberResponseDTO?> GetMemberByIdAsync(int id); // Added
    Task<MemberResponseDTO> CreateMemberAsync(MemberRequestDTO r);
    Task UpdateMemberAsync(int id, MemberRequestDTO r); // Added
    Task DeleteMemberAsync(int id); // Added
}