using TAP.Core.DatastoreORM.DAO;

namespace TAP.Core.DatastoreORM.Test.DAO
{
    [Kind("TestORM-Country")]
    public class CountryDAO: BaseDAO
    {
        [KindKey(false)]
        public string CountryCode { get; set; }

        public string Name { get; set; }


        [DbFieldName("description")]
        public string Description { get; set; }
    }
}
