namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.B4.DataAccess;
    using Enum;

    /// <summary>
    /// Критерии для актуализации регпрограммы
    /// </summary>
    public class CriteriaForActualizeVersion : BaseEntity
    {
        /// <summary>
        /// Наименование критерия
        /// </summary>
        public virtual CriteriaType CriteriaType { get; set; }

        /// <summary>
        /// Нижнее пороговое значение
        /// </summary>
        public virtual int ValueFrom { get; set; }

        /// <summary>
        /// Верхнее пороговое значение
        /// </summary>
        public virtual int ValueTo { get; set; }

        /// <summary>
        /// Количество баллов
        /// </summary>
        public virtual int Points { get; set; }

        /// <summary>
        /// Весовой коэффициент
        /// </summary>
        public virtual decimal Weight { get; set; }
    }
}