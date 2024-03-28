/// <mapping-converter-backup>
/// using Bars.GkhCr.Entities;
/// 
/// namespace Bars.GkhCr.Modules.ClaimWork.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class BuilderViolatorViolMap : BaseImportableEntityMap<BuilderViolatorViol>
///     {
///         public BuilderViolatorViolMap()
///             : base("CR_BUILDER_VIOLATOR_VIOL")
///         {
///             References(x => x.Violation, "VIOL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.BuilderViolator, "BUILDER_VIOLATOR_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Note, "NOTE", false,  1000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; 
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Нарушение в реестре подрядчиков"</summary>
    public class BuilderViolatorViolMap : BaseEntityMap<BuilderViolatorViol>
    {
        
        public BuilderViolatorViolMap() : 
                base("Нарушение в реестре подрядчиков", "CR_BUILDER_VIOLATOR_VIOL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BuilderViolator, "Нарушитель договоров").Column("BUILDER_VIOLATOR_ID").NotNull().Fetch();
            Reference(x => x.Violation, "Нарушение").Column("VIOL_ID").NotNull().Fetch();
            Property(x => x.Note, "Примечание").Column("NOTE").Length(1000);
        }
    }
}
