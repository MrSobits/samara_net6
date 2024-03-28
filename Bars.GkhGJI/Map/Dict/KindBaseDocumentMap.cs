namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.KindBaseDocument"</summary>
	public class KindBaseDocumentMap : BaseEntityMap<KindBaseDocument>
    {
        
        public KindBaseDocumentMap() : 
                base("Bars.GkhGji.Map.Dict.KindBaseDocument", "GJI_KIND_BASE_DOC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Code, "Code").Column("CODE").Length(100).NotNull();
        }
    }
}
