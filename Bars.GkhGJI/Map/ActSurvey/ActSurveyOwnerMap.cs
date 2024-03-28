/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения о собственниках в акте обследования"
///     /// </summary>
///     public class ActSurveyOwnerMap : BaseEntityMap<ActSurveyOwner>
///     {
///         public ActSurveyOwnerMap()
///             : base("GJI_ACTSURVEY_OWNER")
///         {
/// 
///             Map(x => x.Fio, "FIO").Length(300).Not.Nullable();
///             Map(x => x.Position, "POSITION").Length(300);
///             Map(x => x.WorkPlace, "WORK_PLACE").Length(300);
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
/// 
///             References(x => x.ActSurvey, "ACTSURVEY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Сведения о собсвенниках в акте обследования"</summary>
    public class ActSurveyOwnerMap : BaseEntityMap<ActSurveyOwner>
    {
        
        public ActSurveyOwnerMap() : 
                base("Сведения о собсвенниках в акте обследования", "GJI_ACTSURVEY_OWNER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            Property(x => x.WorkPlace, "Место работы").Column("WORK_PLACE").Length(300);
            Property(x => x.DocumentName, "Правоустанавливающий документ").Column("DOCUMENT_NAME").Length(300);
            Reference(x => x.ActSurvey, "Акт обследования").Column("ACTSURVEY_ID").NotNull().Fetch();
        }
    }
}
