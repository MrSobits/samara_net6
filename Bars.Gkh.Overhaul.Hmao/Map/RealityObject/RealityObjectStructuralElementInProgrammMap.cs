/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class RealityObjectStructuralElementInProgrammMap : BaseImportableEntityMap<RealityObjectStructuralElementInProgramm>
///     {
///         public RealityObjectStructuralElementInProgrammMap() : base("OVRHL_RO_STRUCT_EL_IN_PRG")
///         {
///             References(x => x.StructuralElement, "RO_SE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage2, "STAGE2_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.ServiceCost, "SERVICE_COST", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Конструктивный элемент дома в ДПКР"</summary>
    public class RealityObjectStructuralElementInProgrammMap : BaseImportableEntityMap<RealityObjectStructuralElementInProgramm>
    {
        
        public RealityObjectStructuralElementInProgrammMap() : 
                base("Конструктивный элемент дома в ДПКР", "OVRHL_RO_STRUCT_EL_IN_PRG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StructuralElement, "КЭ").Column("RO_SE_ID").NotNull().Fetch();
            Property(x => x.Year, "Год").Column("YEAR").NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            Property(x => x.ServiceCost, "Стоимость услуг").Column("SERVICE_COST").NotNull();
            Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").Fetch();
        }
    }
}
