namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using B4;
    using Bars.Gkh.RegOperator.Entities;

    public class SberbankPaymentDocViewModel : BaseViewModel<SberbankPaymentDoc>
    {
        public override IDataResult List(IDomainService<SberbankPaymentDoc> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Period = x.Period.Name,
                    AccNumber = x.Account.PersonalAccountNum,
                    x.LastDate,
                    x.Count,
                    x.GUID,
                    x.PaymentDocFile
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}