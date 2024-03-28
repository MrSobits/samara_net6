namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Linq;
    using B4;
    using Overhaul.Entities;

    public interface IRealityObjectStructElementService
    {
        IQueryable<RealityObjectStructuralElement> GetElementsForLongProgram();

        IQueryable<RealityObjectStructuralElement> GetElementsForLongProgram(IDomainService<RealityObjectStructuralElement> domainService);
    }
}
