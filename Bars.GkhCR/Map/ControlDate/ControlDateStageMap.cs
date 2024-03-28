/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Этапы работ контрольных сроков вида работ"
///     /// </summary>
///     public class ControlDateStageWorkMap : BaseImportableEntityMap<ControlDateStageWork>
///     {
///         public ControlDateStageWorkMap()
///             : base("CR_CTRL_DATE_STAGE")
///         {
///             References(x => x.ControlDate, "CONTROL_DATE_ID").Fetch.Join().Not.Nullable();
///             References(x => x.StageWork, "STAGE_ID").Fetch.Join().Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Этапы работ контрольных сроков вида работ"</summary>
    public class ControlDateStageWorkMap : BaseImportableEntityMap<ControlDateStageWork>
    {
        
        public ControlDateStageWorkMap() : 
                base("Этапы работ контрольных сроков вида работ", "CR_CTRL_DATE_STAGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ControlDate, "Программа КР").Column("CONTROL_DATE_ID").NotNull().Fetch();
            Reference(x => x.StageWork, "Этапы работ").Column("STAGE_ID").NotNull().Fetch();
        }
    }
}
