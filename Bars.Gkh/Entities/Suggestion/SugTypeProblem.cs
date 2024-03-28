using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities.Suggestion
{
    /// <summary>
    /// Тип проблемы с шаблонами
    /// </summary>
    public class SugTypeProblem : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Шаблон запроса
        /// </summary>
        public virtual string RequestTemplate { get; set; }
        /// <summary>
        /// Шаблон  ответа
        /// </summary>
        public virtual string ResponceTemplate { get; set; }
        /// <summary>
        /// Рубрика
        /// </summary>
        public virtual Rubric Rubric { get; set; }


    }
}