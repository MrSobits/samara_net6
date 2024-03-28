using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    public class DPKRActualCriteriasMap : BaseEntityMap<DPKRActualCriterias>
    {
        public DPKRActualCriteriasMap() : base("Критерии актуализации ДПКР", "OVRHL_DPKR_ACTUAL_CRITERIAS")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Operator, "Оператор").Column("ORERATOR_ID").NotNull().Fetch();
            Reference(x => x.Status, "Статус дома").Column("STATE_ID").Fetch();
            Reference(x => x.SEStatus, "Статус КЭ").Column("KESTATE_ID").Fetch();
            //
            Property(x => x.DateStart, "Дата начала действия условий").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата прекращения действия условий").Column("DATE_END").NotNull();
            Property(x => x.TypeHouse, "Допустимый тип дома").Column("TYPE_HOUSE");
            Property(x => x.ConditionHouse, "Допустимое состояние дома").Column("CONDITION_HOUSE");
            Property(x => x.IsNumberApartments, "Включен отбор по количеству квартир?").Column("IS_NUMBER_APARTMENTS").NotNull();
            Property(x => x.NumberApartmentsCondition, " Условие на количество квартир").Column("NUMBER_APARTMENTS_CONDITION");
            Property(x => x.NumberApartments, "Количество квартир").Column("NUMBER_APARTMENTS");
            Property(x => x.IsYearRepair, "Включен отбор по году последнего капитального ремонта?").Column("IS_YEAR_REPAIR").NotNull();
            Property(x => x.YearRepairCondition, "Условие на год последнего капитального ремонта").Column("YEAR_REPAIR_CONDITION");
            Property(x => x.YearRepair, "Год последнего капитального ремонта").Column("YEAR_REPAIR");
            Property(x => x.CheckRepairAdvisable, "Учитывать признак «Ремонт не целесообразен»").Column("CHECK_REPAIR_ADVISABLE").NotNull();
            Property(x => x.CheckInvolvedCr, "Учитывать признак «Дом не участвует в КР»").Column("CHECK_INVOLVED_CR").NotNull();
            Property(x => x.IsStructuralElementCount, "Включен отбор по количеству КЭ?").Column("IS_STRUCT_EL_COUNT").NotNull();
            Property(x => x.StructuralElementCountCondition, "Условие на количество КЭ").Column("STRUCT_EL_COUNT_CONDITION");
            Property(x => x.StructuralElementCount, "Количество КЭ").Column("STRUCT_EL_COUNT");

        }
    }

    //public class DPKRActualCriteriasNHibernateMapping : ClassMapping<DPKRActualCriterias>
    //{
    //    public DPKRActualCriteriasNHibernateMapping()
    //    {
    //        Property(
    //            x => x.Statuses,
    //            x =>
    //            {
    //                x.Type<JsonSerializedType<List<State>>>();
    //            });
    //    }
    //}
}
