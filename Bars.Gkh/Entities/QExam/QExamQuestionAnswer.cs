namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Вариант ответа
    /// </summary>
    public class QExamQuestionAnswer : BaseGkhEntity
    {
        /// <summary>
        /// Вопрос экзамена
        /// </summary>
        public virtual QExamQuestion QExamQuestion { get; set; }

        /// <summary>
        /// Вариант ответа
        /// </summary>
        public virtual QualifyTestQuestionsAnswers QualifyTestQuestionsAnswers { get; set; }

        /// <summary>
        /// Текст вопроса
        /// </summary>
        public virtual string AnswerText { get; set; }       
        
    }
}