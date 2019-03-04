using Microsoft.EntityFrameworkCore;


namespace Models
{
    public class BookCommandContext : DbContext
    {
        public BookCommandContext(DbContextOptions<BookCommandContext> options) : base(options)
        {
        }

        public DbSet<BookData> BookData { get; set; }        
    }
}