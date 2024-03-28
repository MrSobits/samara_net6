namespace Bars.GkhGji.ViewModel.FuelInfo
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Представление для Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР)
    /// по состоянию на конец отчетного периода
    /// </summary>
    public class FuelEnergyDebtInfoViewModel : BaseViewModel<FuelEnergyDebtInfo>
    {
        /// <inheritdoc/>
        public override IDataResult List(IDomainService<FuelEnergyDebtInfo> domainService, BaseParams baseParams)
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