namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Overhaul.Entities;

    public interface IRealObjStructElementService
    {
        IDataResult MassDelete(BaseParams baseParams);

        IDataResult List(BaseParams baseParams);

        IDataResult ListRoForMassDelete(BaseParams baseParams);

        IQueryable<RealityObjectStructuralElement> GetUsedInLongProgram();
    }
}