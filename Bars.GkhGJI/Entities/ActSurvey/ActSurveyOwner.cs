namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сведения о собсвенниках в акте обследования
    /// </summary>
    public class ActSurveyOwner : BaseEntity
    {
        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual ActSurvey ActSurvey { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string WorkPlace { get; set; }

        /// <summary>
        /// Правоустанавливающий документ
        /// </summary>
        public virtual string DocumentName { get; set; }
    }
}