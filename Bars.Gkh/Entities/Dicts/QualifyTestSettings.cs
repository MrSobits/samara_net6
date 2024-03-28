using Bars.Gkh.Enums;
using System;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Настройки квалификационного экзамена
    /// </summary>
    public class QualifyTestSettings : BaseGkhEntity
    {
        /// <summary>
        /// Количество вопроса
        /// </summary>
        public virtual int QuestionsCount { get; set; }

        /// <summary>
        /// Продолжительность в минутах
        /// </summary>
        public virtual int TimeStampMinutes { get; set; }

        /// <summary>
        /// Баллов за вопрос
        /// </summary>
        public virtual int CorrectBall { get; set; }

        /// <summary>
        /// Проходной балл
        /// </summary>
        public virtual decimal AcceptebleRate { get; set; }

        /// <summary>
        /// Актуальный c
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Актуальный по
        /// </summary>
        public virtual DateTime? DateTo { get; set; }
    }
}
