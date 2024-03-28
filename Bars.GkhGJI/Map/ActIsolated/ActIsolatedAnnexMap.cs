namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Приложения акта без взаимодействия"</summary>
    public class ActIsolatedAnnexMap : BaseEntityMap<ActIsolatedAnnex>
    {
        
        public ActIsolatedAnnexMap() : 
                base("Приложения акта без взаимодействия", "GJI_ACTISOLATED_ANNEX")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
