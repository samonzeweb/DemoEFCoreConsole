using Microsoft.EntityFrameworkCore;

namespace DemoEFCoreConsole.DataLayer
{
    public class MyContext : DbContext
    {
        public DbSet<MyTable> MyTable { get; set; }
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nothing useful here, but it's the right place to configure models, eg:
            modelBuilder.Entity<MyTable>().Property(mt => mt.DummyString).HasMaxLength(32);
        }
    }
}
