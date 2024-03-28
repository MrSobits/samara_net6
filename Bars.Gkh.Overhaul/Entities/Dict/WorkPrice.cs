namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Расценка по работе
    /// </summary>
    public class WorkPrice : BaseGkhEntity
    {
        /// <summary>
        /// Работа
        /// </summary>
        public virtual Job Job { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Нормативная стоимость
        /// </summary>
        public virtual decimal NormativeCost { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Стоимость по квадратному метру жилой площади 
        /// </summary>
        public virtual decimal? SquareMeterCost { get; set; }

        /// <summary>
        /// Группа капитальности
        /// </summary>
        public virtual CapitalGroup CapitalGroup { get; set; }
    }
}