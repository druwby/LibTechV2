using Libtech.Api.DTOs;
namespace Libtech.Api.Services;

public interface IMemberService {
    Task<IEnumerable<MemberResponseDTO>> GetAllMembersAsync();
    Task<MemberResponseDTO> CreateMemberAsync(MemberRequestDTO r);
}