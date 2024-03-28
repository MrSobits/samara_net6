namespace Bars.GkhCr.Regions.Tymen.DomainService.GisGkhRegional.Impl
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using System.Linq;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public class GisGkhRegionalService : Bars.GkhCr.DomainService.GisGkhRegional.Impl.GisGkhRegionalService
    {
        public override GisGkhWorkFinancingType GetWorkFinancingType(TypeWorkCr work)
        {
            if (work.FinanceSource != null)
            {
                switch (work.FinanceSource.Name)
                {
                    case "Региональный оператор":
                        return GisGkhWorkFinancingType.Owners;
                    case "Бюджет субъекта":
                        return GisGkhWorkFinancingType.RegionBudget;
                    default:
                        return GisGkhWorkFinancingType.Owners;
                }
            }
            else return GisGkhWorkFinancingType.Owners;
        }
    }
}