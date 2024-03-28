namespace Bars.GkhCr.DomainService.GisGkhRegional.Impl
{
    using Bars.GkhCr.DomainService.GisGkhRegional;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : IGisGkhRegionalService
    {
        public virtual GisGkhWorkFinancingType GetWorkFinancingType(TypeWorkCr work)
        {
            return GisGkhWorkFinancingType.Owners;
        }
    }
}