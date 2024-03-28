namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedAnnexMap : BaseEntityMap<TaskActionIsolatedAnnex>
    {
        public TaskActionIsolatedAnnexMap() : 
            base("Приложение задания КНМ без взаимодействия с контролируемыми лицами", "GJI_TASK_ACTIONISOLATED_ANNEX")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Task, "Задание").Column("TASK_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}