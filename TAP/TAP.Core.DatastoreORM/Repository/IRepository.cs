using System.Threading.Tasks;
using TAP.Core.DatastoreORM.DAO;

namespace TAP.Core.DatastoreORM.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : BaseDAO
    {
        Task<TKey> InsertAsync(TEntity entity);
        Task<TKey[]> InsertAsync(TEntity[] dalEntities);

        Task<TEntity> GetByIdAsync(TKey id);

        Task DeleteAsync(TKey id);
        Task DeleteAsync(TKey[] ids);

        Task UpdateAsync(TEntity entity);
    }
}
