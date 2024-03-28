using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Hmao.Enum;
using System;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// Критерии актуализации ДПКР
    /// </summary>
    public class DPKRActualCriterias : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Дата начала действия условий
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата прекращения действия условий
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Допустимые статусы
        /// </summary>
        public virtual State Status { get; set; }

        /// <summary>
        /// Допустимый тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Допустимое состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Включен отбор по количеству квартир?
        /// </summary>
        public virtual bool IsNumberApartments { get; set; }

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
        public virtual bool IsYearRepair { get; set; }

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
        public virtual bool CheckRepairAdvisable { get; set; }

        /// <summary>
        /// Учитывать признак «Дом не участвует в КР»
        /// </summary>
        public virtual bool CheckInvolvedCr { get; set; }

        /// <summary>
        /// Включен отбор по количеству КЭ?
        /// </summary>
        public virtual bool IsStructuralElementCount { get; set; }

        /// <summary>
        /// Условие на количество КЭ
        /// </summary>
        public virtual Condition StructuralElementCountCondition { get; set; }

        /// <summary>
        /// Количество КЭ
        /// </summary>
        public virtual int StructuralElementCount { get; set; }

        /// <summary>
        /// Допустимые статус КЭ
        /// </summary>
        public virtual State SEStatus { get; set; }

    }
}
