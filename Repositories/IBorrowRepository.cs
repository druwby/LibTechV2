using Libtech.Api.Models;
namespace Libtech.Api.Repositories;

public interface IBorrowRepository {
    Task<IEnumerable<BorrowRecord>> GetAllRecordsAsync();
    Task<IEnumerable<BorrowRecord>> GetMemberHistoryAsync(int memberId);
    Task AddRecordAsync(BorrowRecord record);
    Task<BorrowRecord?> GetActiveRecordAsync(int bookId, int memberId);
    Task SaveChangesAsync();
}