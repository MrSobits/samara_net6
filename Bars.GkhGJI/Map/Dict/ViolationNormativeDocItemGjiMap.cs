/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.Dict;
/// 
///     public class ViolationNormativeDocItemGjiMap : BaseEntityMap<ViolationNormativeDocItemGji>
///     {
///         public ViolationNormativeDocItemGjiMap() : base("GJI_DICT_VIOL_NORMDITEM")
///         {
///             References(x => x.ViolationGji, "VIOLATION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.NormativeDocItem, "NORMATIVEDOCITEM_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.ViolationStructure, "VIOL_STRUCT", false, 200);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Сущность связи нарушения и пункта нормативного документа"</summary>
    public class ViolationNormativeDocItemGjiMap : BaseEntityMap<ViolationNormativeDocItemGji>
    {
        
        public ViolationNormativeDocItemGjiMap() : 
                base("Сущность связи нарушения и пункта нормативного документа", "GJI_DICT_VIOL_NORMDITEM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
            Reference(x => x.NormativeDocItem, "Пункт нормативного документа").Column("NORMATIVEDOCITEM_ID").NotNull().Fetch();
            Property(x => x.ViolationStructure, "Состав правонарушения").Column("VIOL_STRUCT").Length(200);
        }
    }
}
