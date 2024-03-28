namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Основание признания дома аварийным
    /// </summary>
    public class BaseHouseEmergency : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
