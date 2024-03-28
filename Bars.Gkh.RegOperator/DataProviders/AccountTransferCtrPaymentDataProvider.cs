namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Оплаты подрядчикам"
    /// </summary>
    public class AccountTransferCtrPaymentDataProvider : BaseCollectionDataProvider<AccountTransferCtrPaymentMeta>
    {
        /// <inheritdoc />
        public AccountTransferCtrPaymentDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<AccountTransferCtrPaymentMeta> GetDataInternal(BaseParams baseParams)
        {
            var roIds = baseParams.Params.GetAs<string>("roIds").ToLongArray();
            if (roIds.IsEmpty())
            {
                throw new ValidationException("Параметр \"Дом\" обязателен для заполнения");
            }
            
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var roPaymentAccountDomain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();
            var realityObjectTransferDomain = this.Container.ResolveDomain<RealityObjectTransfer>();
            var accountTransferDomain = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>();
            var distributionOperationDomain = this.Container.ResolveDomain<DistributionOperation>();
            var walletDomain = this.Container.ResolveDomain<Wallet>();

            using (this.Container.Using(
                accountDomain, roPaymentAccountDomain, realityObjectTransferDomain, distributionOperationDomain, accountTransferDomain, walletDomain))
            {
                var accountQuery = accountDomain.GetAll().WhereContains(x => x.Room.RealityObject.Id, roIds);

                var realityObjectInfo = roPaymentAccountDomain.GetAll()
                    .WhereContains(x => x.RealityObject.Id, roIds)
                    .Select(x => new
                    {
                        Id = x.Id,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,

                        BaseTariffBalance = x.BaseTariffPaymentWallet.Balance,
                        DecisionTariffBalance = x.DecisionPaymentWallet.Balance,
                        PenaltyAccountPayments = x.PenaltyPaymentWallet.Balance,

                        BaseTariffGuid = x.BaseTariffPaymentWallet.WalletGuid,
                        DecisionTariffGuid = x.DecisionPaymentWallet.WalletGuid,
                        PenaltyGuid = x.PenaltyPaymentWallet.WalletGuid,
                    })
                    .ToDictionary(x => x.RoId);

                var distributionQuery = distributionOperationDomain.GetAll()
                    .Where(x => x.Code == DistributionCode.TransferContractorDistribution ||
                           x.Code == DistributionCode.BuildControlPaymentDistribution ||
                           x.Code == DistributionCode.DEDPaymentDistribution);

                var walletTypes = new[] { WalletType.BaseTariffWallet, WalletType.DecisionTariffWallet, WalletType.PenaltyWallet };

                var realityObjectTransferCtrPayments = realityObjectTransferDomain.GetAll()
                    .WhereContains(x => x.Owner.Id, realityObjectInfo.Values.Select(x => x.Id).ToArray())
                    .Where(x => x.IsAffect)
                    .Where(x => distributionQuery.Any(y => y.Operation == x.Operation))
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .Where(x => walletDomain.GetAll().Any(y => walletTypes.Contains(y.WalletType) && y.WalletGuid == x.SourceGuid))
                    .GroupBy(x => x.SourceGuid)
                    .Select(x => new
                    {
                        x.Key,
                        Sum = x.Sum(y => y.Amount)
                    })
                    .ToDictionary(x => x.Key, x => x.Sum);

                // берем оплаты дома (копии с ЛС)
                var realityObjectAccountPayments = realityObjectTransferDomain.GetAll()
                    .WhereContains(x => x.Owner.Id, realityObjectInfo.Values.Select(x => x.Id).ToArray())
                    .Where(x => x.IsAffect && x.CopyTransfer != null)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .Where(x => walletDomain.GetAll().Any(y => walletTypes.Contains(y.WalletType) && y.WalletGuid == x.TargetGuid))
                    .GroupBy(x => x.TargetGuid)
                    .Select(x => new
                    {
                        x.Key,
                        Sum = x.Sum(y => y.Amount)
                    })
                    .ToDictionary(x => x.Key, x => x.Sum);

                // вычитаем расходы
                realityObjectTransferDomain.GetAll()
                    .WhereContains(x => x.Owner.Id, realityObjectInfo.Values.Select(x => x.Id).ToArray())
                    .Where(x => x.IsAffect)
                    .Where(x => x.CopyTransfer != null)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .Where(x => walletDomain.GetAll().Any(y => walletTypes.Contains(y.WalletType) && y.WalletGuid == x.SourceGuid))
                    .GroupBy(x => x.SourceGuid)
                    .Select(x => new
                    {
                        x.Key,
                        Sum = x.Sum(y => y.Amount)
                    })
                    .ForEach(x => realityObjectAccountPayments[x.Key] = realityObjectAccountPayments.Get(x.Key) - x.Sum);

                // берем оплаты ЛС + слияния
                var accountPayments = accountTransferDomain.GetAll()
                    .Where(x => accountQuery.Any(y => y == x.Owner))
                    .Where(x => x.IsAffect)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .Where(x => walletDomain.GetAll().Any(y => walletTypes.Contains(y.WalletType) && y.WalletGuid == x.TargetGuid))
                    .GroupBy(x => x.TargetGuid)
                    .Select(x => new
                    {
                        x.Key,
                        Sum = x.Sum(y => y.TargetCoef * y.Amount)
                    })
                    .ToDictionary(x => x.Key, x => x.Sum);

                // вычитаем расходы + слияния
                accountTransferDomain.GetAll()
                    .Where(x => accountQuery.Any(y => y == x.Owner))
                    .Where(x => x.IsAffect)
                    .Where(x => !x.Operation.IsCancelled && x.Operation.CanceledOperation == null)
                    .Where(x => walletDomain.GetAll().Any(y => walletTypes.Contains(y.WalletType) && y.WalletGuid == x.SourceGuid))
                    .GroupBy(x => x.SourceGuid)
                    .Select(x => new
                    {
                        x.Key,
                        Sum = x.Sum(y => y.TargetCoef * y.Amount)
                    })
                    .ForEach(x => accountPayments[x.Key] = accountPayments.Get(x.Key) - x.Sum);

                var resultData = accountQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.PersonalAccountNum,
                        RoId = x.Room.RealityObject.Id,
                        x.Room.RoomNum,
                        x.Room.ChamberNum,
                        AccountOwner = (x.AccountOwner as IndividualAccountOwner).Name
                                    ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name,
                        
                        BaseWalletGuid = x.BaseTariffWallet.WalletGuid,
                        DecisionWalletGuid = x.DecisionTariffWallet.WalletGuid,
                        PenaltyWalletGuid = x.PenaltyWallet.WalletGuid
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .SelectMany(accountsByRo =>
                    {
                        var realityObjectData = realityObjectInfo.Get(accountsByRo.Key);

                        var baseAccountPayments = realityObjectAccountPayments.Get(realityObjectData.BaseTariffGuid);
                        var decisionAccountPayments = realityObjectAccountPayments.Get(realityObjectData.DecisionTariffGuid);
                        var penaltyAccountPayments = realityObjectAccountPayments.Get(realityObjectData.PenaltyGuid);

                        var realityObjectPaymentInfo = new
                        {
                            BaseAccountPayments = baseAccountPayments,
                            DecisionAccountPayments = decisionAccountPayments,
                            PenaltyAccountPayments = penaltyAccountPayments,

                            BasePayments = Math.Min(baseAccountPayments, realityObjectTransferCtrPayments.Get(realityObjectData.BaseTariffGuid)),
                            DecisionPayments = Math.Min(decisionAccountPayments, realityObjectTransferCtrPayments.Get(realityObjectData.DecisionTariffGuid)),
                            PenaltyPayments = Math.Min(penaltyAccountPayments, realityObjectTransferCtrPayments.Get(realityObjectData.PenaltyGuid))
                        };

                        var accounts = accountsByRo.Select(x => new AccountTransferCtrPaymentMeta
                            {
                                ИдентификаторЛС = x.Id,
                                АдресДома = realityObjectData.Address,
                                МуниципальныйРайон = realityObjectData.Municipality,
                                НомерКвартиры = new[] { x.RoomNum, x.ChamberNum }.AggregateWithSeparator("/"),
                                НомерЛС = x.PersonalAccountNum,
                                Собственник = x.AccountOwner,

                                ОплаченоВсегоБазовый = realityObjectPaymentInfo.BaseAccountPayments,
                                ОплаченоВсегоТариф = realityObjectPaymentInfo.DecisionAccountPayments,
                                ОплаченоВсегоПени = realityObjectPaymentInfo.PenaltyAccountPayments,

                                ВсегоОплатНаЛСБазовый = accountPayments.Get(x.BaseWalletGuid),
                                ВсегоОплатНаЛСТариф = accountPayments.Get(x.DecisionWalletGuid),
                                ВсегоОплатНаЛСПени = accountPayments.Get(x.PenaltyWalletGuid)
                            })
                            .ToList();

                        accounts = Utils.MoneyAndCentDistribution(
                            accounts,
                            x => realityObjectPaymentInfo.BaseAccountPayments > 0 
                                ? x.ВсегоОплатНаЛСБазовый * realityObjectPaymentInfo.BasePayments / realityObjectPaymentInfo.BaseAccountPayments
                                : 0,
                            realityObjectPaymentInfo.BasePayments,
                            (x, y) =>
                            {
                                x.ОплаченоПодрядчикамБазовый = y;
                                return x;
                            },
                            (proxy, coin) =>
                            {
                                if (proxy.ОплаченоПодрядчикамБазовый > 0)
                                {
                                    proxy.ОплаченоПодрядчикамБазовый += coin;
                                    return true;
                                }
                                else
                                {
                                    proxy.ОплаченоПодрядчикамБазовый = 0;
                                    return false;
                                }
                            });

                        accounts = Utils.MoneyAndCentDistribution(
                            accounts,
                            x => realityObjectPaymentInfo.DecisionAccountPayments > 0
                                ? x.ВсегоОплатНаЛСТариф * realityObjectPaymentInfo.DecisionPayments / realityObjectPaymentInfo.DecisionAccountPayments
                                : 0,
                            realityObjectPaymentInfo.DecisionPayments,
                            (x, y) =>
                            {
                                x.ОплаченоПодрядчикамТариф = y;
                                return x;
                            },
                            (proxy, coin) =>
                            {
                                if (proxy.ОплаченоПодрядчикамТариф > 0)
                                {
                                    proxy.ОплаченоПодрядчикамТариф += coin;
                                    return true;
                                }
                                else
                                {
                                    proxy.ОплаченоПодрядчикамТариф = 0;
                                    return false;
                                }
                            });

                        accounts = Utils.MoneyAndCentDistribution(
                            accounts,
                            x => realityObjectData.PenaltyAccountPayments > 0 
                                ? x.ВсегоОплатНаЛСПени * realityObjectPaymentInfo.PenaltyPayments / realityObjectData.PenaltyAccountPayments
                                : 0,
                            realityObjectPaymentInfo.PenaltyPayments,
                            (x, y) =>
                            {
                                x.ОплаченоПодрядчикамПени = y;
                                return x;
                            },
                            (proxy, coin) =>
                            {
                                if (proxy.ОплаченоПодрядчикамПени > 0)
                                {
                                    proxy.ОплаченоПодрядчикамПени += coin;
                                    return true;
                                }
                                else
                                {
                                    proxy.ОплаченоПодрядчикамПени = 0;
                                    return false;
                                }
                            });

                        return accounts;
                    })
                    .ToArray();

                return resultData.AsQueryable();
            }
        }

        /// <inheritdoc />
        public override string Name => "Оплаты подрядчикам";

        /// <inheritdoc />
        public override string Description => "Отчет для просмотра сумм оплат подрядчикам в разрезе ЛС";
    }
}