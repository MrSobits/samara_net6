/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PaysizeMap : BaseImportableEntityMap<Paysize>
///     {
///         public PaysizeMap() : base("OVRHL_PAYSIZE")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.Indicator, "INDICATOR");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Размер взноса на кр"</summary>
    public class PaysizeMap : BaseImportableEntityMap<Paysize>
    {
        
        public PaysizeMap() : 
                base("Размер взноса на кр", "OVRHL_PAYSIZE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Indicator, "Показатель").Column("INDICATOR");
            Property(x => x.DateStart, "Дата начала действия").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания действия").Column("DATE_END");
        }
    }
}
