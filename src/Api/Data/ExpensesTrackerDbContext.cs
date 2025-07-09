using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ExpensesTrackerDbContext(DbContextOptions<ExpensesTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
}