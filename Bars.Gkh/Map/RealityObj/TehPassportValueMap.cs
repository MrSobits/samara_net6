namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Значение ТехПаспорта"</summary>
    public class TehPassportValueMap : BaseImportableEntityMap<TehPassportValue>
    {
        
        public TehPassportValueMap() : 
                base("Значение ТехПаспорта", "TP_TEH_PASSPORT_VALUE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.FormCode, "Код Формы").Column("FORM_CODE");
            this.Property(x => x.CellCode, "Код Ячейки").Column("CELL_CODE");
            this.Property(x => x.Value, "Значение").Column("VALUE");
            this.Reference(x => x.TehPassport, "ТехПаспорт").Column("TEH_PASSPORT_ID").NotNull().Fetch();
        }
    }
}
