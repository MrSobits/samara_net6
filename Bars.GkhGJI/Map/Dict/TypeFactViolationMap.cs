namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.TypeFactViolation"</summary>
	public class TypeFactViolationMap : BaseEntityMap<TypeFactViolation>
    {
        
        public TypeFactViolationMap() : 
                base("Bars.GkhGji.Map.Dict.TypeFactViolation", "GJI_DICT_TYPE_FACT_VIOL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Name, "Name").Column("NAME").Length(500).NotNull();
        }
    }
}
