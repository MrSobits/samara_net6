namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Связь с тематиками обращения
    /// </summary>
    public class AppealCitsAnswerStatSubject : BaseEntity
    {
        /// <summary>
        /// Ответ
        /// </summary>
        public virtual AppealCitsAnswer AppealCitsAnswer { get; set; }

        /// <summary>
        /// Таблица связи тематики, подтематики и характеристики и обращения
        /// </summary>
        public virtual AppealCitsStatSubject StatSubject { get; set; }
    }
}