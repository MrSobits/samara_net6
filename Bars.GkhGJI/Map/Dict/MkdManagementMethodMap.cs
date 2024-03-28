namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.MkdManagementMethod"</summary>
	public class MkdManagementMethodMap : BaseEntityMap<MkdManagementMethod>
    {
        
        public MkdManagementMethodMap() : 
                base("Bars.GkhGji.Map.Dict.MkdManagementMethod", "GJI_MKD_MANAGEMENT_METHOD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Code, "Code").Column("CODE").Length(100).NotNull();
        }
    }
}
