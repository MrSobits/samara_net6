namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;

    public interface IRealObjDecInfoService
    {
        HashSet<long> GetRealityObjHasDecProtocol(IQueryable<RealityObject> roQuery);
    }
}
