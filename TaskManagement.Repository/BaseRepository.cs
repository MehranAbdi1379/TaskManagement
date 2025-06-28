using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly TaskManagementDBContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(TaskManagementDBContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.Where(x => x.Id == id && x.Deleted == false).FirstAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(x => x.Deleted == false).ToListAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FirstAsync(x => x.Id == id && x.Deleted == false);
        entity.Delete();
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}