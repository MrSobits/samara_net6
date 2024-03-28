using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Hmao.Enum;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// Критерии попадания в подпрограмму
    /// </summary>
    public class SubProgramCriterias : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        // <summary>
        /// Включен отбор по статусу дома?
        /// </summary>
        public virtual bool IsStatusUsed { get; set; }

        // <summary>
        /// Статус дома
        /// </summary>
        public virtual State Status { get; set; }

        // <summary>
        /// Включен отбор по типу дома?
        /// </summary>
        public virtual bool IsTypeHouseUsed { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        // <summary>
        /// Включен отбор по состоянию дома?
        /// </summary>
        public virtual bool IsConditionHouseUsed { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Включен отбор по количеству квартир?
        /// </summary>
        public virtual bool IsNumberApartmentsUsed { get; set; }

        /// <summary>
        /// Условие на количество квартир
        /// </summary>
        public virtual Condition NumberApartmentsCondition { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int NumberApartments { get; set; }

        /// <summary>
        /// Включен отбор по году последнего капитального ремонта?
        /// </summary>
        public virtual bool IsYearRepairUsed { get; set; }

        /// <summary>
        /// Условие на год последнего капитального ремонта
        /// </summary>
        public virtual Condition YearRepairCondition { get; set; }

        /// <summary>
        /// Год последнего капитального ремонта
        /// </summary>
        public virtual short YearRepair { get; set; }

        /// <summary>
        /// Учитывать признак «Ремонт не целесообразен»
        /// </summary>
        public virtual bool IsRepairNotAdvisableUsed { get; set; }

        /// <summary>
        /// признак «Ремонт не целесообразен»
        /// </summary>
        public virtual bool RepairNotAdvisable { get; set; }

        /// <summary>
        /// Учитывать признак «Дом не участвует в КР»
        /// </summary>
        public virtual bool IsNotInvolvedCrUsed { get; set; }

        /// <summary>
        /// признак «Дом не участвует в КР»
        /// </summary>
        public virtual bool NotInvolvedCr { get; set; }

        /// <summary>
        /// Включен отбор по количеству КЭ?
        /// </summary>
        public virtual bool IsStructuralElementCountUsed { get; set; }

        /// <summary>
        /// Условие на количество КЭ
        /// </summary>
        public virtual Condition StructuralElementCountCondition { get; set; }

        /// <summary>
        /// Количество КЭ
        /// </summary>
        public virtual int StructuralElementCount { get; set; }

        /// <summary>
        /// Включен отбор по количеству этажей?
        /// </summary>
        public virtual bool IsFloorCountUsed { get; set; }

        /// <summary>
        /// Условие на количество этажей
        /// </summary>
        public virtual Condition FloorCountCondition { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public virtual int FloorCount { get; set; }

        /// <summary>
        /// Включен отбор по cроку эксплуатации?
        /// </summary>
        public virtual bool IsLifetimeUsed { get; set; }

        /// <summary>
        /// Условие на cрок эксплуатации
        /// </summary>
        public virtual Condition LifetimeCondition { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual short Lifetime { get; set; }
    }
}
