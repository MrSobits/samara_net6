namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис получения информации о денежных движениях по управлению домами
    /// </summary>
    public class DisclosureInfoHouseManagingMoneys : IDisclosureInfoHouseManagingMoneys
    {
        /// <summary>
        /// Домен-сервис <see cref="FinActivityManagRealityObj"/>
        /// </summary>
        public IDomainService<FinActivityManagRealityObj> FinActivityManagRealityObjDomainService { get; set; }

        /// <summary>
        /// Метод возвращает информацию о денежных движениях по управлению домами
        /// </summary>
        /// <param name="diQuery">Запрос фильтрации</param>
        /// <returns>Словарь</returns>
        public IDictionary<long, DisclosureInfoHouseManagingInfo> GetDisclosureInfoHouseManagingInfo(IQueryable<DisclosureInfo> diQuery)
        {
            return this.FinActivityManagRealityObjDomainService.GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .Select(x => new
                    {
                        Id = x.DisclosureInfo.ManagingOrganization.Id,
                        RoId = x.RealityObject.Id,
                        SumFactExpense = x.SumFactExpense ?? 0,
                        SumIncomeManage = x.SumIncomeManage ?? 0,
                        ReceivedProvidedService = x.ReceivedProvidedService ?? 0,
                        PresentedToRepay = x.PresentedToRepay ?? 0
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => new DisclosureInfoHouseManagingInfo
                    {
                        Id = y.Key,
                        SumIncomeManage = y.DistinctBy(x => x.RoId).SafeSum(x => x.SumIncomeManage),
                        SumFactExpense = y.DistinctBy(x => x.RoId).SafeSum(x => x.SumFactExpense),
                        ReceivedProvidedService = y.DistinctBy(x => x.RoId).SafeSum(x => x.ReceivedProvidedService),
                        PresentedToRepay = y.DistinctBy(x => x.RoId).SafeSum(x => x.PresentedToRepay),
                    });
        }
    }
}