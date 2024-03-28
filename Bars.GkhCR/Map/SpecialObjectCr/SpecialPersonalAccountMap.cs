namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>
    /// Маппинг для "Лицевой счет"
    /// </summary>
    public class SpecialPersonalAccountMap : BaseImportableEntityMap<SpecialPersonalAccount>
    {
        
        public SpecialPersonalAccountMap() : 
                base("Лицевой счет", "CR_SPECIAL_OBJ_PERS_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.FinanceGroup, "Группа финансирования").Column("TYPE_FIN_GROUP").NotNull();
            this.Property(x => x.Closed, "Счет закрыт").Column("CLOSED").NotNull();
            this.Property(x => x.Account, "Лицевой счет").Column("ACCOUNT").Length(300);
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
        }
    }
}
