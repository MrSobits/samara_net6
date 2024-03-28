using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    public class SubProgramCriteriasMap : BaseEntityMap<SubProgramCriterias>
    {
        public SubProgramCriteriasMap() :
                base("Критерии попадания в подпрограмму", "OVRHL_SUBPROGRAM_CRITERIAS")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Operator, "Оператор").Column("ORERATOR_ID").NotNull().Fetch();
            //
            Property(x => x.Name, "Название").Column("NAME").NotNull();
            //
            Property(x => x.IsStatusUsed, "Включен отбор по статусу дома?").Column("IS_STATE_USED").NotNull();
            Reference(x => x.Status, "Статус дома").Column("STATE_ID").Fetch();
            //
            Property(x => x.IsTypeHouseUsed, "Включен отбор по типу дома?").Column("IS_TYPE_HOUSE_USED").NotNull();
            Property(x => x.TypeHouse, "Тип дома").Column("TYPE_HOUSE").NotNull();
            //
            Property(x => x.IsConditionHouseUsed, "Включен отбор по состоянию дома?").Column("IS_CONDITION_HOUSE_USED").NotNull();
            Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE").NotNull();
            //
            Property(x => x.IsNumberApartmentsUsed, "Включен отбор по количеству квартир?").Column("IS_NUMBER_APARTMENTS_USED").NotNull();
            Property(x => x.NumberApartmentsCondition, "Условие на количество квартир").Column("NUMBER_APARTMENTS_CONDITION").NotNull();
            Property(x => x.NumberApartments, "Количество квартир").Column("NUMBER_APARTMENTS").NotNull();
            //
            Property(x => x.IsYearRepairUsed, "Включен отбор по году последнего капитального ремонта?").Column("IS_YEAR_REPAIR_USED").NotNull();
            Property(x => x.YearRepairCondition, "Условие на год последнего капитального ремонта").Column("YEAR_REPAIR_CONDITION").NotNull();
            Property(x => x.YearRepair, "Год последнего капитального ремонта").Column("YEAR_REPAIR").NotNull();
            //
            Property(x => x.IsRepairNotAdvisableUsed, "Учитывать признак «Ремонт не целесообразен»").Column("IS_REPAIR_NOT_ADVISABLE_USED").NotNull();
            Property(x => x.RepairNotAdvisable, "признак «Ремонт не целесообразен»").Column("REPAIR_NOT_ADVISABLE").NotNull();
            //
            Property(x => x.IsNotInvolvedCrUsed, "Учитывать признак «Дом не участвует в КР»").Column("IS_NOT_INVOLVED_CR_USED").NotNull();
            Property(x => x.NotInvolvedCr, "признак «Дом не участвует в КР»").Column("NOT_INVOLVED_CR").NotNull();
            //
            Property(x => x.IsStructuralElementCountUsed, "Включен отбор по количеству КЭ?").Column("IS_STRUCT_EL_COUNT_USED").NotNull();
            Property(x => x.StructuralElementCountCondition, "Условие на количество КЭ").Column("STRUCT_EL_COUNT_CONDITION").NotNull();
            Property(x => x.StructuralElementCount, "Количество КЭ").Column("STRUCT_EL_COUNT").NotNull();
            //
            Property(x => x.IsFloorCountUsed, "Включен отбор по количеству этажей?").Column("IS_FLOOR_COUNT_USED").NotNull();
            Property(x => x.FloorCountCondition, "Условие на количество этажей").Column("FLOOR_COUNT_CONDITION").NotNull();
            Property(x => x.FloorCount, "Количество этажей").Column("FLOOR_COUNT").NotNull();
            //
            Property(x => x.IsLifetimeUsed, "Включен отбор по cроку эксплуатации?").Column("IS_LIFETIME_USED").NotNull();
            Property(x => x.LifetimeCondition, "Условие на cрок эксплуатации").Column("LIFETIME_CONDITION").NotNull();
            Property(x => x.Lifetime, "Срок эксплуатации").Column("LIFETIME").NotNull();
        }
    }
}
