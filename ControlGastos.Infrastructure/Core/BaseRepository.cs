using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Core;
using ControlGastos.Domain.Repository;
using ControlGastos.Infrastructure.Context;

namespace ControlGastos.Infrastructure.Core;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ControlGastosContext _context;
    protected readonly DbSet<T> _set;

    public BaseRepository(ControlGastosContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await _set.Where(e => !e.Deleted).ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) =>
        await _set.FirstOrDefaultAsync(e => e.Id == id && !e.Deleted);

    public virtual async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return;
        entity.Deleted = true; // borrado lógico
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(int id) =>
        await _set.AnyAsync(e => e.Id == id && !e.Deleted);
}
