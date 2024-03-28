namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Описание для ответа на обращения
    /// </summary>
    public class AppealAnswerLongText : BaseGkhEntity
    {
        /// <summary>
        /// Ответ по обращению граждан
        /// </summary>
        public virtual AppealCitsAnswer AppealCitsAnswer { get; set; }

        /// <summary>
        /// Описание (текст ответа)
        /// </summary>
        public virtual byte[] Description { get; set; }

        /// <summary>
        /// Описание (текст ответа)2
        /// </summary>
        public virtual byte[] Description2 { get; set; }
    }
}