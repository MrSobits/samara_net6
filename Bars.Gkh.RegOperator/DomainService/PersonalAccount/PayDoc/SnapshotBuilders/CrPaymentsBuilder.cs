namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;

    using B4.Utils;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using NHibernate;
    using NHibernate.Linq;
    using System.Linq;
    
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    /// <summary>
    /// Источник ПотраченоНаКр
    /// </summary>
    public class CrPaymentsBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(CrPaymentsBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => CrPaymentsBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник потрачено на кр";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "ПотраченоНаКр";

        private Dictionary<long, decimal> crPaymentsDict;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            var roIds = mainInfo.Select(x => x.RealityObjectId)
                .Distinct()
                .ToArray();

            foreach (var roIdPart in roIds.SplitArray())
            {
                var robjectQuery = session.Query<RealityObjectPaymentAccount>()
                    .Where(x => roIdPart.Contains(x.RealityObject.Id));

                var distrOperation = session.Query<DistributionOperation>()
                    .Where(x => x.Code == DistributionCode.PerformedWorkActsDistribution);

                var moneyOperations = session.Query<MoneyOperation>()
                    .Where(x => !x.IsCancelled && x.CanceledOperation == null)
                    .Where(x => distrOperation.Any(y => y.Operation.Id == x.Id));

                docCache.Cache.RegisterDto<RealityObjectTransfer, RealityObjectCrPayments>()
                    .SetQueryBuilder(
                        r =>
                        {
                            var transferDict = r.GetAll()
                                .Where(y => moneyOperations.Any(x => x.Id == y.Operation.Id))
                                .Select(x => new { x.SourceGuid, x.Amount })
                                .AsEnumerable()
                                .GroupBy(x => x.SourceGuid)
                                .ToDictionary(x => x.Key, x => x.Sum(y => y.Amount));

                            return robjectQuery
                                .Fetch(x => x.BaseTariffPaymentWallet)
                                .Fetch(x => x.DecisionPaymentWallet)
                                .Fetch(x => x.RentWallet)
                                .Fetch(x => x.PenaltyPaymentWallet)
                                .Fetch(x => x.SocialSupportWallet)
                                .Fetch(x => x.PreviosWorkPaymentWallet)
                                .Fetch(x => x.AccumulatedFundWallet)
                                .Fetch(x => x.TargetSubsidyWallet)
                                .Fetch(x => x.FundSubsidyWallet)
                                .Fetch(x => x.RegionalSubsidyWallet)
                                .Fetch(x => x.StimulateSubsidyWallet)
                                .Fetch(x => x.OtherSourcesWallet)
                                .Fetch(x => x.BankPercentWallet)
                                .Fetch(x => x.RestructAmicableAgreementWallet)
                                .AsEnumerable()
                                .Select(x => new RealityObjectCrPayments
                                {
                                    RoId = x.RealityObject.Id,
                                    Payments = x.GetWallets()
                                        .Select(y => transferDict.Get(y.WalletGuid))
                                        .Sum().RegopRoundDecimal(2)
                                })
                                .AsQueryable();
                            });
            }    
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            this.crPaymentsDict = creator.Cache.GetCache<RealityObjectCrPayments>().GetEntities()
                .ToDictionary(x => x.RoId, x => x.Payments);            
        }

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        public override void FillRecord(
            SnapshotCreator creator,
            InvoiceInfo record,
            PersonalAccountPaymentDocProxy account)
        {
            record.ПотраченоНаКР = this.crPaymentsDict.Get(account.RoId);
        }
    }

    internal class RealityObjectCrPayments
    {
        public long RoId { get; set; }
        public decimal Payments { get; set; }
    }
}