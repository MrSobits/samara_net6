namespace Bars.GkhCr.Repositories.PerformedWorkActs
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Repositories;
    using Bars.GkhCr.Entities;
    using NHibernate.Linq;

    public class PerformedWorkActRepository : BaseDomainRepository<PerformedWorkAct>, IPerformedWorkActRepository
    {
        public IQueryable<PerformedWorkAct> GetTargetActsFor(IEnumerable<string> targetGuids)
        {
            var paymentDomain = Container.Resolve<IDomainService<PerformedWorkActPayment>>();
            return
                paymentDomain.GetAll()
                    .Fetch(x => x.PerformedWorkAct)
                    .Where(x => targetGuids.Contains(x.TransferGuid))
                    .Select(x => x.PerformedWorkAct)
                    .Distinct();

        }
    }
}
