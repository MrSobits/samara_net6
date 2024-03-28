namespace Bars.Gkh.RegOperator.Tasks.UnacceptedPayment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;
    using Domain;
    using Domain.ValueObjects;
    using DomainModelServices;
    using DomainModelServices.PersonalAccount;
    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain;
    using Gkh.Enums;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class UnacceptedPaymentTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<UnacceptedPaymentPacket> UnacceptedPaymentPacketDomain { get; set; }
        public IDomainService<UnacceptedPayment> UnacceptedPaymentDomain { get; set; }
        public IWindsorContainer Container { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        public IDomainService<BankDocumentImport> BankDocumentDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IPersonalAccountPaymentCommandFactory CommandFactory { get; set; }
        public IDomainService<Transfer> TransferDomain { get; set; }
        public IRealtyObjectPaymentSession PaymentSession { get; set; }
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        public IDomainService<Wallet> WalletDomain { get; set; }

        #region Implementation of ITaskExecutor

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var packetIds = @params.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            IDataResult result = new BaseDataResult();

            try
            {
                if (packetIds.IsEmpty())
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи для подтверждения");
                }

                Container.InTransaction(() =>
                {
                    var payments =
                        UnacceptedPaymentDomain.GetAll()
                            .WhereIf(packetIds.IsNotEmpty(), x => packetIds.Contains(x.Packet.Id))
                            .Where(x => !x.Accepted)
                            .ToList();

                    result = AcceptPayments(payments);

                    RecalcEventManager.SaveEvents();
                });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            catch (InvalidOperationException e)
            {
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(" message: {0} {1}\r\n stacktrace: {2}".FormatUsing(e.Message, e.InnerException, e.StackTrace));
            }
            finally
            {
                using (this.Container.BeginScope())
                {
                    CleanUp(packetIds);
                }
            }

            return new BaseDataResult(true, result.Message);
        }

        public string ExecutorCode { get; private set; }

        #endregion

        private void CleanUp(IEnumerable<long> packetIds)
        {
            var packets = UnacceptedPaymentPacketDomain.GetAll()
                .Where(x => packetIds.Contains(x.Id))
                .Where(x => x.State == PaymentOrChargePacketState.InProgress)
                .ToArray();

            if (packets.Any())
            {
                packets.ForEach(x =>
                {
                    x.Payments.ForEach(z =>
                    {
                        var wallets = z.PersonalAccount.GetWallets();

                        foreach (var wallet in wallets.Where(y => y.Id == 0))
                        {
                            WalletDomain.Save(wallet);
                        }

                        z.Accepted = false;
                    });

                    x.UnacceptedPayments.ForEach(z =>
                    {
                        var wallets = z.PersonalAccount.GetWallets();

                        foreach (var wallet in wallets.Where(y => y.Id == 0))
                        {
                            WalletDomain.Save(wallet);
                        }
                    });

                    x.Cancel();
                    UnacceptedPaymentPacketDomain.Update(x);
                });
            }
        }

        protected virtual IDataResult AcceptPayments(List<UnacceptedPayment> payments)
        {
            if (!payments.Any())
            {
                return new BaseDataResult(false, "Отсутствуют оплаты для подтверждения.");
            }

            var stringMessages = new StringBuilder();

            decimal totalSum = 0;
            var personAccCount = 0;

            var chargePeriod = ChargePeriodRepo.GetCurrentPeriod();

            if (chargePeriod == null)
            {
                return new BaseDataResult(false, "Нет открытого периода");
            }

            //выводим сообщение если статус уже Подтвержден
            if (payments.Any(x => x.Packet.State == PaymentOrChargePacketState.Accepted))
            {
                return new BaseDataResult(false, "Запись уже подтверждена, невозможно подтвердить запись дважды.");
            }

            var packets = payments.GroupBy(x => x.Packet);

            var listFailedPayments = new StringBuilder();

            foreach (var packet in packets)
            {
                var operation = packet.Key.CreateOperation(chargePeriod);
                MoneyOperationDomain.Save(operation);

                operation.Reason = "Подтверждение оплаты";

                var amountDistrType = GetDistributionType(packet.Key);

                foreach (var payment in packet.Where(x => !x.Accepted))
                {
                    decimal sum = 0m;

                    switch (payment.PaymentType)
                    {
                        case PaymentType.Basic:
                            sum = payment.Sum;
                            break;
                        case PaymentType.SocialSupport:
                            sum = payment.Sum;
                            break;
                        case PaymentType.Penalty:
                            sum = payment.PenaltySum ?? 0;
                            break;
                    }

                    totalSum += sum;

                    var command = CommandFactory.GetCommand(payment.PaymentType);
                    var transfers = payment.PersonalAccount.ApplyPayment(command,
                        new MoneyStream(
                            packet.Key,
                            operation,
                            payment.PaymentDate,
                            sum)
                        {
                            Description = "Подтверждение оплаты"
                        }, amountDistrType);

                    personAccCount += 1;

                    payment.Accepted = true;

                    UnacceptedPaymentDomain.Update(payment);

                    var summary = payment.PersonalAccount.GetOpenedPeriodSummary();
                    this.PersonalAccountPeriodSummaryDomain.Update(summary);

                    transfers.ForEach(TransferDomain.Save);
                }

                var unaccpacket = packet.Key;
                unaccpacket.Accept();
                UnacceptedPaymentPacketDomain.Update(unaccpacket);

                PaymentSession.Complete();
            }

            if (personAccCount == 1)
            {
                stringMessages.AppendLine(
                    "Оплата на сумму {0} по одному лицевому счету подтверждена. <br>".FormatUsing(
                        totalSum.RegopRoundDecimal(2)));
            }
            else
            {
                stringMessages.AppendLine(
                    "Оплата на сумму {0} по {1} лицевым счетам подтверждена. <br>".FormatUsing(
                        totalSum.RegopRoundDecimal(2), personAccCount));
            }

            return new BaseDataResult(true, stringMessages + listFailedPayments.ToString());
        }

        private AmountDistributionType GetDistributionType(UnacceptedPaymentPacket packet)
        {
            if (!packet.BankDocumentId.HasValue) return AmountDistributionType.Tariff;

            var distrPenalty = BankDocumentDomain.GetAll()
                .Where(x => x.Id == packet.BankDocumentId.Value)
                .Select(x => (YesNo?)x.DistributePenalty)
                .FirstOrDefault();

            return distrPenalty.HasValue && distrPenalty.Value == YesNo.Yes
                ? AmountDistributionType.TariffAndPenalty
                : AmountDistributionType.Tariff;
        }
    }
}