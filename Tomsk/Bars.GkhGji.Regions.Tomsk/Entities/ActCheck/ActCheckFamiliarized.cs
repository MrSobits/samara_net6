namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Лица, присутствующие при проверке (или свидетели)
    /// </summary>
    public class ActCheckFamiliarized : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }
    }
}