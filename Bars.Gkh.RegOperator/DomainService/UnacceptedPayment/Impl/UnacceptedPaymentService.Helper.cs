namespace Bars.Gkh.RegOperator.DomainService.Impl
{
	using System;
	using System.Linq;
    using B4;
    using B4.Utils;
    using B4.DataAccess;

	using Bars.Gkh.Repositories.ChargePeriod;

	using Domain.Repository;
    using Entities;
    using Enums;
    using DomainModelServices;
    using Gkh.Enums;

    public partial class UnacceptedPaymentService
    {
        public IDomainService<BankDocumentImport> BankDocumentDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IPersonalAccountPaymentCommandFactory PersonalAccountPaymentCommandFactory { get; set; }
        public IDomainService<UnacceptedPaymentPacket> UnacceptedPaymentPacketDomain { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        
        private AmountDistributionType GetDistributionType(UnacceptedPaymentPacket packet)
        {
            if (!packet.BankDocumentId.HasValue) return AmountDistributionType.Tariff;

            var distrPenalty = BankDocumentDomain.GetAll()
                .Where(x => x.Id == packet.BankDocumentId.Value)
                .Select(x => (YesNo?) x.DistributePenalty)
                .FirstOrDefault();

            return distrPenalty.HasValue && distrPenalty.Value == YesNo.Yes
                ? AmountDistributionType.TariffAndPenalty
                : AmountDistributionType.Tariff;
        }

        protected IDataResult RemovePayments(IQueryable<UnacceptedPaymentPacket> paymentQuery)
        {
            if (!paymentQuery.Any())
            {
                return new BaseDataResult(false, "Отсутствуют оплаты для удаления.");
            }

            if (paymentQuery.Any(x => x.State != PaymentOrChargePacketState.Pending))
            {
                return new BaseDataResult(false, "Нельзя удалить подтвержденные оплаты");
            }

            var packetRepo = Container.ResolveDomain<UnacceptedPaymentPacket>();
            
            foreach (var paymentPacket in paymentQuery)
            {
                packetRepo.Delete(paymentPacket.Id);
            }

            return new BaseDataResult();
        }

        protected IDataResult CancelPayments(IQueryable<UnacceptedPayment> paymentQuery)
        {
            if (!paymentQuery.Any())
            {
                return new BaseDataResult(false, "Отсутствуют оплаты для отмены.");
            }

            if (paymentQuery.Any(x => x.Packet.State != PaymentOrChargePacketState.Accepted))
            {
                return new BaseDataResult(false, "Нельзя отменить неподтвержденные записи");
            }

            var packets = paymentQuery.GroupBy(x => x.Packet);

            var packetQuery = paymentQuery.Select(x => x.Packet);

            var operations =
                MoneyOperationRepo
                    .GetOperationsByOriginator(packetQuery)
                    .Where(x => x.CanceledOperation == null)
                    .AsEnumerable()
                    .ToDictionary(x => x.OriginatorGuid);

            var period = ChargePeriodRepo.GetCurrentPeriod();

            foreach (var packet in packets)
            {
                var applyOperation = operations.Get(packet.Key.TransferGuid);

                var cancelOperation = applyOperation.Cancel(period);
                cancelOperation.Reason = "Отмена подтверждения оплаты";

                foreach (var unaccPays in packet.Where(x => x.Accepted).GroupBy(x => x.PersonalAccount))
                {
                    var command = PersonalAccountPaymentCommandFactory.GetCommand(unaccPays.First().PaymentType);

                    var transfers = unaccPays.Key.UndoPayment(command, period, cancelOperation, DateTime.Now);

                    transfers.ForEach(TransferDomain.Save);
                    
                    unaccPays.ForEach(x =>
                    {
                        x.Accepted = false;
                        UnaccPaymentDomain.Update(x);
                    });

                    var summary = unaccPays.Key.GetOpenedPeriodSummary();
                    this.PersonalAccountPeriodSummaryDomain.Update(summary);
                }

                var packetEntity = packet.Key;

                packetEntity.Cancel();

                UnacceptedPaymentPacketDomain.Update(packetEntity);
            }

            return new BaseDataResult();
        }
    }
}