namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Банк контрагента"</summary>
    public class ContragentBankMap : BaseImportableEntityMap<ContragentBank>
    {
        public ContragentBankMap() :
                base("Банк контрагента", "GKH_CONTRAGENT_BANK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Bik, "БИК").Column("BIK").Length(50);
            this.Property(x => x.CorrAccount, "Корреспондентский счет").Column("CORR_ACCOUNT").Length(300);
            this.Property(x => x.SettlementAccount, "Расчетный счет").Column("SETTL_ACCOUNT").Length(30);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Okonh, "ОКОНХ").Column("OKONH").Length(50);
            this.Property(x => x.Okpo, "ОКПО").Column("OKPO").Length(50);
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
