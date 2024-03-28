namespace Bars.GkhGji.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Подтематика обращения
    /// </summary>
    public class StatSubsubjectGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код подтематики в Тематическом классификаторе
        /// </summary>
        public virtual string QuestionCode { get; set; }

        /// <summary>
        /// Код ССТУ
        /// </summary>
        public virtual string SSTUCodeSub { get; set; }

        /// <summary>
        /// Код ССТУ
        /// </summary>
        public virtual string SSTUNameSub { get; set; }

        /// <summary>
        /// Выгружать в СОПР
        /// </summary>
        public virtual bool ISSOPR { get; set; }

        /// <summary>
        /// Текст стандартного ответа
        /// </summary>
        public virtual string AppealAnswerText { get; set; }

        /// <summary>
        /// Текст стандартного ответа 2
        /// </summary>
        public virtual string AppealAnswerText2 { get; set; }

        /// <summary>
        /// Отслеживать обращение
        /// </summary>
        public virtual bool TrackAppealCits { get; set; }
        
        /// Учитывается в СОПР
        /// </summary>
        public virtual bool NeedInSopr { get; set; }
    }
}