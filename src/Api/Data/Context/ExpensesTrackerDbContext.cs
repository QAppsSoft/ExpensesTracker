using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context;

public class ExpensesTrackerDbContext(DbContextOptions<ExpensesTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
}