namespace Librium.Domain.Common.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<bool> SaveChanges();
    Task Delete(T entity);
}
