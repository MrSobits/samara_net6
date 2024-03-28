namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class UnconfirmedPaymentsViewModel : BaseViewModel<UnconfirmedPayments>
    {
        public override IDataResult List(IDomainService<UnconfirmedPayments> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                        x.Sum,
                        x.Guid,
                        x.PaymentDate,
                        x.Description,
                        x.BankName,
                        x.BankBik,
                        x.IsConfirmed,
                        x.File
                    })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}