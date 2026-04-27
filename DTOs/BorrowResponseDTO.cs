namespace Libtech.Api.DTOs;

public class BorrowResponseDTO {
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public string Status { get; set; } = string.Empty;
}