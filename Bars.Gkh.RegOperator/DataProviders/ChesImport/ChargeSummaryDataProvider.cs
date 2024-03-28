namespace Bars.Gkh.RegOperator.DataProviders.ChesImport
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches;

    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Сведения по начислениям импорта ЧЭС
    /// </summary>
    public class ChargeSummaryDataProvider : BaseCollectionDataProvider<ChargeSummaryMeta>
    {
        private readonly IChesImportService service;

        /// <inheritdoc />
        public ChargeSummaryDataProvider(IWindsorContainer container, IChesImportService service)
            : base(container)
        {
            this.service = service;
        }

        /// <inheritdoc />
        protected override IQueryable<ChargeSummaryMeta> GetDataInternal(BaseParams baseParams)
        {
            var importer = this.service.GetImporter(baseParams, FileType.Calc);
            var data = importer.GetSummaryData(baseParams);

            return new[]
            {
                new ChargeSummaryMeta
                {
                    ВходящееСальдо = data.Get("SaldoIn").ToDecimal(),
                    ВходящееСальдоПоБазовомуТарифу = data.Get("SaldoInBase").ToDecimal(),
                    ВходящееСальдоПоПени = data.Get("SaldoInPeni").ToDecimal(),
                    ВходящееСальдоПоТарифуРешения = data.Get("SaldoInTr").ToDecimal(),

                    НачисленоПоБазовомуТарифу = data.Get("ChargeBase").ToDecimal(),
                    НачисленоПоПени = data.Get("ChargePeni").ToDecimal(),
                    НачисленоПоТарифуРешения = data.Get("ChargeTr").ToDecimal(),

                    ОплаченоПоБазовомуТарифу = data.Get("PaymentBase").ToDecimal(),
                    ОплаченоПоПени = data.Get("PaymentPeni").ToDecimal(),
                    ОплаченоПоТарифуРешения = data.Get("PaymentTr").ToDecimal(),

                    ИзменениеСальдоПоБазовомуТарифу = data.Get("ChangeBase").ToDecimal(),
                    ИзменениеСальдоПоПени = data.Get("ChangePeni").ToDecimal(),
                    ИзменениеСальдоПоТарифуРешения = data.Get("ChangeTr").ToDecimal(),

                    ПерерасчетПоБазовомуТарифу = data.Get("RecalcBase").ToDecimal(),
                    ПерерасчетПоПени = data.Get("RecalcPeni").ToDecimal(),
                    ПерерасчетПоТарифуРешения = data.Get("RecalcTr").ToDecimal(),

                    ИсходящееСальдо = data.Get("SaldoOuth").ToDecimal(),
                    ИсходящееСальдоПоБазовомуТарифу = data.Get("SaldoOuthBase").ToDecimal(),
                    ИсходящееСальдоПоПени = data.Get("SaldoOuthPeni").ToDecimal(),
                    ИсходящееСальдоПоТарифуРешения = data.Get("SaldoOuthTr").ToDecimal()
                }
            }.AsQueryable();
        }

        /// <inheritdoc />
        public override string Name => "Сведения по начислениям импорта ЧЭС";

        /// <inheritdoc />
        public override string Description => this.Name;
    }
}