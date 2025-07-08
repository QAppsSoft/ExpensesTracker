using Api.Data;
using Api.Models;
using Api.Endpoints.CategoryEndpoint.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.CategoryEndpoint.Repository;

public class CategoryRepository(ExpensesTrackerDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Name == name)
            .ConfigureAwait(false);
    }

    public async Task CreateAsync(Category category)
    {
        await context.Categories.AddAsync(category).ConfigureAwait(false);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}