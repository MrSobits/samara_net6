namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.B4.DataAccess;

    public interface IBlobPropertyService<TEntity, TBlobEntity>
        where TEntity : PersistentObject, new() where TBlobEntity : PersistentObject, new()
    {
        IDataResult Get(BaseParams baseParams);

        IDataResult Save(BaseParams baseParams);
    }
}