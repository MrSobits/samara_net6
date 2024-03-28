namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;

    using Bars.Gkh.RegOperator.DomainService;

    using DataResult;
    using Entities;

    /// <summary>
    /// Вью-модель займов жилого дома
    /// </summary>
    public class RealityObjectLoanViewModel : BaseViewModel<RealityObjectLoan>
    {
        /// <summary>
        /// Сервис фильтрации займов
        /// </summary>
        public IRealityObjectLoanViewService RealityObjectLoanViewService { get; set; }

        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<RealityObjectLoan> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = this.RealityObjectLoanViewService.List(baseParams, true);

            var totalCount = data.Count();
            var loanSum = totalCount > 0 ? data.Sum(x => x.LoanSum) : 0;
            var loanReturned = totalCount > 0 ? data.Sum(x => x.LoanReturnedSum) : 0;

            data = data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(
                       data.ToList(),
                       totalCount,
                       new { LoanSum = loanSum, LoanReturnedSum = loanReturned, DebtSum = loanSum - loanReturned });
        }
    }
}