namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsResolutionMap : BaseEntityMap<AppealCitsResolution>
    {
        
        public AppealCitsResolutionMap() : 
                base("Обращениям граждан - Резолюция", "GJI_APPCIT_RESOLUTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ResolutionText, "Текст резолюции").Column("RESOLUTION_TEXT");
            Property(x => x.ResolutionTerm, "Срок резолюции").Column("RESOLUTION_TERM");
            Property(x => x.ResolutionAuthor, "Автор резолюции").Column("RESOLUTION_AUTHOR").Length(150);
            Property(x => x.ResolutionDate, "Дата резолюции").Column("RESOLUTION_DATE");
            Property(x => x.ResolutionContent, "Содержание резолюции").Column("CONTENT");
            Property(x => x.ImportId, "ID из АС ДОУ").Column("IMPORT_ID");
            Property(x => x.ParentId, "ID из АС ДОУ").Column("PARENT_ID");
            Property(x => x.Executed, "Отчет принят").Column("EXECUTED"); //
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull().Fetch();
        }
    }
    
}
