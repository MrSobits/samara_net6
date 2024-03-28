namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Bars.Gkh.RegOperator.Domain.ImportExport.Enums;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;

    public class BenefitsCategoryProxyMap : AbstractImportMap<BenefitsCategoryInfoProxy>
    {
        public override string ProviderCode
        {
            get
            {
                return "Benefits";
            }
        }

        public override string ProviderName
        {
            get
            {
                return "Сведения по льготным категориям граждан";
            }
        }

        public override string Format
        {
            get
            {
                return "dbf";
            }
        }

        public override ImportExportType Direction
        {
            get
            {
                return ImportExportType.Export;
            }
        }

        public BenefitsCategoryProxyMap()
        {
            Map(x => x.Surname, x => x.SetLookuper(new Lookuper("FAMIL")), 50);
            Map(x => x.Name, x => x.SetLookuper(new Lookuper("IMJA")), 50);
            Map(x => x.Otch, x => x.SetLookuper(new Lookuper("OTCH")), 50);
            Map(x => x.HouseCode, x => x.SetLookuper(new Lookuper("ID_DOMA")), 10);
            Map(x => x.MunicipalityName, x => x.SetLookuper(new Lookuper("MU")), 50);
            Map(x => x.TypeCity, x => x.SetLookuper(new Lookuper("TP_CITY")), 10);
            Map(x => x.City, x => x.SetLookuper(new Lookuper("CITY")), 50);
            Map(x => x.TypeStreet, x => x.SetLookuper(new Lookuper("TP_STREET")), 10);
            Map(x => x.Street, x => x.SetLookuper(new Lookuper("STREET")), 50);
            Map(x => x.House, x => x.SetLookuper(new Lookuper("HOUSE_NUM")), 10);
            Map(x => x.Letter, x => x.SetLookuper(new Lookuper("LITER")), 10);
            Map(x => x.Housing, x => x.SetLookuper(new Lookuper("KORPUS")), 10);
            Map(x => x.Building, x => x.SetLookuper(new Lookuper("BUILDING")), 10);
            Map(x => x.FlatNum, x => x.SetLookuper(new Lookuper("FLAT_NUM")), 10);
            Map(x => x.Dats, x => x.SetLookuper(new Lookuper("DATS")), 20);
            Map(x => x.LiveArea, x => x.SetLookuper(new Lookuper("LIVE_AREA")), 12, 2);
            Map(x => x.Share, x => x.SetLookuper(new Lookuper("SHARE")), 12, 2);
            Map(x => x.SummaLg, x => x.SetLookuper(new Lookuper("SUMMA_LG")), 12, 2);
            Map(x => x.Sum, x => x.SetLookuper(new Lookuper("SUML1")), 12, 2);
            Map(x => x.Lsa, x => x.SetLookuper(new Lookuper("LSA")), 9);
            Map(x => x.HasDebt, x => x.SetLookuper(new Lookuper("DOLG")), 1);
        }
    }
}