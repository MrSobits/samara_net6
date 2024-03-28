namespace Bars.GkhCr.Repositories.PerformedWorkActs
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Repositories;
    using Bars.GkhCr.Entities;

    public interface IPerformedWorkActRepository : IDomainRepository<PerformedWorkAct>
    {
        IQueryable<PerformedWorkAct> GetTargetActsFor(IEnumerable<string> targetGuids);
    }
}
