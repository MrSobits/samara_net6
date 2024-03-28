namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Bars.B4.Utils;

    using Enums;

    public class ChargeOutVersion1TxtImportFormat : AbstractChargeOutExportProvider
    {
        public override string ProviderCode
        {
            get { return "Версия=1"; }
        }

        public override string ProviderName
        {
            get { return "Выгрузка"; }
        }

        public override string Format
        {
            get { return "json"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Export; }
        }

        protected override string FileName
        {
            get { return "{0}.txt".FormatUsing(ProviderCode); }
        }

        public override string GetName()
        {
            return "Версия=1 (txt | Export)";
        }

        public ChargeOutVersion1TxtImportFormat()
        {
            Map(x => x.persacc_name, x => x.SetLookuper("persacc_name"));
            Map(x => x.persacc_extsys, x => x.SetLookuper("personal_acc"));
            Map(x => x.mr, x => x.SetLookuper("mr"));
            Map(x => x.mu, x => x.SetLookuper("mu"));
            Map(x => x.city, x => x.SetLookuper("city"));
            Map(x => x.street, x => x.SetLookuper("street"));
            Map(x => x.house, x => x.SetLookuper("house"));
            Map(x => x.liter, x => x.SetLookuper("liter"));
            Map(x => x.housing, x => x.SetLookuper("housing"));
            Map(x => x.building, x => x.SetLookuper("building"));
            Map(x => x.room_num, x => x.SetLookuper("room_num"));
            Map(x => x.month, x => x.SetLookuper("month"));
            Map(x => x.year, x => x.SetLookuper("year"));
            Map(x => x.charged_sum, x => x.SetLookuper("charged_sum"));
            Map(x => x.square, x => x.SetLookuper("square"));
        } 
    }
}