namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Вопрос экзамена
    /// </summary>
    public class QExamQuestion : BaseGkhEntity
    {
        /// <summary>
        /// Запрос на допуск к экзамену
        /// </summary>
        public virtual PersonRequestToExam PersonRequestToExam { get; set; }

        /// <summary>
        /// Вопрос к экзамену
        /// </summary>
        public virtual QualifyTestQuestions QualifyTestQuestions { get; set; }

        /// <summary>
        /// Номер вопроса
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Текст вопроса
        /// </summary>
        public virtual string QuestionText { get; set; }

        /// <summary>
        /// Выбранный ответ
        /// </summary>
        public virtual QualifyTestQuestionsAnswers QualifyTestQuestionsAnswers { get; set; }
        
    }
}