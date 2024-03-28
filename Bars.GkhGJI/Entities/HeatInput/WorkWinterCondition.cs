namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Подготовка к работе в зимних условиях
    /// </summary>
    public class WorkWinterCondition : BaseEntity
    {
        /// <summary>
        /// Период подачи тепла
        /// </summary>
        public virtual HeatInputPeriod HeatInputPeriod { get; set; }

        /// <summary>
        /// Показатели о готовности ЖКС к зимнему периоду
        /// </summary>
        public virtual WorkInWinterMark WorkInWinterMark { get; set; }

        /// <summary>
        /// Всего
        /// </summary>
        public virtual decimal? Total { get; set; }

        /// <summary>
        /// Задание по подготовке
        /// </summary>
        public virtual decimal? PreparationTask { get; set; }

        /// <summary>
        /// Подготовлено для работы в зимних условиях на отчетный период
        /// </summary>
        public virtual decimal? PreparedForWork { get; set; }

        /// <summary>
        /// Выполнено работ по капитальному ремонту, реконструкции, замене
        /// </summary>
        public virtual decimal? FinishedWorks { get; set; }

        /// <summary>
        /// % выполнения задания
        /// </summary>
        public virtual decimal? Percent { get; set; }
    }
}