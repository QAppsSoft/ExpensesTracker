using System.Diagnostics;
using Api.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestsCommons.Db;

public sealed class DatabaseSeedDataFixture : IDisposable
{
    private readonly TemporalStorage _storageFixture = new();
    public Func<ExpensesTrackerDbContext> ExpensesTrackerDatabaseContextFactory { get; }
    private readonly DbContextOptions<ExpensesTrackerDbContext> _contextOptions;

    public DatabaseSeedDataFixture()
    {
        var databasePath = _storageFixture.GetTemporalFileName(".db");
        
        var builder = new SqliteConnectionStringBuilder { DataSource =  databasePath};

        var options = new DbContextOptionsBuilder<ExpensesTrackerDbContext>();
        options.UseSqlite(builder.ConnectionString);
        options.EnableSensitiveDataLogging();
        options.LogTo(s => Debug.WriteLine(s));
        
        _contextOptions = options.Options;
        
        using var databaseContext = GetContext();

        databaseContext.Database.EnsureCreated();

        ExpensesTrackerDatabaseContextFactory = GetContext;
    }

    private ExpensesTrackerDbContext GetContext() => new(_contextOptions);

    public void Dispose()
    {
        _ = GetContext().Database.EnsureDeleted();
        _storageFixture.Dispose();
    }
}