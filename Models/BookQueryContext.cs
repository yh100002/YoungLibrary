using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class BookQueryContext : DbContext
    {
        public BookQueryContext(DbContextOptions<BookQueryContext> options) : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }                
    }
}