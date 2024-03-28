namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Представление для Запись Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringInfoViewModel : BaseViewModel<HousingFundMonitoringInfo>
    {
        /// <inheritdoc/>
        public override IDataResult List(IDomainService<HousingFundMonitoringInfo> domainService, BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");

            var data = domainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .OrderBy(x => x.RowNumber)
                .ToList();

            return new ListDataResult(data, data.Count);
        }
    }
}