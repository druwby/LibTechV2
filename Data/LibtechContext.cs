using Microsoft.EntityFrameworkCore;
using Libtech.Api.Models;

namespace Libtech.Api.Data;

public class LibtechContext : DbContext
{
    public LibtechContext(DbContextOptions<LibtechContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().Property(b => b.ISBN).IsRequired();
        modelBuilder.Entity<Member>().Property(m => m.Email).IsRequired();
    }
}