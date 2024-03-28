
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Виды рисков"</summary>
    public class CSCalculationMap : BaseEntityMap<CSCalculation>
    {

        public CSCalculationMap() :
                base("Виды рисков", "GKH_CS_CALCULATION")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Код").Column("DESCRIPTION").Length(1500);
            Property(x => x.Result, "Значение").Column("RESULT");
            Property(x => x.CalcDate, "CalcDate").Column("CALC_DATE");
            Reference(x => x.CSFormula, "Формула").Column("FORMULA_ID");
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.Room, "Помещение").Column("ROOM_ID").Fetch();
        }

        //public class CSCalculationNHibernateMapping : ClassMapping<CSCalculation>
        //{
        //    public CSCalculationNHibernateMapping()
        //    {
        //        this.Bag(
        //            x => x.CalculatedVariables,
        //            mapper =>
        //            {
        //                mapper.Access(Accessor.NoSetter);
        //                mapper.Fetch(CollectionFetchMode.Select);
        //                mapper.Lazy(CollectionLazy.Lazy);
        //                mapper.Key(x => x.Column("CALC_ID"));
        //                mapper.Cascade(Cascade.None);
        //            },
        //            action => action.OneToMany());
        //    }
        //}
    }
}
