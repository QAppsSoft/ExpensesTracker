using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TestsCommons.Db;

public class MockDb : IDbContextFactory<ExpensesTrackerDbContext>
{
    public MockDb()
    {
        var options = new DbContextOptionsBuilder<ExpensesTrackerDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var dbContext = new ExpensesTrackerDbContext(options);

        LastContext = dbContext;
    }
    
    public ExpensesTrackerDbContext LastContext { set; get; }
    
    public ExpensesTrackerDbContext CreateDbContext()
    {
        return LastContext;
    }

    public Task<ExpensesTrackerDbContext> CreateDbContextAsync(CancellationToken cancellationToken = new())
    {
        return Task.FromResult(CreateDbContext());
    }
}