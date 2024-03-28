namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Лица, присутствующие при проверке (или свидетели)
    /// </summary>
    public class ActRemovalWitness : BaseGkhEntity
    {
        /// <summary>
        /// Акт проверки предписания
        /// </summary>
        public virtual ActRemoval ActRemoval { get; set; }

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