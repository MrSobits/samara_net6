namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Подготовка к работе в зимних условиях
    /// </summary>
    public class WorkWinterConditionViewModel : BaseViewModel<WorkWinterCondition>
    {
        /// <summary>
        /// Получить список подготовок к работе в зимних условиях
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="baseParams"></param>
        /// <returns>Список значений подготовок к работе в зимних условиях</returns>
        public override IDataResult List(IDomainService<WorkWinterCondition> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var heatInputPeriodId = baseParams.Params.GetAs<long>("hipId");

            var data = domainService.GetAll()
                .Where(x => x.HeatInputPeriod.Id == heatInputPeriodId)
                .Select(x => new
                {
                    x.Id,
                    x.WorkInWinterMark.Name,
                    x.WorkInWinterMark.RowNumber,
                    x.WorkInWinterMark.Measure,
                    x.WorkInWinterMark.Okei,
                    x.Total,
                    x.PreparationTask,
                    x.PreparedForWork,
                    x.FinishedWorks,
                    x.Percent
                })
                .OrderBy(x => x.RowNumber)
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            var totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParams).ToList(), totalCount);
        }
    }
}