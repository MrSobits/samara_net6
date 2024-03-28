namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.Gkh.Map;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>
    /// Маппинг для сущности "Приложения протокола ГЖИ 19.7"
    /// </summary>
	public class Protocol197AnnexMap : BaseEntityMap<Protocol197Annex>
    {
		public Protocol197AnnexMap() : 
                base("Приложения протокола ГЖИ", "GJI_PROTOCOL197_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
