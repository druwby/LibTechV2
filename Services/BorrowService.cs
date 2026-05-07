using Libtech.Api.DTOs;
using Libtech.Api.Models;
using Libtech.Api.Repositories;

namespace Libtech.Api.Services;

public class BorrowService : IBorrowService {
    private readonly IBorrowRepository _borrowRepo;
    private readonly IBookRepository _bookRepo;

    public BorrowService(IBorrowRepository br, IBookRepository bkr) {
        _borrowRepo = br; _bookRepo = bkr;
    }

    public async Task<BorrowResponseDTO> BorrowBookAsync(BorrowRequestDTO req) {
        var book = await _bookRepo.GetByIdAsync(req.BookId);
        
        // Check if last copy is gone
        if (book == null || book.AvailableCopies <= 0) 
            throw new InvalidOperationException("Book is unavailable.");

        // Added: Prevent duplicate borrowing without returning
        var existingBorrow = await _borrowRepo.GetActiveRecordAsync(req.BookId, req.MemberId);
        if (existingBorrow != null)
            throw new InvalidOperationException("Member has already borrowed this book and has not returned it.");

        book.AvailableCopies--;
        var record = new BorrowRecord { BookId = req.BookId, MemberId = req.MemberId, Status = "Borrowed" };
        
        await _borrowRepo.AddRecordAsync(record);
        await _bookRepo.SaveChangesAsync();
        
        return new BorrowResponseDTO { Id = record.Id, BookId = book.Id, MemberId = req.MemberId, Status = record.Status, BorrowDate = record.BorrowDate };
    }

    public async Task ReturnBookAsync(int bookId, int memberId) {
        var record = await _borrowRepo.GetActiveRecordAsync(bookId, memberId);
        if (record == null) throw new InvalidOperationException("No active borrow records found.");

        var book = await _bookRepo.GetByIdAsync(bookId);
        if (book != null) book.AvailableCopies++;

        record.Status = "Returned";
        record.ReturnDate = DateTime.UtcNow;
        await _bookRepo.SaveChangesAsync();
    }

    // Added: Get all borrow records
    public async Task<IEnumerable<BorrowResponseDTO>> GetAllBorrowRecordsAsync() {
        var records = await _borrowRepo.GetAllRecordsAsync();
        return records.Select(r => new BorrowResponseDTO {
            Id = r.Id,
            BookId = r.BookId,
            MemberId = r.MemberId,
            BorrowDate = r.BorrowDate,
            Status = r.Status
        });
    }

    // Added: Get borrow history for a specific member
    public async Task<IEnumerable<BorrowResponseDTO>> GetMemberBorrowHistoryAsync(int memberId) {
        var records = await _borrowRepo.GetMemberHistoryAsync(memberId);
        return records.Select(r => new BorrowResponseDTO {
            Id = r.Id,
            BookId = r.BookId,
            MemberId = r.MemberId,
            BorrowDate = r.BorrowDate,
            Status = r.Status
        });
    }
}