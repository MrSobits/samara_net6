namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using System;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Работа по договору КПР
    /// </summary>
    public class RisCrWork : BaseRisEntity
    {
        /// <summary>
        /// Идентификатор работы в КПР: данный идентификатор может быть получен из ГИС 
        /// при выполнении метода exportPlan и сопоставлении плановых работ из ГИС и работ в системе источнике
        /// </summary> 
        public virtual string WorkPlanGUID { get; set; }

        /// <summary>
        /// Идентификатор многоквартирного дома
        /// </summary>
        public virtual string ApartmentBuildingFiasGuid { get; set; }

        /// <summary>
        /// Вид работ капитального ремонта (НСИ 219). Код.
        /// </summary>
        public virtual string WorkKindCode { get; set; }

        /// <summary>
        /// Вид работ капитального ремонта (НСИ 219). Guid.
        /// </summary>
        public virtual string WorkKindGuid { get; set; }

        /// <summary>
        /// Месяц и год окончания работ
        /// </summary>
        public virtual string EndMonthYear { get; set; }

        /// <summary>
        /// Договор КР
        /// </summary>
        public virtual RisCrContract Contract { get; set; }

        /// <summary>
        /// Дата начала выполнения работы
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания выполнения работы
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Стоимость работы в договоре
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// Соответствующая сумма в КПР
        /// </summary>
        public virtual decimal? CostPlan { get; set; }

        /// <summary>
        /// Объем работы
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual string Okei { get; set; }

        /// <summary>
        /// Другая единица измерения
        /// </summary>
        public virtual string OtherUnit { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public virtual string AdditionalInfo { get; set; }
    }
}
