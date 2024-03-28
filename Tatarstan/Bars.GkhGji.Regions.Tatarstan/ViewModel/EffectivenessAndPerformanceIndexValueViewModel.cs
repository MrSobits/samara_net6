namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class EffectivenessAndPerformanceIndexValueViewModel : BaseViewModel<EffectivenessAndPerformanceIndexValue>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<EffectivenessAndPerformanceIndexValue> domainService, BaseParams baseParams)
        {
                return domainService.GetAll().Select(x => new
                {
                    x.Id,
                    x.CalculationStartDate,
                    x.CalculationEndDate,
                    x.EffectivenessAndPerformanceIndex,
                    EffectivenessAndPerformanceIndexName = x.EffectivenessAndPerformanceIndex.Name,
                    x.Value
                })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
