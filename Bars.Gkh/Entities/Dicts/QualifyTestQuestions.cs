using Bars.Gkh.Enums;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Попросы к квалификационному экзамену
    /// </summary>
    public class QualifyTestQuestions : BaseGkhEntity
    {
        /// <summary>
        /// Код вопроса
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Тематика вопроса
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string Question { get; set; }

        /// <summary>
        /// Актуальный
        /// </summary>
        public virtual YesNoNotSet IsActual { get; set; }
    }
}
