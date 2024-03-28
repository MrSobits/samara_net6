/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Контрольные сроки вида работ"
///     /// </summary>
///     public class ControlDateMap : BaseImportableEntityMap<ControlDate>
///     {
///         public ControlDateMap()
///             : base("CR_CONTROL_DATE")
///         {
///             Map(x => x.Date, "DATE_CTRL");
/// 
///             References(x => x.ProgramCr, "PROGRAM_ID").Fetch.Join().Not.Nullable();
///             References(x => x.Work, "WORK_ID").Fetch.Join().Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Контрольные сроки вида работ"</summary>
    public class ControlDateMap : BaseImportableEntityMap<Entities.ControlDate>
    {
        
        public ControlDateMap() : 
                base("Контрольные сроки вида работ", "CR_CONTROL_DATE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Контрольный срок").Column("DATE_CTRL");
            Reference(x => x.ProgramCr, "Программа КР").Column("PROGRAM_ID").NotNull().Fetch();
            Reference(x => x.Work, "Вид работы").Column("WORK_ID").NotNull().Fetch();
        }
    }
}
