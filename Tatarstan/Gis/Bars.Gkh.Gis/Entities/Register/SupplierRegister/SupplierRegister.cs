namespace Bars.Gkh.Gis.Entities.Register.SupplierRegister
{
    using B4.DataAccess;

    /// <summary>
    /// Поставщик
    /// </summary>
    public class SupplierRegister : BaseEntity
    {
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual long Inn { get; set; }
    }
}
