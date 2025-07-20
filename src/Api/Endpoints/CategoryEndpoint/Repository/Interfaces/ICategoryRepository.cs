using Api.Data;

namespace Api.Endpoints.CategoryEndpoint.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<bool> ExistsByIdAsync(int id);
    Task<ICollection<Category>> GetAllAsync();
    Task<Category?> GetByNameAsync(string name);
    Task<Category?> GetByIdAsync(int id);
    Task CreateAsync(Category category);
    Task RemoveAsync(Category category);
    Task UpdateAsync(Category updatedCategory);
    Task SaveChangesAsync();
}