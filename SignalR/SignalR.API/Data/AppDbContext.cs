using Microsoft.EntityFrameworkCore;
using SignalR.API.Data.Entity;

namespace SignalR.API.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
