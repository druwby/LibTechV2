using System.ComponentModel.DataAnnotations;
namespace Libtech.Api.DTOs;

public class MemberRequestDTO {
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
}