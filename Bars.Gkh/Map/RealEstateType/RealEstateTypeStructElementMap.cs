/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealEstateType
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstateType;
/// 
///     public class RealEstateTypeStructElementMap : BaseImportableEntityMap<RealEstateTypeStructElement>
///     {
///         public RealEstateTypeStructElementMap()
///             : base("REAL_EST_TYPE_STRUCT_EL") 
///         {
///             Map(x => x.Exists, "IS_EXISTS", true, true);
///             References(x => x.RealEstateType, "REAL_EST_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.StructuralElement, "STRUCT_EL_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealEstateType;
    using System;
    
    
    /// <summary>Маппинг для "Конструктивные элемент типа домов"</summary>
    public class RealEstateTypeStructElementMap : BaseImportableEntityMap<RealEstateTypeStructElement>
    {
        
        public RealEstateTypeStructElementMap() : 
                base("Конструктивные элемент типа домов", "REAL_EST_TYPE_STRUCT_EL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "Тип домов").Column("REAL_EST_TYPE_ID").NotNull().Fetch();
            Reference(x => x.StructuralElement, "Конструктивный элемент").Column("STRUCT_EL_ID").NotNull().Fetch();
            Property(x => x.Exists, "Флаг наличие/отсутствие элемента").Column("IS_EXISTS").DefaultValue(true).NotNull();
        }
    }
}
