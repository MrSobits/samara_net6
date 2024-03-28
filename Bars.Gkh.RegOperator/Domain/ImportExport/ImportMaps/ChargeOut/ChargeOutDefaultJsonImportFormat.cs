namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;

    public class ChargeOutDefaultJsonImportFormat : AbstractChargeOutExportProvider
    {
        public override string ProviderCode
        {
            get { return "Default"; }
        }

        public override string ProviderName
        {
            get { return "Выгрузка/загрузка"; }
        }

        public override string Format
        {
            get { return "Json"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Export | ImportExportType.Import; }
        }

        public ChargeOutDefaultJsonImportFormat()
        {
            Map(x => x.persacc_name, x => x.SetLookuper("persacc_name"));
            Map(x => x.personal_acc, x => x.SetLookuper("personal_acc"));
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