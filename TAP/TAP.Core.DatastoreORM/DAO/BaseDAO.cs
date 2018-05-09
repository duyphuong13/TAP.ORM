namespace TAP.Core.DatastoreORM.DAO
{
    public abstract class BaseDAO
    {
    }

    public abstract class IdentityBaseDAO : BaseDAO
    {
        [KindKey]
        public long Id { get; set; }
    }

}
