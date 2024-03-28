/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Содержание ответа"
///     /// </summary>
///     public class AnswerContentGjiMap : BaseGkhEntityMap<AnswerContentGji>
///     {
///         public AnswerContentGjiMap() : base("GJI_DICT_ANSWER_CONTENT")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Содержание ответа"</summary>
    public class AnswerContentGjiMap : BaseEntityMap<AnswerContentGji>
    {
        
        public AnswerContentGjiMap() : 
                base("Содержание ответа", "GJI_DICT_ANSWER_CONTENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
