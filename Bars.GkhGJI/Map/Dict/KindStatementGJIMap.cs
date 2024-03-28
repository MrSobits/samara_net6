/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Вид обращения"
///     /// </summary>
///     public class KindStatementGjiMap : BaseGkhEntityMap<KindStatementGji>
///     {
///         public KindStatementGjiMap() : base("GJI_DICT_KINDSTATEMENT")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вид обращения"</summary>
    public class KindStatementGjiMap : BaseEntityMap<KindStatementGji>
    {
        
        public KindStatementGjiMap() : 
                base("Вид обращения", "GJI_DICT_KINDSTATEMENT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.Postfix, "Постфикс").Column("POSTFIX").Length(300).DefaultValue(string.Empty);
        }
    }
}
