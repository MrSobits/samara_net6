namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Типы информации в НПА"</summary>
    public class TypeInformationNpaMap : BaseEntityMap<TypeInformationNpa>
    {
        public TypeInformationNpaMap() : 
                base("Типы информации в НПА", "GKH_DICT_TYPE_INFORMATION_NPA")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
            this.Property(x => x.Category, "Категория").Column("CATEGORY").NotNull();
        }
    }
}
