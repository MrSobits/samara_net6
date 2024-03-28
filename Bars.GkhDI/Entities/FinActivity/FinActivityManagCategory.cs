namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Управление домами по категории
    /// </summary>
    public class FinActivityManagCategory : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Тип категории дома
        /// </summary>
        public virtual TypeCategoryHouseDi TypeCategoryHouseDi { get; set; }

        /// <summary>
        /// Доход от управления
        /// </summary>
        public virtual decimal? IncomeManaging { get; set; }

        /// <summary>
        /// Доход от использования общего имущества
        /// </summary>
        public virtual decimal? IncomeUsingGeneralProperty { get; set; }

        /// <summary>
        /// Расходы на управление
        /// </summary>
        public virtual decimal? ExpenseManaging { get; set; }

        /// <summary>
        /// Взыскано с населением
        /// </summary>
        public virtual decimal? ExactPopulation { get; set; }

        /// <summary>
        /// Задолженность населения на начало
        /// </summary>
        public virtual decimal? DebtPopulationStart { get; set; }

        /// <summary>
        /// Задолженность населения на конец
        /// </summary>
        public virtual decimal? DebtPopulationEnd { get; set; }

        /// <summary>
        /// Признак не валидности для саммари(не хранимое)
        /// </summary>
        public virtual string IsInvalid { get; set; }
    }
}
