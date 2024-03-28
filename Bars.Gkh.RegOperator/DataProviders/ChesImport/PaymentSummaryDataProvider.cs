namespace Bars.Gkh.RegOperator.DataProviders.ChesImport
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сведения по оплатам оплатам ЧЭС
    /// </summary>
    public class PaymentSummaryDataProvider : BaseCollectionDataProvider<PaymentSummaryMeta>
    {
        private readonly IChesImportService service;

        /// <inheritdoc />
        public PaymentSummaryDataProvider(IWindsorContainer container, IChesImportService service)
            : base(container)
        {
            this.service = service;
        }

        /// <inheritdoc />
        protected override IQueryable<PaymentSummaryMeta> GetDataInternal(BaseParams baseParams)
        {
            var paymentProxy = ((List<ChesImportService.PaymentSummaryProxy>)this.service.ListPaymentInfo(baseParams).Data)
                .ToDictionary(x => x.WalletType);

            return new[]
            {
                new PaymentSummaryMeta
                {
                    ОплаченоПоБазовомуТарифу = paymentProxy[WalletType.BaseTariffWallet].Paid,
                    ОплаченоПени = paymentProxy[WalletType.PenaltyWallet].Paid,
                    ОплаченоПоТарифуРешения = paymentProxy[WalletType.DecisionTariffWallet].Paid,

                    ОтменаОплатПоБазовомуТарифу = paymentProxy[WalletType.BaseTariffWallet].Cancelled,
                    ОтменаОплатПени = paymentProxy[WalletType.PenaltyWallet].Cancelled,
                    ОтменаОплатПоТарифуРешения = paymentProxy[WalletType.DecisionTariffWallet].Cancelled,

                    ВозвратОплатПоБазовомуТарифу = paymentProxy[WalletType.BaseTariffWallet].Refund,
                    ВозвратОплатПени = paymentProxy[WalletType.PenaltyWallet].Refund,
                    ВозвратОплатПоТарифуРешения = paymentProxy[WalletType.DecisionTariffWallet].Refund,
                }
            }.AsQueryable();
        }

        /// <inheritdoc />
        public override string Name => "Сведения по оплатам импорта ЧЭС";

        /// <inheritdoc />
        public override string Description => this.Name;
    }
}