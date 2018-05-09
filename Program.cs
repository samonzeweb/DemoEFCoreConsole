using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoEFCoreConsole.Configuration;
using DemoEFCoreConsole.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCoreConsole
{
    class Program
    {
        const int _iterations = 5;
        const int _totalRows = 100_000;
        static void Main(string[] args)
        {
            // Fetching configuration
            var configuration = ConfigurationFactory.Create();
            var databaseSettings = configuration.GetDatabaseSettings();

            // Just for the example, use either a more robust factory,
            // or dependency injection.
            Func<MyContext>  createContext = () => {
                var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
                optionsBuilder.UseSqlite(databaseSettings.ConnectionString);
                return new MyContext(optionsBuilder.Options);
             };


            using(var ctx = createContext())
            {
                Console.WriteLine("Initializing database");
                // Don't do that in production unless you know what you're doing
                ctx.Database.Migrate();
            }

            for(var i=0;i<_iterations;i++)
            {
                using(var ctx = createContext())
                {
                    RemoveExistingContent(ctx);
                    Console.Write($"Inserting {_totalRows} rows ...");
                    var chrono = new Stopwatch();
                    chrono.Start();
                    InsertDummyContent(ctx);
                    chrono.Stop();
                    Console.WriteLine($" time elapsed : {chrono.ElapsedMilliseconds} ms");
                }
            }

            for(var i=0;i<_iterations;i++)
            {
                using(var ctx = createContext())
                {
                    Console.Write($"Reading {_totalRows} rows ...");
                    var chrono = new Stopwatch();
                    chrono.Start();
                    ReadDummyContent(ctx);
                    chrono.Stop();
                    Console.WriteLine($" time elapsed : {chrono.ElapsedMilliseconds} ms");
                }
            }
        }

        static void RemoveExistingContent(MyContext ctx)
        {
            // There is no 'truncate' command in Sqlite
            ctx.Database.ExecuteSqlCommand("delete from MyTable");
        }

        static void InsertDummyContent(MyContext ctx)
        {
            var rowList = new List<MyTable>();
            for(var i=0;i<_totalRows;i++)
            {
                rowList.Add(new MyTable {
                    DummyString = "I'm a dummy string",
                });
            }
            ctx.AddRange(rowList);
            ctx.SaveChanges(); // or SaveChangesAsync with async/await
        }

        static void ReadDummyContent(MyContext ctx)
        {
            var rowList = ctx.MyTable.ToList(); // or ToListAsync with async/await
        }
    }
}
