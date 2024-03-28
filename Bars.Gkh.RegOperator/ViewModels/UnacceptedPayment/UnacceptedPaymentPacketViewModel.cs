namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;
    using Gkh.Enums;

    public class UnacceptedPaymentPacketViewModel : BaseViewModel<UnacceptedPaymentPacket>
    {
        public override IDataResult List(IDomainService<UnacceptedPaymentPacket> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var showAccepted = baseParams.Params.GetAs<bool>("showAccepted");

            var paymentService = Container.ResolveDomain<UnacceptedPayment>();
            var bankDocsDomain = Container.ResolveDomain<BankDocumentImport>();

            var query = domainService.GetAll()
                .WhereIf(!showAccepted, x => x.State != PaymentOrChargePacketState.Accepted);

            var bankDocsDistrPenalty = bankDocsDomain.GetAll()
                .Where(x => query.Any(z => z.BankDocumentId.HasValue && z.BankDocumentId.Value == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.DistributePenalty
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id, z => (YesNo?)z.DistributePenalty);

            var data = domainService.GetAll()
                .WhereIf(!showAccepted, x => x.State != PaymentOrChargePacketState.Accepted)
                .Select(x => new
                {
                    x.Id,
                    x.CreateDate,
                    Count = paymentService.GetAll().Count(y => y.Packet.Id == x.Id),
                    x.State,
                    x.Sum,
                    x.Type,
                    x.BankDocumentId
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.CreateDate,
                    Description = string.Format("{0} по {1} лицевым счетам", x.Type.GetEnumMeta().Display, x.Count),
                    x.State,
                    x.Sum,
                    DistributePenalty = bankDocsDistrPenalty.Get(x.BankDocumentId ?? 0L) ?? YesNo.No
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
        }
    }
}