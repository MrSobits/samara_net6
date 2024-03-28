using Bars.Gkh.Enums;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Ответы к квалификационному экзамену
    /// </summary>
    public class QualifyTestQuestionsAnswers : BaseGkhEntity
    {
        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual QualifyTestQuestions QualifyTestQuestions { get; set; }

        /// <summary>
        /// Номер ответа
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual  string Answer { get; set; }

        /// <summary>
        /// правильный
        /// </summary>
        public virtual YesNoNotSet IsCorrect { get; set; }

       
    }
}
