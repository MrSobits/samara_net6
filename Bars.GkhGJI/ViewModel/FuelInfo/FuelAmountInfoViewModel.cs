namespace Bars.GkhGji.ViewModel.FuelInfo
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Представление для Поставка, расход и остатки топлива
    /// </summary>
    public class FuelAmountInfoViewModel : BaseViewModel<FuelAmountInfo>
    {
        /// <inheritdoc/>
        public override IDataResult List(IDomainService<FuelAmountInfo> domainService, BaseParams baseParams)
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