using Libtech.Api.DTOs;
namespace Libtech.Api.Services;

public interface IBookService {
    Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync();
    Task<BookResponseDTO?> GetBookByIdAsync(int id);
    Task<BookResponseDTO> CreateBookAsync(BookRequestDTO r);
}