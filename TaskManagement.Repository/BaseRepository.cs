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
        var entity = await _dbSet.Where(x => x.Id == id && x.Deleted == false).FirstOrDefaultAsync();
        if (entity == null) throw new NullReferenceException($"Entity of type {typeof(T)} with id {id} not found");
        return entity;
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
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (entity == null) throw new NullReferenceException($"Entity of type {typeof(T)} not found");
        entity.Delete();
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public bool Exist(int id)
    {
        return _dbSet.Any(x => x.Id == id && x.Deleted == false);
    }

    public async Task DeleteAsync(T entity)
    {
        entity.Delete();
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}