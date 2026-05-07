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
        // Added: Check for duplicate ISBN before creating
        var existingBook = (await _repo.GetAllAsync()).FirstOrDefault(b => b.ISBN == req.ISBN);
        if (existingBook != null)
            throw new InvalidOperationException($"A book with ISBN '{req.ISBN}' already exists.");

        // Updated: AvailableCopies now comes from request instead of defaulting to TotalCopies
        var book = new Book { Title = req.Title, Author = req.Author, ISBN = req.ISBN, TotalCopies = req.TotalCopies, AvailableCopies = req.AvailableCopies };
        await _repo.AddAsync(book);
        await _repo.SaveChangesAsync();
        _cache.Remove(CACHE_KEY);
        return MapToDTO(book);
    }

    public async Task UpdateBookAsync(int id, BookRequestDTO req) {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) throw new KeyNotFoundException();
        
        // Added: Check for duplicate ISBN when updating (excluding current book)
        var existingBook = (await _repo.GetAllAsync()).FirstOrDefault(b => b.ISBN == req.ISBN && b.Id != id);
        if (existingBook != null)
            throw new InvalidOperationException($"A book with ISBN '{req.ISBN}' already exists.");
        
        book.Title = req.Title;
        // Updated: now updates all fields, not just Title
        book.Author = req.Author;
        book.ISBN = req.ISBN;
        book.TotalCopies = req.TotalCopies;
        book.AvailableCopies = req.AvailableCopies;
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