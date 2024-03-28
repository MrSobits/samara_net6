namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тип шахты лифта
    /// </summary>
    public class TypeLiftShaft : BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}