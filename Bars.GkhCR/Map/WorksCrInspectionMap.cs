/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Обследование объекта"
///     /// </summary>
///     public class WorksCrInspectionMap : BaseImportableEntityMap<WorksCrInspection>
///     {
///         /// <summary>
///         /// Конструктор
///         /// </summary>
///         public WorksCrInspectionMap() : base("CR_OBJ_INSPECTION")
///         {
///             References(x => x.TypeWork, "TYPE_WORK_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Official, "OFFICIAL_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER", false, 100);
///             Map(x => x.InspectionState, "INSPECTION_STATE", true, (object)0);
///             Map(x => x.PlanDate, "PLAN_DATE");
///             Map(x => x.FactDate, "FACT_DATE");
///             Map(x => x.Reason, "REASON", false, 1000);
///             Map(x => x.Description, "DESCRIPTION", false, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    
    
    /// <summary>Маппинг для "Обследование объекта"</summary>
    public class WorksCrInspectionMap : BaseImportableEntityMap<WorksCrInspection>
    {
        
        public WorksCrInspectionMap() : 
                base("Обследование объекта", "CR_OBJ_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").NotNull().Fetch();
            Reference(x => x.Official, "Должностное лицо").Column("OFFICIAL_ID").Fetch();
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(100);
            Property(x => x.PlanDate, "Плановая дата").Column("PLAN_DATE");
            Property(x => x.InspectionState, "Факт обследования").Column("INSPECTION_STATE").DefaultValue(InspectionState.NotInspected).NotNull();
            Property(x => x.FactDate, "Фактическая дата").Column("FACT_DATE");
            Property(x => x.Reason, "Причина").Column("REASON").Length(1000);
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
        }
    }
}
