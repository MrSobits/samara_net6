/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.CommonEstateObject
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.CommonEstateObject;
/// 
///     public class StructuralElementGroupAttributeMap : BaseImportableEntityMap<StructuralElementGroupAttribute>
///     {
///         public StructuralElementGroupAttributeMap()
///             : base("OVRHL_STRUCT_EL_GROUP_ATR")
///         {
///             References(x => x.Group, "GROUP_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Name, "NAME", true);
///             Map(x => x.IsNeeded, "IS_NEEDED", true, false);
///             Map(x => x.AttributeType, "ATR_TYPE", true);
///             Map(x => x.Hint, "HINT", false, 3000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.CommonEstateObject;
    using System;
    
    
    /// <summary>Маппинг для "Атрибут группы конструктивных элементов"</summary>
    public class StructuralElementGroupAttributeMap : BaseImportableEntityMap<StructuralElementGroupAttribute>
    {
        
        public StructuralElementGroupAttributeMap() : 
                base("Атрибут группы конструктивных элементов", "OVRHL_STRUCT_EL_GROUP_ATR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Group, "Группа конструктивных элементов").Column("GROUP_ID").NotNull().Fetch();
            Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            Property(x => x.IsNeeded, "Флаг: обязательность").Column("IS_NEEDED").DefaultValue(false).NotNull();
            Property(x => x.AttributeType, "Тип атрибута").Column("ATR_TYPE").NotNull();
            Property(x => x.Hint, "Подсказка").Column("HINT").Length(3000);
        }
    }
}
