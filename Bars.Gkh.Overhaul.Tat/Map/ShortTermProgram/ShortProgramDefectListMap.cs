/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дефектная ведомость краткосрочки"
///     /// </summary>
///     public class ShortProgramDefectListMap : BaseEntityMap<ShortProgramDefectList>
///     {
///         public ShortProgramDefectListMap()
///             : base("OVRHL_SHORT_PROG_DEFECT")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
/// 
///             References(x => x.ShortObject, "SHORT_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Work, "WORK_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ShortProgramDefectList"</summary>
    public class ShortProgramDefectListMap : BaseEntityMap<ShortProgramDefectList>
    {
        
        public ShortProgramDefectListMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ShortProgramDefectList", "OVRHL_SHORT_PROG_DEFECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.Sum, "Sum").Column("SUM");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Reference(x => x.ShortObject, "ShortObject").Column("SHORT_OBJ_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("WORK_ID").Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
