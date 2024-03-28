namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>Маппинг для "Приложения акта проверки предписания ГЖИ"</summary>
    public class ActRemovalAnnexMap : BaseEntityMap<ActRemovalAnnex>
    {

		public ActRemovalAnnexMap() : 
                base("Приложения акта проверки предписания ГЖИ", "GJI_NSO_ACTREMOVAL_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
