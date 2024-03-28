namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Разрез финансирования по КР"</summary>
    public class FinanceSourceMap : BaseImportableEntityMap<FinanceSource>
    {
        
        public FinanceSourceMap() : 
                base("Разрез финансирования по КР", "CR_DICT_FIN_SOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeFinanceGroup, "Группа финансирования").Column("TYPE_FIN_GROUP").NotNull();
            Property(x => x.TypeFinance, "Тип разреза").Column("TYPE_FINANCE").NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(200);
            Property(x => x.Name, "Наимеонвание").Column("NAME").Length(300);
        }
    }
}
