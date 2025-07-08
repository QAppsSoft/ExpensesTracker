using Api.Models;

namespace Api.Endpoints.CategoryEndpoint.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByNameAsync(string name);
    
    Task CreateAsync(Category category);
    
    Task SaveChangesAsync();
}