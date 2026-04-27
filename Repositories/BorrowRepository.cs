using Libtech.Api.Models;
using Libtech.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace Libtech.Api.Repositories;

public class BorrowRepository : IBorrowRepository {
    private readonly LibtechContext _db;
    public BorrowRepository(LibtechContext db) => _db = db;
    public async Task<IEnumerable<BorrowRecord>> GetAllRecordsAsync() => await _db.BorrowRecords.ToListAsync();
    public async Task<IEnumerable<BorrowRecord>> GetMemberHistoryAsync(int mid) => 
        await _db.BorrowRecords.Where(r => r.MemberId == mid).ToListAsync();
    public async Task AddRecordAsync(BorrowRecord r) => await _db.BorrowRecords.AddAsync(r);
    public async Task<BorrowRecord?> GetActiveRecordAsync(int bid, int mid) =>
        await _db.BorrowRecords.FirstOrDefaultAsync(r => r.BookId == bid && r.MemberId == mid && r.Status == "Borrowed");
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}