namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using System;
    using Base.Entities;

    /// <summary>
    /// Экспортируемый из ГИС-а краткосрочный план ремонта
    /// </summary>
    public class RisCrPlan : BaseRisEntity
    {
        /// <summary>
        /// Наименование плана. (1..1). Строка не более 500 символов
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Муниципалитет. Код по ОКТМО. (1..1). Длина значения должна быть меньше или равна 11.
        /// </summary>
        public virtual string MunicipalityCode { get; set; }

        /// <summary>
        /// Муниципалитет. Полное наименование. (0..1). Длина значения должна быть меньше или равна 500.
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Месяц и год начала периода реализации.
        /// </summary>
        public virtual DateTime StartMonthYear { get; set; }

        /// <summary>
        /// Месяц и год окончания периода реализации.
        /// (дата с последнем дня месяца)
        /// </summary>
        public virtual DateTime EndMonthYear { get; set; }
    }
}
