namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

    public interface IActivityTsjService
    {
        IQueryable<ActivityTsj> GetFilteredByOperator(IDomainService<ActivityTsj> domainService);
    }
}