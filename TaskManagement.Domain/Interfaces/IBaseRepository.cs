using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> UpdateAsync(T entity);
    bool Exist(int id);
    Task DeleteAsync(T entity);
}