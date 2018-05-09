using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Core.DatastoreORM.Repository;
using TAP.Core.DatastoreORM.Test.DAO;

namespace TAP.Core.DatastoreORM.Test.Repository
{
    public interface ICountryRepository: IRepository<CountryDAO, string>
    {
    }
}
