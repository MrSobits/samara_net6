namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;

    public class ChargeOutFsGorodImportFormat : AbstractChargeOutExportProvider
    {
        public override string ProviderCode
        {
            get { return "fs_gorod"; }
        }

        public override string ProviderName
        {
            get { return "ФС Город"; }
        }

        public override string Format
        {
            get { return "txt"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Export; }
        }
    }
}