/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Dicts;
/// 
///     public class RealEstateTypeMap : BaseImportableEntityMap<RealEstateType>
///     {
///         public RealEstateTypeMap()
///             : base("OVRHL_REAL_ESTATE_TYPE")
///         {
///             Map(x => x.Name, "NAME", true, 300);
///             Map(x => x.Code, "CODE", false, 100);
///             Map(x => x.MarginalRepairCost, "MARG_REPAIR_COST");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Тип дома"</summary>
    public class RealEstateTypeMap : BaseImportableEntityMap<RealEstateType>
    {
        
        public RealEstateTypeMap() : 
                base("Тип дома", "OVRHL_REAL_ESTATE_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Property(x => x.MarginalRepairCost, "MarginalRepairCost").Column("MARG_REPAIR_COST");
        }
    }
}
