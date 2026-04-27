using System.ComponentModel.DataAnnotations;
namespace Libtech.Api.DTOs;

public class MemberResponseDTO {
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
}