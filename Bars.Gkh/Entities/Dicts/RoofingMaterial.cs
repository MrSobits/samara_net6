namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Материал кровли
    /// </summary>
    public class RoofingMaterial : BaseGkhEntity
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
