namespace Bars.Gkh.Repositories
{
    using Bars.B4.DataAccess;

    public interface IDomainRepository<T> where T : BaseEntity
    {
        void SaveOrUpdate(T entity);
    }
}
