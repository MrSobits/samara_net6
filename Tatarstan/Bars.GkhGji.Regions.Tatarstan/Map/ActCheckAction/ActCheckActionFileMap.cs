namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionFileMap : BaseEntityMap<ActCheckActionFile>
    {
        public ActCheckActionFileMap()
            : base("Файл действия акта проверки", "GJI_ACTCHECK_ACTION_FILE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE").NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            
            this.Reference(x => x.ActCheckAction, "Действие акта проверки").Column("ACTCHECK_ACTION_ID");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}