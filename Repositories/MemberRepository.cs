using Libtech.Api.Models;
using Libtech.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace Libtech.Api.Repositories;

public class MemberRepository : IMemberRepository {
    private readonly LibtechContext _db;
    public MemberRepository(LibtechContext db) => _db = db;
    public async Task<IEnumerable<Member>> GetAllAsync() => await _db.Members.ToListAsync();
    public async Task<Member?> GetByIdAsync(int id) => await _db.Members.FindAsync(id);
    public async Task AddAsync(Member member) => await _db.Members.AddAsync(member);
    public async Task UpdateAsync(Member member) { _db.Members.Update(member); await Task.CompletedTask; }
    public async Task DeleteAsync(int id) {
        var m = await GetByIdAsync(id);
        if (m != null) _db.Members.Remove(m);
    }
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}