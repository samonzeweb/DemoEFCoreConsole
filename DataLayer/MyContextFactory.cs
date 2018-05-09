using DemoEFCoreConsole.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace DemoEFCoreConsole.DataLayer
{
    // Why this factory ? For design time ! (using 'dotnet ef' CLI)
    // It's not useful for all project.
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation#from-a-design-time-factory
    public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationFactory.Create();
            var databaseSettings = configuration.GetDatabaseSettings();

            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlite(databaseSettings.ConnectionString);
            return new MyContext(optionsBuilder.Options);
        }
    }
}
