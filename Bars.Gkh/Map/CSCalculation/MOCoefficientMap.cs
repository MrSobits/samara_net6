
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Виды рисков"</summary>
    public class MOCoefficientMap : BaseEntityMap<MOCoefficient>
    {

        public MOCoefficientMap() :
                base("Виды рисков", "GKH_CS_COEFFICIENT")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").NotNull().Length(20);
            Property(x => x.UnitMeasure, "размерность").Column("UNIT_MEASURE");
            Property(x => x.Value, "значение").Column("VALUE");
            Property(x => x.DateFrom, "Дата с").Column("DATE_FROM").NotNull();
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID");
            Reference(x => x.CategoryCSMKD, "Категория МКД").Column("CATEGORY_ID");
        }
    }
}
