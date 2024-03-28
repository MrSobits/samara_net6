namespace Bars.GkhGji.ViewModel.FuelInfo
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Представление для Расстояние от места добычи топлива до потребителя
    /// </summary>
    public class FuelExtractionDistanceInfoViewModel : BaseViewModel<FuelExtractionDistanceInfo>
    {
        /// <inheritdoc/>
        public override IDataResult List(IDomainService<FuelExtractionDistanceInfo> domainService, BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");

            var data = domainService.GetAll()
                .Where(x => x.FuelInfoPeriod.Id == periodId)
                .OrderBy(x => x.RowNumber)
                .ToList();

            return new ListDataResult(data, data.Count);
        }
    }
}