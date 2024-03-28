namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Обновления сальдо"</summary>
    public class SaldoRefreshMap : BaseEntityMap<SaldoRefresh>
    {
        
        public SaldoRefreshMap() : 
                base("Расчетный счет", "REGOP_SALDO_REFRESH")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Group, "Группа").Column("GROUP_ID");
        }
    }
}
