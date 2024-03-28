namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System.Collections;
    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    public interface IUtilityDebtorClaimWorkService
    {
        /// <summary>
        /// Метод получения списка 
        /// </summary>
        IList GetList(IDomainService<UtilityDebtorClaimWork> domainService, BaseParams baseParams, bool isPaging, ref int totalCount);
    }
}
