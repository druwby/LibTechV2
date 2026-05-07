using Libtech.Api.DTOs;
namespace Libtech.Api.Services;

public interface IBorrowService {
    Task<BorrowResponseDTO> BorrowBookAsync(BorrowRequestDTO r);
    Task ReturnBookAsync(int bid, int mid);
    Task<IEnumerable<BorrowResponseDTO>> GetAllBorrowRecordsAsync(); // Added
    Task<IEnumerable<BorrowResponseDTO>> GetMemberBorrowHistoryAsync(int memberId); // Added
}