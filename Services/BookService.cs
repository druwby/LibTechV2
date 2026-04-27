using Libtech.Api.DTOs;
using Libtech.Api.Models;
using Libtech.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Libtech.Api.Services;

public class BookService : IBookService {
    private readonly IBookRepository _repo;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY = "Books_All";

    public BookService(IBookRepository repo, IMemoryCache cache) {
        _repo = repo;
        _cache = cache;
    }

    public async Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync() {
        if (!_cache.TryGetValue(CACHE_KEY, out IEnumerable<BookResponseDTO>? books)) {
            var entities = await _repo.GetAllAsync();
            books = entities.Select(MapToDTO);
            _cache.Set(CACHE_KEY, books, TimeSpan.FromMinutes(10));
        }
        return books!;
    }

    public async Task<BookResponseDTO?> GetBookByIdAsync(int id) {
        var book = await _repo.GetByIdAsync(id);
        return book == null ? null : MapToDTO(book);
    }

    public async Task<BookResponseDTO> CreateBookAsync(BookRequestDTO req) {
        var book = new Book { Title = req.Title, Author = req.Author, ISBN = req.ISBN, TotalCopies = req.TotalCopies, AvailableCopies = req.TotalCopies };
        await _repo.AddAsync(book);
        await _repo.SaveChangesAsync();
        _cache.Remove(CACHE_KEY);
        return MapToDTO(book);
    }

    public async Task UpdateBookAsync(int id, BookRequestDTO req) {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) throw new KeyNotFoundException();
        book.Title = req.Title;
        await _repo.UpdateAsync(book);
        await _repo.SaveChangesAsync();
        _cache.Remove(CACHE_KEY);
    }

    public async Task DeleteBookAsync(int id) {
        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
        _cache.Remove(CACHE_KEY);
    }

    private BookResponseDTO MapToDTO(Book b) => new() { Id = b.Id, Title = b.Title, Author = b.Author, ISBN = b.ISBN, AvailableCopies = b.AvailableCopies };
}