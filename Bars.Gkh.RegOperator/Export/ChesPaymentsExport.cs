namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;

    /// <summary>
    /// Экспорт полат в excel
    /// </summary>
    public class ChesPaymentsExport : BaseDataExportService
    {
        public IChesImportService ChesImportService { get; set; }

        /// <summary>
        /// Метод получения Данных для Экспорта
        /// </summary>
        public override IList GetExportData(BaseParams baseParams)
        {
            baseParams.Params.SetValue("page", null);
            baseParams.Params.SetValue("limit", null);

            return this.ChesImportService.PaymentsList(baseParams)?.Data as IList;
        }
    }
}