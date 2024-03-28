namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedArticleLawMap : BaseEntityMap<TaskActionIsolatedArticleLaw>
    {
        public TaskActionIsolatedArticleLawMap() : 
            base("Статья закона задания КНМ без взаимодействия с контролируемыми лицами", "GJI_TASK_ACTIONISOLATED_ARTICLE_LAW")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Task, "Задание").Column("TASK_ID");
            this.Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID");
        }
    }
}