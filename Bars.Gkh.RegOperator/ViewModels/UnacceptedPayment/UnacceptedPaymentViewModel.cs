namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class UnacceptedPaymentViewModel : BaseViewModel<UnacceptedPayment>
    {
        public override IDataResult List(IDomainService<UnacceptedPayment> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var packetId = loadParams.Filter.GetAs<long>("packetId");

            var data = domainService.GetAll()
                .Where(x => x.Packet.Id == packetId)
                .Select(x => new
                {
                    PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                    x.PaymentType,
                    Sum = x.PaymentType == PaymentType.Basic || x.PaymentType == PaymentType.SocialSupport
                        ? x.Sum
                        : x.PenaltySum,
                    x.PaymentDate
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
