namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class RestructDebtViewModel : BaseViewModel<RestructDebt>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<RestructDebt> domainService, BaseParams baseParams)
        {
            var scheduleDomain = this.Container.ResolveDomain<RestructDebtSchedule>();

            using (this.Container.Using(scheduleDomain))
            {
                var id = baseParams.Params.GetAsId();

                var sum = scheduleDomain.GetAll()
                    .Where(x => x.RestructDebt.Id == id)
                    .Select(x => new
                    {
                        x.RestructDebt.Id,
                        x.PaymentSum,
                        x.PlanedPaymentSum,
                        x.PlanedPaymentDate,
                        x.PaymentDate,
                        x.IsExpired
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .Select(x => new
                    {
                        PaymentSum = x.SafeSum(s => s.PaymentSum),
                        PlanedPaymentSum = x.SafeSum(s => s.PlanedPaymentSum),
                        Status = x.Any(s => s.IsExpired) 
                            ? RestructDebtStatus.Expired
                            : x.Any(s => !s.PaymentDate.HasValue)
                                ? RestructDebtStatus.NotPaid
                                : RestructDebtStatus.Paid
                    })
                    .FirstOrDefault();

                var paidDebtSum = sum?.PaymentSum ?? 0;
                var notPaidDebtSum = (sum?.PlanedPaymentSum ?? 0) - paidDebtSum;
                var status = sum?.Status ?? RestructDebtStatus.NotPaid;

                var result = domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.DocumentDate,
                        x.DocFile,
                        x.PaymentScheduleFile,
                        x.BaseTariffDebtSum,
                        x.DecisionTariffDebtSum,
                        x.DebtSum,
                        x.PenaltyDebtSum,
                        x.RestructSum,
                        Status = status,
                        x.Reason,

                        // Расторжение договора
                        x.DocumentState,
                        PaidDebtSum = paidDebtSum,
                        NotPaidDebtSum = notPaidDebtSum,
                        x.TerminationDate,
                        x.TerminationNumber,
                        x.TerminationFile,
                        x.TerminationReason
                    })
                    .FirstOrDefault();

                return new BaseDataResult(result);
            }
        }
    }
}