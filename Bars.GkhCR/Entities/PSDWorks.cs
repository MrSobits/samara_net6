using Bars.B4.DataAccess;

namespace Bars.GkhCr.Entities
{
    /// <summary>
    /// Связка работ и их псд работ
    /// </summary>
    public class PSDWorks : BaseEntity
    {
        /// <summary>
        /// Работа ПСД
        /// </summary>
        public virtual TypeWorkCr PSDWork { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual TypeWorkCr Work { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Cost { get; set; }
    }
}
