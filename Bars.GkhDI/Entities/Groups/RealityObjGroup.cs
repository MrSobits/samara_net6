namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом группы домов
    /// </summary>
    public class RealityObjGroup : BaseImportableEntity
    {
        /// <summary>
        /// Группа дома
        /// </summary>
        public virtual GroupDi GroupDi { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
