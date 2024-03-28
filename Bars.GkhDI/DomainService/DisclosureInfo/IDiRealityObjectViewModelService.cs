namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис для получения управляемых домов УО
    /// </summary>
    public interface IDiRealityObjectViewModelService
    {
        IQueryable<long> GetManagedRealityObjects(DisclosureInfo disclosureInfo, long disclosureInfoRealityObjId = 0);
    }
}