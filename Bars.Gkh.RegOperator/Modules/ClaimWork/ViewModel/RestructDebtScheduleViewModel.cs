namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System.Linq;

    using B4;

    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    public class RestructDebtScheduleViewModel : BaseViewModel<RestructDebtSchedule>
    {
        public override IDataResult List(IDomainService<RestructDebtSchedule> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("restructDebtId");

            return domainService.GetAll()
                .Where(x => x.RestructDebt.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccount.PersonalAccountNum,
                    x.TotalDebtSum,
                    x.PlanedPaymentDate,
                    x.PlanedPaymentSum,
                    x.PaymentDate,
                    x.PaymentSum
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}