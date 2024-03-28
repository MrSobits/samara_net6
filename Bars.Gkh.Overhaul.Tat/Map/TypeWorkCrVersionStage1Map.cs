/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.Version
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class TypeWorkCrVersionStage1Map : BaseEntityMap<TypeWorkCrVersionStage1>
///     {
///         public TypeWorkCrVersionStage1Map()
///             : base("OVRHL_TYPE_WORK_CR_ST1")
///         {
///             References(x => x.Stage1Version, "ST1_ID", ReferenceMapConfig.Fetch);
///             this.References(x => x.TypeWorkCr, "TYPE_WORK_CR_ID", ReferenceMapConfig.NotNullAndFetch);
///             this.Map(x => x.Volume, "VOLUME", true, 0);
///             this.Map(x => x.Sum, "SUM", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.TypeWorkCrVersionStage1"</summary>
    public class TypeWorkCrVersionStage1Map : BaseEntityMap<TypeWorkCrVersionStage1>
    {
        
        public TypeWorkCrVersionStage1Map() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.TypeWorkCrVersionStage1", "OVRHL_TYPE_WORK_CR_ST1")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Stage1Version, "Stage1Version").Column("ST1_ID").Fetch();
            Reference(x => x.TypeWorkCr, "TypeWorkCr").Column("TYPE_WORK_CR_ID").NotNull().Fetch();
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
        }
    }
}
