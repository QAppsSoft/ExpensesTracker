using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TestsCommons.Db;

public sealed class DbContextFactoryFixture : IDbContextFactory<ExpensesTrackerDbContext>, IDisposable
{
    private readonly DatabaseSeedDataFixture _fixture = new();
    
    public ExpensesTrackerDbContext CreateDbContext()
    {
        return _fixture.ExpensesTrackerDatabaseContextFactory();
    }

    public void Dispose()
    {
        _fixture.Dispose();
    }
}