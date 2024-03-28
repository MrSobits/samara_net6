namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Лица, присутствующие при проверке (или свидетели) для акта без взаимодействия
    /// </summary>
    public class ActIsolatedWitness : BaseEntity
    {
        /// <summary>
        /// Акт без взаимодействия
        /// </summary>
        public virtual ActIsolated ActIsolated { get; set; }

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