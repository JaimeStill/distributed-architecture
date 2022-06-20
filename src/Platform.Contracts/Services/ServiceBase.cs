using Microsoft.EntityFrameworkCore;
using Platform.Core;
using Platform.Core.Query;

namespace Platform.Contracts;
public class ServiceBase<T> : IService<T> where T : EntityBase
{
    protected DbContext db;
    protected DbSet<T> set;

    public ServiceBase(DbContext db)
    {
        this.db = db;
        set = db.Set<T>();
    }

    protected static async Task<QueryResult<T>> Query(
        IQueryable<T> queryable,
        QueryParams query,
        Func<IQueryable<T>, string, IQueryable<T>> search
    )
    {
        var container = new QueryContainer<T>(
            queryable,
            query
        );

        return await container.Query((data, s) =>
            data.SetupSearch(s, search));
    }

    protected virtual async Task<T> Add(T entity)
    {
        try
        {
            await set.AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new ServiceException<T>("Add", ex);
        }
    }

    protected virtual async Task<T> Update(T entity)
    {
        try
        {
            set.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new ServiceException<T>("Update", ex);
        }
    }

    public virtual async Task<QueryResult<T>> QueryAll(QueryParams query) =>
        await Query(
            set, query, (data, term) => data
        );

    public virtual async Task<T> Find(int id) =>
        await set.FindAsync(id);

    public virtual async Task<T> Save(T entity) =>
        entity.Id > 0
            ? await Update(entity)
            : await Add(entity);

    public virtual async Task<bool> Remove(T entity)
    {
        set.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }
}