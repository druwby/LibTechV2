using System.ComponentModel.DataAnnotations;
namespace Libtech.Api.DTOs;

public class BookRequestDTO {
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Author { get; set; } = string.Empty;
    [Required] public string ISBN { get; set; } = string.Empty;
    [Range(1, int.MaxValue)] public int TotalCopies { get; set; }
}