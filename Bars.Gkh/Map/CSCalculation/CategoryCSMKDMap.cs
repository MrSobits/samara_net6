
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Категория"</summary>
    public class CategoryCSMKDMap : BaseEntityMap<CategoryCSMKD>
    {

        public CategoryCSMKDMap() :
                base("Тип категории", "GKH_CS_CATEGORY")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").NotNull().Length(20);
            Reference(x => x.TypeCategoryCS, "Категория").Column("TYPE_ID").NotNull(); ;
        }
    }
}
