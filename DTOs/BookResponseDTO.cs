namespace Libtech.Api.DTOs;
public class BookResponseDTO {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; } // Added
    public int AvailableCopies { get; set; }
}