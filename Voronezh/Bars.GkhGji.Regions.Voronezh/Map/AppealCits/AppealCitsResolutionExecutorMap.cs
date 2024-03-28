namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsResolutionExecutorMap : BaseEntityMap<AppealCitsResolutionExecutor>
    {
        
        public AppealCitsResolutionExecutorMap() : 
                base("Обращениям граждан - Резолюция - Исполнители", "GJI_APPCIT_RESOLUTION_EXECUTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Имя исполнителя").Column("NAME").Length(50);
            Property(x => x.Surname, "Фамилия исполнителя").Column("SURNAME").Length(50);
            Property(x => x.Patronymic, "Отчетсво исполнителя").Column("PATRONYMIC").Length(50);
            Property(x => x.PersonalTerm, "Персональный срок").Column("PERSONAL_TERM");
            Property(x => x.Comment, "Комментарий автору").Column("COMMENT").Length(300);
            Property(x => x.IsResponsible, "Ответственный").Column("IS_RESPONSIBLE");
            Reference(x => x.AppealCitsResolution, "Резолюция").Column("RESOLUTION_ID").NotNull().Fetch();
        }
    }
    
}
