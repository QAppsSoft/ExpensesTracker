using Api.Data;
using Api.Data.Context;
using Api.Endpoints.CategoryEndpoint.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.CategoryEndpoint.Repository;

public class CategoryRepository(ExpensesTrackerDbContext context) : ICategoryRepository
{
    public Task<bool> ExistsByIdAsync(int id)
    {
        return context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<ICollection<Category>> GetAllAsync()
    {
        return await context.Categories.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Name == name)
            .ConfigureAwait(false);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories.FindAsync(id).ConfigureAwait(false);
    }

    public async Task CreateAsync(Category category)
    {
        await context.Categories.AddAsync(category).ConfigureAwait(false);
    }

    public Task RemoveAsync(Category category)
    {
        context.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}