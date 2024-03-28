namespace Bars.GkhGji.Regions.Tyumen.Entities.Suggestion
{
    using B4.Modules.States;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Уведомление для заявителя
    /// </summary>
    public class ApplicantNotification : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public virtual string EmailSubject { get; set; }

        /// <summary>
        /// Шаблон сообщения
        /// </summary>
        public virtual string EmailTemplate { get; set; }

        /// <summary>
        /// Шаблон сообщения
        /// </summary>
        public virtual State State { get; set; }
    }
}