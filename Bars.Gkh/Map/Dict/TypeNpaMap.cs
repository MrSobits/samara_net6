namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы НПА"</summary>
    public class TypeNpaMap : BaseEntityMap<TypeNpa>
    {
        public TypeNpaMap() : 
                base("Типы НПА", "GKH_DICT_TYPE_NPA")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
