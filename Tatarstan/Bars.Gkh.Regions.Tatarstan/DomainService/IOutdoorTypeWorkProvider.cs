namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System.Linq;

    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public interface IOutdoorTypeWorkProvider
    {
        IQueryable<WorkRealityObjectOutdoor> GetWorks(long outdoorId, long periodId);
    }
}
