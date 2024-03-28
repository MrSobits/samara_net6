namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Enums;
    using Mapping;
    using ProxyEntity;

    public class PersonalAccountInfoSberProxyMap : AbstractImportMap<PersonalAccountInfoProxy>
    {
        public override string ProviderCode
        {
            get { return "default"; }
        }

        public override string ProviderName
        {
            get { return "Сбер (с ФИО)"; }
        }

        public override string Format
        {
            get { return "dbf"; }
        }

        public override ImportExportType Direction
        {
            get { return ImportExportType.Export; }
        }

        public PersonalAccountInfoSberProxyMap()
        {
            Map(x => x.ContractId, x => x.SetLookuper(new Lookuper("DOGID")), 20);
            Map(x => x.AccountNumber, mapper => mapper.SetLookuper(new Lookuper("UNID")), 30);
            Map(x => x.Surname, x => x.SetLookuper(new Lookuper("NAME1")), 60);
            Map(x => x.Name, x => x.SetLookuper(new Lookuper("NAME2")), 15);
            Map(x => x.SecondName, x => x.SetLookuper(new Lookuper("NAME3")), 15);
            Map(x => x.Address, x => x.SetLookuper(new Lookuper("ADR1")), 60);
            Map(x => x.DateStart, x => x.SetLookuper(new Lookuper("STDATE")), 8);
            Map(x => x.DateEnd, x => x.SetLookuper(new Lookuper("ENDATE")), 8);
            Map(x => x.ChargeTotal, x => x.SetLookuper(new Lookuper("SUMMA")), 20, 2);
            Map(x => x.ReceiptId, x => x.SetLookuper(new Lookuper("BULLNUM")), 30);
            Map(x => x.AddInfo1, x => x.SetLookuper(new Lookuper("USER1")), 20, 5);
            Map(x => x.AddInfo2, x => x.SetLookuper(new Lookuper("USER2")), 30);
            Map(x => x.AddInfo3, x => x.SetLookuper(new Lookuper("USER3")), 8);
            Map(x => x.AddInfo4, x => x.SetLookuper(new Lookuper("USER4")), 20, 5);
            Map(x => x.TypePayment, x => x.SetLookuper(new Lookuper("VID_PL")), 2);
            Map(x => x.ClassificationCode, x => x.SetLookuper(new Lookuper("KBK")), 20);
            Map(x => x.Okato, x => x.SetLookuper(new Lookuper("OKATO")), 11);
        }
    }
}