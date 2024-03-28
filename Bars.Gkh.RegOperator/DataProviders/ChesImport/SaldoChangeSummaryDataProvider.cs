namespace Bars.Gkh.RegOperator.DataProviders.ChesImport
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta;
    using Bars.Gkh.RegOperator.DomainService.Import;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сведения по измененю сальдо оплатам ЧЭС
    /// </summary>
    public class SaldoChangeSummaryDataProvider : BaseCollectionDataProvider<SaldoChangeSummaryMeta>
    {
        private readonly IChesImportService service;

        /// <inheritdoc />
        public SaldoChangeSummaryDataProvider(IWindsorContainer container, IChesImportService service)
            : base(container)
        {
            this.service = service;
        }

        /// <inheritdoc />
        protected override IQueryable<SaldoChangeSummaryMeta> GetDataInternal(BaseParams baseParams)
        {
            var saldoChangeSummaryProxies = ((List<ChesImportService.SaldoChangeSummaryProxy>)this.service.ListSaldoChangeInfo(baseParams).Data)
                .ToDictionary(x => x.WalletType);

            return new[]
            {
                new SaldoChangeSummaryMeta
                {
                    ИзменениеСальдоПоБазовомуТарифу = saldoChangeSummaryProxies.Get(WalletType.BaseTariffWallet).Change,
                    ИзменениеСальдоПоПени = saldoChangeSummaryProxies.Get(WalletType.PenaltyWallet).Change,
                    ИзменениеСальдоПоТарифуРешения = saldoChangeSummaryProxies.Get(WalletType.DecisionTariffWallet).Change,
                }
            }.AsQueryable();
        }

        /// <inheritdoc />
        public override string Name => "Сведения по измененю сальдо импорта ЧЭС";

        /// <inheritdoc />
        public override string Description => this.Name;
    }
}