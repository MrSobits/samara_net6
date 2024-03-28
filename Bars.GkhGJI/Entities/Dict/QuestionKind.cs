using Bars.B4.DataAccess;

namespace Bars.GkhGji.Entities.Dict
{
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Вид вопроса
    /// </summary>
    public class QuestionKind : BaseEntity
    {
        /// <summary>
        /// Тип вопроса
        /// </summary>
        public virtual QuestionType? QuestionType { get; set; }

        /// <summary>
        /// Код записи
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
