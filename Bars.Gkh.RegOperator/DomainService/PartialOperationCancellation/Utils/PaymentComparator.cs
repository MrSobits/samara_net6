namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;

    using NHibernate.Util;

    /// <summary>
    /// Утилита по сопоставлению оплат и трансферов
    /// </summary>
    public class PaymentComparator
    {
        /// <summary>
        /// Метод пытается сопоставит оплату с её источником
        /// </summary>
        /// <typeparam name="TTransfer">Тип трансфера</typeparam>
        /// <typeparam name="TOwner">Тип владельца трансфера</typeparam>
        /// <typeparam name="TPayment">Тип оплат</typeparam>
        /// <param name="owner">Владелец трансферов</param>
        /// <param name="payments">Оплаты из реестра</param>
        /// <param name="transfers">Трансферы этих оплат</param>
        /// <param name="ignoreDate">Параметр, который указывает, что не нужно сверять по датам, а просто "раскидать" оплаты по суммам. Нужно для БО</param>
        /// <returns>Сопоставленные оплаты</returns>
        public static IDictionary<TPayment, TTransfer[]> Compare<TTransfer, TOwner, TPayment>(
           TOwner owner,
           IEnumerable<TPayment> payments,
           IEnumerable<TTransfer> transfers,
           bool ignoreDate = false)
               where TTransfer : TransferWithOwner<TOwner>
               where TOwner : class, ITransferOwner
               where TPayment : ICancelablePayment
        {
            if (transfers.Any(x => x.Owner != owner))
            {
                throw new InvalidOperationException("В коллекции присутствуют трансферы другого владельца");
            }

            return new PaymentComparator().CompareInternal(owner, payments, transfers, ignoreDate)
                .ToDictionary(x => x.Key, x => x.Value.Cast<TTransfer>().ToArray());
        }

        /// <summary>
        /// Метод пытается сопоставит оплату с её источником
        /// </summary>
        /// <typeparam name="TPayment"></typeparam>
        /// <param name="owner">Владелец трансферов</param>
        /// <param name="payments">Оплаты из реестра</param>
        /// <param name="transfers">Трансферы этих оплат</param>
        /// <param name="ignoreDate">Параметр, который указывает, что не нужно сверять по датам, а просто "раскидать" оплаты по суммам. Нужно для БО</param>
        /// <returns>Сопоставленные оплаты</returns>
        public static IDictionary<TPayment, Transfer[]> Compare<TPayment>(
            ITransferOwner owner,
            IEnumerable<TPayment> payments,
            IEnumerable<Transfer> transfers,
            bool ignoreDate = false) 
                where TPayment : ICancelablePayment
        {
            if (transfers.Any(x => x.Owner != owner))
            {
                throw new InvalidOperationException("В коллекции присутствуют трансферы другого владельца");
            }

            return new PaymentComparator().CompareInternal(owner, payments, transfers, ignoreDate);
        }

        /// <summary>
        /// Метод пытается сопоставит оплату с её источником
        /// </summary>
        /// <param name="owner">Владелец трансферов</param>
        /// <param name="payments">Оплаты из реестра</param>
        /// <param name="transfers">Трансферы этих оплат</param>
        /// <param name="ignoreDate">Параметр, который указывает, что не нужно сверять по датам, а просто "раскидать" оплаты по суммам. Нужно для БО</param>
        /// <returns>Сопоставленные оплаты</returns>
        public static IDictionary<ICancelablePayment, Transfer[]> Compare(
            ITransferOwner owner,
            IEnumerable<ICancelablePayment> payments,
            IEnumerable<Transfer> transfers,
            bool ignoreDate = false)
        {
            if (transfers.Any(x => x.Owner != owner))
            {
                throw new InvalidOperationException("В коллекции присутствуют трансферы другого владельца");
            }

            return new PaymentComparator().CompareInternal(owner, payments, transfers, ignoreDate);
        }

        private PaymentComparator()
        {
        }

        private IDictionary<TPayment, Transfer[]> CompareInternal<TPayment>(
            ITransferOwner owner,
            IEnumerable<TPayment> payments,
            IEnumerable<Transfer> transfers,
            bool ignoreDate)
               where TPayment : ICancelablePayment
        {
            var walletDict = new Dictionary<WalletType, Wallet>
            {
                { WalletType.BaseTariffWallet, owner.GetWalletByType(WalletType.BaseTariffWallet) },
                { WalletType.DecisionTariffWallet, owner.GetWalletByType(WalletType.DecisionTariffWallet) },
                { WalletType.PenaltyWallet, owner.GetWalletByType(WalletType.PenaltyWallet) },
                { WalletType.RentWallet, owner.GetWalletByType(WalletType.RentWallet) },
                { WalletType.SocialSupportWallet, owner.GetWalletByType(WalletType.SocialSupportWallet) },
                { WalletType.PreviosWorkPaymentWallet, owner.GetWalletByType(WalletType.PreviosWorkPaymentWallet) },
                { WalletType.AccumulatedFundWallet, owner.GetWalletByType(WalletType.AccumulatedFundWallet) },
                { WalletType.RestructAmicableAgreementWallet, owner.GetWalletByType(WalletType.RestructAmicableAgreementWallet) },
                { WalletType.TargetSubsidyWallet, owner.GetWalletByType(WalletType.TargetSubsidyWallet) },
                { WalletType.FundSubsidyWallet, owner.GetWalletByType(WalletType.FundSubsidyWallet) },
                { WalletType.RegionalSubsidyWallet, owner.GetWalletByType(WalletType.RegionalSubsidyWallet) },
                { WalletType.StimulateSubsidyWallet, owner.GetWalletByType(WalletType.StimulateSubsidyWallet) },
                { WalletType.OtherSourcesWallet, owner.GetWalletByType(WalletType.OtherSourcesWallet) },
                { WalletType.BankPercentWallet, owner.GetWalletByType(WalletType.BankPercentWallet) },
            };

            return ignoreDate 
                ? this.DistributedPayments(payments, transfers, walletDict)
                : this.DistributedPaymentsByDate(payments, transfers, walletDict);
        }

        private IDictionary<TPayment, Transfer[]> DistributedPaymentsByDate<TPayment>(IEnumerable<TPayment> payments, IEnumerable<Transfer> transfers, IDictionary<WalletType, Wallet> walletDict)
            where TPayment : ICancelablePayment
        {
            var paymentsByDate = payments.GroupBy(x => x.PaymentDate).ToDictionary(x => x.Key);

            var transferByDateAndType = transfers
                .GroupBy(x => x.PaymentDate)
                .ToDictionary(
                    x => x.Key,
                    y => y.ToList());

            var distributedPayments = new Dictionary<TPayment, Transfer[]>();
            foreach (var transfersByType in transferByDateAndType)
            {
                var sources = paymentsByDate.Get(transfersByType.Key);
                if (sources.IsEmpty())
                {
                    throw new ValidationException("Не найдена оплата на указанную дату");
                }

                distributedPayments.AddOrOverride(this.DistributedPayments(sources, transfersByType.Value, walletDict));
            }

            return distributedPayments;
        }

        private IDictionary<TPayment, Transfer[]> DistributedPayments<TPayment>(IEnumerable<TPayment> payments, IEnumerable<Transfer> transfers, IDictionary<WalletType, Wallet> walletDict)
            where TPayment : ICancelablePayment
        {
            var transferByDateAndType = transfers
                .GroupBy(x => PaymentComparator.GetWalletType(x, walletDict))
                .ToDictionary(x => x.Key, x => x.ToList());

            var distributedPayments = new Dictionary<TPayment, Transfer[]>();
            foreach (var cancelablePayment in payments)
            {
                Transfer[] result;
                if (this.TryComparePayments(cancelablePayment, transferByDateAndType, walletDict, out result))
                {
                    distributedPayments.Add(cancelablePayment, result);
                }
            }

            return distributedPayments;
        }

        private bool TryComparePayments(
            ICancelablePayment source,
            IDictionary<WalletType, List<Transfer>> transfers,
            IDictionary<WalletType, Wallet> walletDict,
            out Transfer[] result)
        {
            var btTransfersCount = Math.Max(transfers.Get(WalletType.BaseTariffWallet).AllOrEmpty().Count(), 1);
            var dtTransfersCount = Math.Max(transfers.Get(WalletType.DecisionTariffWallet).AllOrEmpty().Count(), 1);
            var pTransfersCount = Math.Max(transfers.Get(WalletType.PenaltyWallet).AllOrEmpty().Count(), 1);

            if (transfers.Get(WalletType.BaseTariffWallet) == null && transfers.Get(WalletType.DecisionTariffWallet) == null
                && transfers.Get(WalletType.PenaltyWallet) == null)
            {
                var transfer = transfers.Values
                    .SelectMany(x => x)
                    .FirstOrDefault(x => x.Amount == source.Sum);

                if (transfer != null)
                {
                    transfers.Get(PaymentComparator.GetWalletType(transfer, walletDict)).Remove(transfer);
                    result = new[] {transfer};
                    return true;
                }
            }
            else
            {
                for (var btIndex = 0; btIndex < btTransfersCount; btIndex++)
                {
                    for (var dtIndex = 0; dtIndex < dtTransfersCount; dtIndex++)
                    {
                        for (var pIndex = 0; pIndex < pTransfersCount; pIndex++)
                        {
                            var btTransfer = transfers.Get(WalletType.BaseTariffWallet)?.ElementAtOrDefault(btIndex);
                            var dtTransfer = transfers.Get(WalletType.DecisionTariffWallet)?.ElementAtOrDefault(btIndex);
                            var pTransfer = transfers.Get(WalletType.PenaltyWallet)?.ElementAtOrDefault(btIndex);

                            var sum = (btTransfer?.Amount ?? 0m) + (dtTransfer?.Amount ?? 0m) + (pTransfer?.Amount ?? 0m);
                            if (Math.Abs(sum - source.Sum) < 0.01m)
                            {
                                result = new[] { btTransfer, dtTransfer, pTransfer }.Where(x => x != null).ToArray();

                                transfers.Get(WalletType.BaseTariffWallet)?.Remove(btTransfer);
                                transfers.Get(WalletType.DecisionTariffWallet)?.Remove(dtTransfer);
                                transfers.Get(WalletType.PenaltyWallet)?.Remove(pTransfer);

                                return true;
                            }
                        }
                    }
                }
            }
            
            result = null;
            return false;
        }

        private static WalletType GetWalletType(Transfer transfer, IDictionary<WalletType, Wallet> walletDict)
        {
            return walletDict.First(x => transfer.TargetGuid == x.Value.WalletGuid || transfer.SourceGuid == x.Value.WalletGuid).Key;
        }
    }
}