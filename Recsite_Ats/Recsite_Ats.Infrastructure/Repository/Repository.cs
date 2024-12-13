using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Infrastructure.Data;
using System.Linq.Expressions;

namespace Recsite_Ats.Infrastructure.Repository;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet { get; }
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    public async Task<T> Add(T entity)
    {
        var newEntity = await dbSet.AddAsync(entity);
        return newEntity.Entity;
    }
    public async Task Update(T entity) // Implementing the Update method
    {
        dbSet.Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<T> UpdateAndGetData(T entity)
    {
        dbSet.Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task BulkUpdate(IEnumerable<T> entities)
    {
        await _db.BulkUpdateAsync(entities.ToList());
    }

    public async Task BulkAdd(IEnumerable<T> entities)
    {
        await _db.BulkInsertAsync(entities.ToList());
    }

    public async Task BulkDelete(IEnumerable<T> entities)
    {
        await _db.BulkDeleteAsync(entities.ToList());
    }
    public async Task<T?> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return query.FirstOrDefault();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _db.Set<T>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return query.ToList();
    }
    public async void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public async Task SelectedRemove(Expression<Func<T, bool>> predicate)
    {
        var entities = dbSet.Where(predicate).ToList(); // Get entities matching the condition
        dbSet.RemoveRange(entities);
    }

    // Adding Max method to the repository
    public async Task<int> Max(Expression<Func<T, int>> selector, Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Check if the sequence contains any elements
        bool hasElements = await query.AnyAsync();

        if (!hasElements)
        {
            return 0;
        }

        int? maxValue = await query.MaxAsync(selector);
        return maxValue ?? 0;
    }

}
