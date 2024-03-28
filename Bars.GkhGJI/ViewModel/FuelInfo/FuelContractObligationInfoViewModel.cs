namespace Bars.GkhGji.ViewModel.FuelInfo
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Представление для Выполнение договорных обязательств по поставкам топлива
    /// </summary>
    public class FuelContractObligationInfoViewModel : BaseViewModel<FuelContractObligationInfo>
    {
        /// <inheritdoc/>
        public override IDataResult List(IDomainService<FuelContractObligationInfo> domainService, BaseParams baseParams)
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