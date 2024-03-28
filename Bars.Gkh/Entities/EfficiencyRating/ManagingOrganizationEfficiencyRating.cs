namespace Bars.Gkh.Entities.EfficiencyRating
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// УК в периоде расчёта рейтинга эффективности
    /// </summary>
    public class ManagingOrganizationEfficiencyRating : BaseEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Период рейтинга эффективности
        /// </summary>
        public virtual EfficiencyRatingPeriod Period { get; set; }

        /// <summary>
        /// Рейтинг эффективности
        /// </summary>
        public virtual decimal Rating { get; set; }

        /// <summary>
        /// Динамика
        /// </summary>
        public virtual decimal Dynamics { get; set; }

        /// <summary>
        /// Метод возвращает название mutex'а
        /// </summary>
        public virtual string GetMutexName() => $"ManagingOrganizationEfficiencyRating_{this.ManagingOrganization.Id}_{this.Period.Id}";
    }
}