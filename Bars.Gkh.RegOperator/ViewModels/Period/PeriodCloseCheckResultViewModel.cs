namespace Bars.Gkh.RegOperator.ViewModels.Period
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Enums;

    public class PeriodCloseCheckResultViewModel : BaseViewModel<PeriodCloseCheckResult>
    {
        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<PeriodCloseCheckResult> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var periodId = baseParams.Params.GetAs<long?>("periodId") ?? loadParams.Filter.GetAsId("periodId");
            if (periodId == 0)
            {
                return BaseDataResult.Error("Не передан идентификатор периода");
            }

            var data = domainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.IsCritical,
                        x.CheckState,
                        x.CheckDate,
                        x.LogFile,
                        x.FullLogFile,
                        x.Name,
                        x.Note,
                        PersAccGroup = x.CheckState == PeriodCloseCheckStateType.Error ? x.PersAccGroup : null,
                        x.User
                    })
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }
    }
}