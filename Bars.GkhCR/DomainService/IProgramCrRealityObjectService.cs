namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.Gkh.Entities;

    public interface IProgramCrRealityObjectService
    {
        IQueryable<RealityObject> GetObjectsInMainProgram();
    }
}
