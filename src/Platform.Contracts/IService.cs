using Platform.Core.Query;

namespace Platform.Contracts;
public interface IService<T> where T : EntityBase
{
    Task<QueryResult<T>> QueryAll(QueryParams query);
    Task<T> Find(int id);
    Task<T> Save(T entity);
    Task<bool> Remove(T entity);
}