namespace Bars.GkhRf.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class PaymentItemViewModel : BaseViewModel<PaymentItem>
    {
        public override IDataResult List(IDomainService<PaymentItem> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var typePayment = baseParams.Params.ContainsKey("typePayment")
                ? baseParams.Params["typePayment"].ToStr()
                : string.Empty;

            var paymentId = baseParams.Params.ContainsKey("paymentId")
                ? baseParams.Params["paymentId"].ToLong()
                : 0;

            var data = domain.GetAll()
                .WhereIf(!string.IsNullOrEmpty(typePayment), x => x.TypePayment == typePayment.To<TypePayment>())
                .WhereIf(paymentId > 0, x => x.Payment.Id == paymentId)
                .Select(x => new
                    {
                        x.Id,
                        ManagingOrganizationName = x.ManagingOrganization.Contragent.Name,
                        x.TypePayment,
                        x.ChargeDate,
                        x.IncomeBalance,
                        x.OutgoingBalance,
                        x.ChargePopulation,
                        x.PaidPopulation,
                        x.Recalculation,
                        x.TotalArea
                    })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}