namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис фильтрации займов
    /// </summary>
    public class RealityObjectLoanViewService : IRealityObjectLoanViewService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoan"/>
        /// </summary>
        public IDomainService<RealityObjectLoan> RealityObjectLoanDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoanWallet"/>
        /// </summary>
        public IDomainService<RealityObjectLoanWallet> RealityObjectLoanWalletDomain { get; set; }

        /// <summary>
        /// Метод возвращает запрос согласно фильтрам
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <param name="fetchWalletNames">Подгрузить имена кошельков (нужно для счета оплат дома)</param>
        /// <returns>Подзапрос</returns>
        public IQueryable<RealityObjectLoanDto> List(BaseParams baseParams, bool fetchWalletNames)
        {
            var loadParams = baseParams.GetLoadParam();

            var roId = baseParams.Params.GetAsId("roId");
            var ids = baseParams.Params.GetAs("ids", new long[0]);

            if (roId == 0)
            {
                // Так пишу потому что в клиентской части в займах переделал отправку параметра на filter чтобы не упало 
                // если есть еще места которые работают с этим параметром как с Params
                roId = loadParams.Filter.GetAsId("roId");
            }

            var query = this.RealityObjectLoanDomain.GetAll()
                    .WhereIf(roId > 0, x => x.LoanTaker.RealityObject.Id == roId && !x.State.StartState)
                    .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                    .Select(x => new
                    {
                        Loan = x,
                        ProgramCr = x.ProgramCr.Name,
                        LoanReceiver = x.LoanTaker.RealityObject.Address,
                        Municipality = x.LoanTaker.RealityObject.Municipality.Name,
                        Saldo = x.LoanTaker.DebtTotal - x.LoanTaker.CreditTotal,
                        Settlement = x.LoanTaker.RealityObject.MoSettlement.Name,
                        Document = (long?)x.Document.Id
                    });

            // если нужны кошельки, то по-другому обрабатываем запрос
            if (fetchWalletNames)
            {
                var walletNames = this.RealityObjectLoanWalletDomain.GetAll()
                .WhereIf(roId > 0, x => x.Loan.LoanTaker.RealityObject.Id == roId)
                .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Loan.Id))
                .Select(x => new { x.Loan.Id, x.TypeSourceLoan })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.AggregateWithSeparator(z => z.TypeSourceLoan.GetEnumMeta().Display, ", "));

                return query.AsEnumerable().Select(x =>
                        new RealityObjectLoanDto
                        {
                            Id = x.Loan.Id,
                            LoanDate = x.Loan.LoanDate,
                            State = x.Loan.State,
                            ProgramCr = x.ProgramCr,
                            LoanReceiver = x.LoanReceiver,
                            Municipality = x.Municipality,
                            Settlement = x.Settlement,
                            LoanSum = x.Loan.LoanSum,
                            Saldo = x.Saldo,
                            LoanReturnedSum = x.Loan.LoanReturnedSum,
                            DebtSum = x.Loan.LoanSum - x.Loan.LoanReturnedSum,
                            PlanLoanMonthCount = x.Loan.PlanLoanMonthCount,
                            FactEndDate = x.Loan.FactEndDate,
                            Document = x.Document,
                            Sources = walletNames.Get(x.Loan.Id) ?? string.Empty
                        })
                .AsQueryable()
                .Filter(loadParams, this.Container);
            }

            return
                query.Select(x =>
                    new RealityObjectLoanDto
                    {
                        Id = x.Loan.Id,
                        LoanDate = x.Loan.LoanDate,
                        State = x.Loan.State,
                        ProgramCr = x.ProgramCr,
                        LoanReceiver = x.LoanReceiver,
                        Municipality = x.Municipality,
                        Settlement = x.Settlement,
                        LoanSum = x.Loan.LoanSum,
                        Saldo = x.Saldo,
                        LoanReturnedSum = x.Loan.LoanReturnedSum,
                        DebtSum = x.Loan.LoanSum - x.Loan.LoanReturnedSum,
                        PlanLoanMonthCount = x.Loan.PlanLoanMonthCount,
                        FactEndDate = x.Loan.FactEndDate,
                        Document = x.Document
                    }).Filter(loadParams, this.Container);
        }
    }
}