
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Тип категории"</summary>
    public class TypeCategoryCSMap : BaseEntityMap<TypeCategoryCS>
    {

        public TypeCategoryCSMap() :
                base("Тип категории", "GKH_CS_CATEGORY_TYPE")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").NotNull().Length(20);
        }
    }
}
