namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.DomainService;
    using Castle.Windsor;
    using ServiceStack.Common;

    public class RealObjDecInfoService : IRealObjDecInfoService
    {
        public IWindsorContainer Container { get; set; }

        public HashSet<long> GetRealityObjHasDecProtocol(IQueryable<RealityObject> roQuery)
        {
            var realObjDecDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();

            try
            {
                return realObjDecDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

            }
            finally
            {
                Container.Release(realObjDecDomain);
            }
        }
    }
}