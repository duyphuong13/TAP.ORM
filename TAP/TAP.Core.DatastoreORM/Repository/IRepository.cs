using System.Threading.Tasks;
using TAP.Core.DatastoreORM.DAO;

namespace TAP.Core.DatastoreORM.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : BaseDAO
    {
        Task<TKey> InsertAsync(TEntity entity);
        Task<TKey[]> InsertAsync(TEntity[] dalEntities);

        Task<TEntity[]> GetByIdsAsync(params TKey[] ids);

        Task<TEntity> GetByIdAsync(TKey id);

        Task<TEntity[]> GetAllAsync(int limit = 1000);

        //Task<TEntity[]> FindEqualAsync();

        //Task<TEntity[]> FindEqualsAsync(int limit = int.MaxValue, string currentPageToken = null, out string nextPageToken);

        Task DeleteAsync(TKey id);
        Task DeleteAsync(TKey[] ids);

        Task UpdateAsync(TEntity entity);
    }
}
