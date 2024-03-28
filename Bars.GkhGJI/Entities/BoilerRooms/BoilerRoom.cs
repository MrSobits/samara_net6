namespace Bars.GkhGji.Entities.BoilerRooms
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Котельная
    /// </summary>
    public class BoilerRoom : BaseEntity
    {
        /// <summary>
        /// Адрес котельной
        /// </summary>
        public virtual FiasAddress Address { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}