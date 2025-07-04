using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ExpensesTrackerDbContext : DbContext
{
    DbSet<Category> Categories { get; set; }
}