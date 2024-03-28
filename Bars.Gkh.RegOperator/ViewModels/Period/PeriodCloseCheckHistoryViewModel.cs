namespace Bars.Gkh.RegOperator.ViewModels.Period
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseCheckHistoryViewModel : BaseViewModel<PeriodCloseCheckHistory>
    {
        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<PeriodCloseCheckHistory> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var checkId = baseParams.Params.GetAs<long?>("checkId") ?? loadParams.Filter.GetAsId("checkId");
            if (checkId == 0)
            {
                return BaseDataResult.Error("Не передан идентификатор проверки");
            }

            var data = domainService.GetAll()
                .Where(x => x.Check.Id == checkId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.ChangeDate,
                        x.IsCritical,
                        x.User
                    })
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }
    }
}