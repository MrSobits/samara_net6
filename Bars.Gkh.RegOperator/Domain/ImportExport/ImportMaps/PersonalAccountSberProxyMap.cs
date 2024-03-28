namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using ProxyEntity;

    public class PersonalAccountSberProxyMap : AbstractImportMap<PersonalAccountInfoProxy>
    {
        public override string ProviderCode
        {
            get { return "Sber"; }
        }

        public override string ProviderName
        {
            get { return "Сбер (Тамбов)"; }
        }

        public override string Format
        {
            get { return "dbf"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Export; }
        }

        public PersonalAccountSberProxyMap()
        {
            Map(x => x.PersAccNumExternalSystems, x => x.SetLookuper(new Lookuper("OUTERLS")), 20);
            Map(x => x.AccountNumber, mapper => mapper.SetLookuper(new Lookuper("LS")), 30);
            Map(x => x.Surname, x => x.SetLookuper(new Lookuper("NAME1")), 60);
            Map(x => x.Name, x => x.SetLookuper(new Lookuper("NAME2")), 15);
            Map(x => x.SecondName, x => x.SetLookuper(new Lookuper("NAME3")), 15);
            Map(x => x.Address, x => x.SetLookuper(new Lookuper("ADR1")), 60);
            Map(x => x.OpenDate, x => x.SetLookuper(new Lookuper("DOPEN")), 8);
            Map(x => x.CloseDate, x => x.SetLookuper(new Lookuper("DCLOSE")), 8);
            Map(x => x.ChargeTotal, x => x.SetLookuper(new Lookuper("SUMMA")), 20, 2);
            Map(x => x.PenaltyDebt, x => x.SetLookuper(new Lookuper("DEBT")), 20, 2);
            Map(x => x.Tariff, x => x.SetLookuper(new Lookuper("TARIFF")), 20, 2);
            Map(x => x.OwnerType, x => x.SetLookuper(new Lookuper("TYPEAB")), 1);
            Map(x => x.Area, x => x.SetLookuper(new Lookuper("AREAPLACE")), 20, 2);
            Map(x => x.AreaShare, x => x.SetLookuper(new Lookuper("ASHARE")), 20, 2);
            Map(x => x.CountDays, x => x.SetLookuper(new Lookuper("DAYCNT")), 3);
            Map(x => x.DebtCountDays, x => x.SetLookuper(new Lookuper("DEBTDAY")), 3);
            Map(x => x.TotalArea, x => x.SetLookuper(new Lookuper("TOTAREA")), 20, 2);
            Map(x => x.TotalAreaChangedDate, x => x.SetLookuper(new Lookuper("TOTAREADAT")), 8);
            Map(x => x.LivingArea, x => x.SetLookuper(new Lookuper("LIVAREA")), 20, 2);
            Map(x => x.LivingAreaChangedDate, x => x.SetLookuper(new Lookuper("LIVAREADAT")), 8);
            Map(x => x.RoomType, x => x.SetLookuper(new Lookuper("ROOMTYPE")), 1);
            Map(x => x.OwnershipType, x => x.SetLookuper(new Lookuper("OWNERSH")), 1);
            Map(x => x.InnLegal, x => x.SetLookuper(new Lookuper("INNLEG")), 1);
            Map(x => x.KppLegal, x => x.SetLookuper(new Lookuper("KPPLEG")), 1);
            Map(x => x.NameLegal, x => x.SetLookuper(new Lookuper("NAMELEG")), 1);
            Map(x => x.AreaShareChangedDate, x => x.SetLookuper(new Lookuper("ASHAREDAT")), 8);
            Map(x => x.SettlementPeriodRecalc, x => x.SetLookuper(new Lookuper("PERRECALC")), 20, 2);
            Map(x => x.RkcId, x => x.SetLookuper("EPD"));
            Map(x => x.Penalty, x => x.SetLookuper(new Lookuper("PENIASSD")), 20, 2);
        }
    }
}