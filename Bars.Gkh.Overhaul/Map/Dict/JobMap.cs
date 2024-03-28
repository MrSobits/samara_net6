/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы"
///     /// </summary>
///     public class JobMap : BaseImportableEntityMap<Job>
///     {
///         public JobMap()
///             : base("OVRHL_DICT_JOB")
///         {
///             Map(x => x.Name, "NAME", true, 300);
///             References(x => x.Work, "WORK_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Работа"</summary>
    public class JobMap : BaseImportableEntityMap<Job>
    {
        
        public JobMap() : 
                base("Работа", "OVRHL_DICT_JOB")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Reference(x => x.Work, "Вид работ").Column("WORK_ID").NotNull().Fetch();
            Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
