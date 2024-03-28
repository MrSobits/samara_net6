namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Базовый сервис получения потребности в займах
    /// </summary>
    public abstract class RealtyObjectNeedLoanBaseService : IRealtyObjectNeedLoanService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectPaymentAccount"/>
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> RealityObjectPaymentAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectChargeAccount"/>
        /// </summary>
        public IDomainService<RealityObjectChargeAccount> RealityObjectChargeAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLand"/>
        /// </summary>
        public IDomainService<RealityObjectLoan> RealityObjectLoanDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoanTask"/>
        /// </summary>
        public IDomainService<RealityObjectLoanTask> RealityObjectLoanTaskDomain { get; set; }

        /// <summary>
        /// Источник получения потребности
        /// </summary>
        public abstract LoanFormationType LoanFormationType { get; }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public abstract IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(Municipality municipality, ProgramCr program);

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public abstract IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(RealityObject[] robjects, ProgramCr program);

        /// <summary>
        /// заполнить текущие балансы (средства по источникам)
        /// </summary>
        protected virtual void FillCurrentBalances(RealtyObjectNeedLoan[] objects)
        {
            var roIds = objects.Select(x => x.Id).ToArray();

            var currentBalances = this.RealityObjectPaymentAccountDomain.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    OwnerBalance =
                        x.BaseTariffPaymentWallet.Balance
                        + x.DecisionPaymentWallet.Balance
                        + x.PenaltyPaymentWallet.Balance
                        + x.SocialSupportWallet.Balance,
                    SubsidyBalance =
                        x.FundSubsidyWallet.Balance
                        + x.RegionalSubsidyWallet.Balance
                        + x.StimulateSubsidyWallet.Balance
                        + x.TargetSubsidyWallet.Balance,
                    OtherSourceBalance =
                        x.AccumulatedFundWallet.Balance
                        + x.OtherSourcesWallet.Balance
                        + x.PreviosWorkPaymentWallet.Balance
                        + x.BankPercentWallet.Balance
                        + x.RentWallet.Balance,
                    Balance = x.DebtTotal - x.CreditTotal,
                })
                .ToList()
                .ToDictionary(x => x.RoId);

            var currentLoans = this.RealityObjectLoanDomain.GetAll()
                .Where(x => roIds.Contains(x.LoanTaker.RealityObject.Id))
                .Where(x => x.Operations.Any())
                .Select(
                    x => new
                    {
                        x.LoanTaker.RealityObject.Id,
                        LoanSum = x.LoanSum - x.LoanReturnedSum
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.LoanSum));


            foreach (var obj in objects)
            {
                if (!currentBalances.ContainsKey(obj.Id))
                    continue;

                var balances = currentBalances[obj.Id];

                obj.OwnerSum = balances.OwnerBalance;
                obj.SubsidySum = balances.SubsidyBalance;
                obj.OtherSum = balances.OtherSourceBalance;
                obj.CurrentBalance = balances.Balance;
                obj.OwnerLoanSum = currentLoans.Get(obj.Id);
            }
        }

        /// <summary>
        /// заполнить собираемость и информацию о задачах, если имеется
        /// </summary>
        protected virtual void FillCollection(RealtyObjectNeedLoan[] objects)
        {
            var roIds = objects.Select(x => x.Id).Distinct().ToArray();

            var collections = this.RealityObjectChargeAccountDomain.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ChargeTotal = x.Operations.Sum(y => y.ChargedTotal),
                    x.PaidTotal
                })
                .ToDictionary(x => x.RoId, x => x.ChargeTotal > 0 ? x.PaidTotal / x.ChargeTotal : 0m);

            var tasks = this.RealityObjectLoanTaskDomain.GetAll()
                .WhereContains(x => x.RealityObject.Id, roIds)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.First().Task);

            foreach (var obj in objects)
            {
                obj.Collection = collections.Get(obj.Id).RegopRoundDecimal(2);
                
                TaskEntry task;
                if (tasks.TryGetValue(obj.Id, out task))
                {
                    obj.Task = new TaskInfo
                    {
                        Id = task.Id,
                        ParentTaskId = task.Parent.Id,
                        Status = task.Status
                    };
                }
            }
        }
    }
}