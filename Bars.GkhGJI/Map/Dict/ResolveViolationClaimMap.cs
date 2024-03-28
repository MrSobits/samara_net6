namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.ResolveViolationClaim"</summary>
	public class ResolveViolationClaimMap : BaseEntityMap<ResolveViolationClaim>
    {
        
        public ResolveViolationClaimMap() : 
                base("Bars.GkhGji.Map.Dict.ResolveViolationClaim", "GJI_DICT_RES_VIOL_CLAIM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Name, "Name").Column("NAME").Length(500).NotNull();
        }
    }
}
