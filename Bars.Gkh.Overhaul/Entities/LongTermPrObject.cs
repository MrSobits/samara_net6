namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Объект долгосрочной программы
    /// </summary>
    public class LongTermPrObject : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
