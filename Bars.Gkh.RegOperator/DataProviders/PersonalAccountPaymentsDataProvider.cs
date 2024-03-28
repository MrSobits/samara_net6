namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    /// <summary>
    /// Провадйер для отчета по оплатам по личевому счету
    /// </summary>
    public class PersonalAccountPaymentsDataProvider : BaseCollectionDataProvider<PersonalAccountPaymentsInfo>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public PersonalAccountPaymentsDataProvider(IWindsorContainer container)
            : base(container)

        {
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns> PersonalAccountPaymentsInfo </returns>
        protected override IQueryable<PersonalAccountPaymentsInfo> GetDataInternal(BaseParams baseParams)
        {
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var transferDomain = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>();
            var moneyOperationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var bankDocImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var bankAccStDomain = this.Container.ResolveDomain<BankAccountStatement>();

            try
            {
                var persAcc = personalAccountDomain.Get(this.AccountId);

                var wallets = persAcc.GetMainWallets()
                    .ToDictionary(x => x.WalletGuid, x => x.WalletType);

                var transferQuery = transferDomain.GetAll()
                    .Where(x => x.Owner == persAcc && (wallets.Keys.Contains(x.SourceGuid) || wallets.Keys.Contains(x.TargetGuid)))
                    .Where(x => !moneyOperationDomain.GetAll().Any(y => y.CanceledOperation.Id == x.Operation.Id) && x.Operation.CanceledOperation == null);

                var bankDocumentImportDict = bankDocImportDomain.GetAll()
                    .Where(x => transferQuery.Any(y => y.Operation.OriginatorGuid == x.TransferGuid))
                    .Select(x => new
                    {
                        x.TransferGuid,
                        x.PaymentAgentName
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TransferGuid, x => x.PaymentAgentName) // на некоторых регионах есть дубли по TransferGuid
                    .ToDictionary(x => x.Key, x => x.First());

                var bankAccStInfoDict = bankAccStDomain
                    .GetAll()
                    .Where(x => transferQuery.Any(y => y.Operation.OriginatorGuid == x.TransferGuid))
                    .Select(x => new
                    {
                        x.TransferGuid,
                        x.Payer.Name
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.TransferGuid, y => y.Name);

                return transferQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.Operation.OriginatorGuid,
                        x.SourceGuid,
                        x.TargetGuid,
                        x.PaymentDate,
                        x.Amount,
                        Reason = x.Reason ?? x.Operation.Reason,
                        PeriodId = x.ChargePeriod.Id
                    })
                    .ToList()
                    .Select(x =>
                    {
                        var isInTransfer = wallets.Keys.Contains(x.TargetGuid);
                        var amount = isInTransfer ? x.Amount : -x.Amount;
                        var walletType = isInTransfer ? wallets[x.TargetGuid] : wallets[x.SourceGuid];

                        return new PersonalAccountPaymentsInfo
                        {
                            Дата = x.PaymentDate,
                            ТипОплаты = x.Reason,
                            Сумма = amount,
                            КредитнаяОрганизация = bankDocumentImportDict.Get(x.OriginatorGuid) ?? bankAccStInfoDict.Get(x.OriginatorGuid),
                            PeriodId = x.PeriodId,
                            WalletType = (int)walletType
                        };
                    })
                    .AsQueryable();
            }
            finally
            {
                this.Container.Release(transferDomain);
                this.Container.Release(bankAccStDomain);
                this.Container.Release(bankDocImportDomain);
                this.Container.Release(personalAccountDomain);
            }
        }

        /// <summary>
        /// Наименование провайдера
        /// </summary>
        public override string Name
        {
            get { return "Информация по оплатам"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountPaymentsDataProvider).Name; }
        }

        /// <summary>
        /// Описание провайдера
        /// </summary>
        public override string Description
        {
            get { return this.Name; }
        }

        /// <summary>
        /// Id лс
        /// </summary>
        public long AccountId { get; set; }
    }
}