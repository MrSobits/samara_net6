namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.DocumentCode"</summary>
	public class DocumentCodeMap : BaseEntityMap<DocumentCode>
    {
        
        public DocumentCodeMap() : 
                base("Bars.GkhGji.Map.Dict.DocumentCode", "GJI_DOCUMENT_CODE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Type, "Type").Column("TYPE").NotNull();
            this.Property(x => x.Code, "Code").Column("CODE").NotNull();
        }
    }
}
