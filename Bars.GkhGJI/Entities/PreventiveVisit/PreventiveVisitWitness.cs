namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Лица, присутствующие при проверке (или свидетели)
    /// </summary>
    public class PreventiveVisitWitness : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual PreventiveVisit PreventiveVisit { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// С актом ознакомлен
        /// </summary>
        public virtual bool IsFamiliar { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }
    }
}