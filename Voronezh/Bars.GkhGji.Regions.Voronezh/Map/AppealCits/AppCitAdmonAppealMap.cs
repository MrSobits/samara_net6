namespace Bars.GkhGji.Regions.Voronezh.Map
{ 
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;



    /// <summary>Маппинг для "Обращениям граждан - Предостережение - Нарушение"</summary>
    public class AppCitAdmonAppealMap : BaseEntityMap<AppCitAdmonAppeal>
    {

        public AppCitAdmonAppealMap() :
                base("Обращения граждан - Предостережение - Обращение", "GJI_CH_APPCIT_ADMON_APPEAL")
        {
        }

        protected override void Map()
        {
            Reference(x => x.AppealCitsAdmonition, "Предостережение").Column("APPCIT_ADMONITION_ID").NotNull().Fetch();
            Reference(x => x.AppealCits, "Нарушение").Column("APPCIT_ID").NotNull().Fetch();
        }
    }
}
