using System;
using System.Threading.Tasks;
using TAP.Core.DatastoreORM.Repository;
using TAP.Core.DatastoreORM.Test.DAO;

namespace TAP.Core.DatastoreORM.Test.Repository
{
    public interface IPlayerRepository: IRepository<PlayerDAO, long>
    {
        Task<PlayerDAO[]> GetNewPlayers(int limit, DateTime fromTime);
    }
}
