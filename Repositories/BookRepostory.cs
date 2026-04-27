using Libtech.Api.Models;
using Libtech.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace Libtech.Api.Repositories;

public class BookRepository : IBookRepository {
    private readonly LibtechContext _db;
    public BookRepository(LibtechContext db) => _db = db;
    public async Task<IEnumerable<Book>> GetAllAsync() => await _db.Books.ToListAsync();
    public async Task<Book?> GetByIdAsync(int id) => await _db.Books.FindAsync(id);
    public async Task AddAsync(Book book) => await _db.Books.AddAsync(book);
    public async Task UpdateAsync(Book book) { _db.Books.Update(book); await Task.CompletedTask; }
    public async Task DeleteAsync(int id) {
        var b = await GetByIdAsync(id);
        if (b != null) _db.Books.Remove(b);
    }
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}