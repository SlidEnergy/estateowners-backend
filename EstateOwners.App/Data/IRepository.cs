using EstateOwners.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public interface IRepository<TEntity, T> where TEntity : class, IUniqueObject<T>
    {
        Task<TEntity> GetById(T id);
        Task<List<TEntity>> GetList();
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
