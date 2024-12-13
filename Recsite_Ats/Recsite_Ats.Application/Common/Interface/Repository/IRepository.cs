using System.Linq.Expressions;

namespace Recsite_Ats.Application.Common.Interface.Repository;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T?> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null);
    Task<T> Add(T entity);
    Task BulkAdd(IEnumerable<T> entities);
    Task Update(T entity);
    Task<T> UpdateAndGetData(T entity);
    Task BulkUpdate(IEnumerable<T> entities);
    Task BulkDelete(IEnumerable<T> entities);
    void Remove(T entity);
    Task SelectedRemove(Expression<Func<T, bool>> predicate);
    Task<int> Max(Expression<Func<T, int>> selector, Expression<Func<T, bool>>? filter = null);

}
