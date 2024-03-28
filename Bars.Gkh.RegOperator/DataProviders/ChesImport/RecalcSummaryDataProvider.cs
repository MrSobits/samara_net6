namespace Bars.Gkh.RegOperator.DataProviders.ChesImport
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta;
    using Bars.Gkh.RegOperator.DomainService.Import;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сведения по перерасчетам оплатам ЧЭС
    /// </summary>
    public class RecalcSummaryDataProvider : BaseCollectionDataProvider<RecalcSummaryMeta>
    {
        private readonly IChesImportService service;

        /// <inheritdoc />
        public RecalcSummaryDataProvider(IWindsorContainer container, IChesImportService service)
            : base(container)
        {
            this.service = service;
        }

        /// <inheritdoc />
        protected override IQueryable<RecalcSummaryMeta> GetDataInternal(BaseParams baseParams)
        {
            var data = ((List<ChesImportService.RecalcSummaryProxy>)this.service.ListRecalcInfo(baseParams).Data).ToDictionary(x => x.WalletType);

            return new[]
            {
                new RecalcSummaryMeta
                {
                    ПерерасчетПоБазовомуТарифу = data[WalletType.BaseTariffWallet].Recalc,
                    ПерерасчетПоПени = data[WalletType.PenaltyWallet].Recalc,
                    ПерерасчетПоТарифуРешения = data[WalletType.DecisionTariffWallet].Recalc
                }
            }.AsQueryable();
        }

        /// <inheritdoc />
        public override string Name => "Сведения по перерасчетам импорта ЧЭС";

        /// <inheritdoc />
        public override string Description => this.Name;
    }
}