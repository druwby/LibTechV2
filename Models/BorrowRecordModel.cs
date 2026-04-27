namespace Libtech.Api.Models;
public class BorrowRecord {
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = "Borrowed";
}