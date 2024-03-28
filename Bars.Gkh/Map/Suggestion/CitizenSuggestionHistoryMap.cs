/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities.Suggestion;
/// 
///     public class CitizenSuggestionHistoryMap : BaseImportableEntityMap<CitizenSuggestionHistory>
///     {
///         public CitizenSuggestionHistoryMap() : base("GKH_SUGG_HISTORY")
///         {
///             Map(x => x.RecordDate, "RECORD_DATE");
///             Map(x => x.ExecutionDeadline, "EXEC_DEADLINE");
///             Map(x => x.ExecutorEmail, "EXEC_EMAIL");
///             Map(x => x.TargetExecutorType, "TARGET_EXEC_TYPE");
/// 
///             References(x => x.CitizenSuggestion, "SUGG_ID");
///             References(x => x.ExecutorManagingOrganization, "MANORG_ID");
///             References(x => x.ExecutorMunicipality, "MUNICIPALITY_ID");
///             References(x => x.ExecutorZonalInspection, "INSPECTION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "История обработки обращения граждан. Сущность создается в рамках процесса автоматической обработки обращения граждан в . Сущность хранит в себе информацию о смене исполнителя и крайнем сроке исполнения."</summary>
    public class CitizenSuggestionHistoryMap : BaseImportableEntityMap<CitizenSuggestionHistory>
    {
        
        public CitizenSuggestionHistoryMap() : 
                base("История обработки обращения граждан. Сущность создается в рамках процесса автомат" +
                        "ической обработки обращения граждан в . Сущность хранит в себе информацию о смен" +
                        "е исполнителя и крайнем сроке исполнения.", "GKH_SUGG_HISTORY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RecordDate, "Дата записи в истории").Column("RECORD_DATE");
            Property(x => x.ExecutionDeadline, "Крайний срок исполнения").Column("EXEC_DEADLINE");
            Property(x => x.ExecutorEmail, "EMail департамента ЖКХ").Column("EXEC_EMAIL");
            Property(x => x.TargetExecutorType, "TargetExecutorType").Column("TARGET_EXEC_TYPE");
            Reference(x => x.CitizenSuggestion, "Обращение граждан, для которого создана запись в истории").Column("SUGG_ID");
            Reference(x => x.ExecutorManagingOrganization, "Исполнитель - управляющая организация").Column("MANORG_ID");
            Reference(x => x.ExecutorMunicipality, "Исполнитель - муниципальное образование").Column("MUNICIPALITY_ID");
            Reference(x => x.ExecutorZonalInspection, "Исполнитель - ГЖИ").Column("INSPECTION_ID");
        }
    }
}
