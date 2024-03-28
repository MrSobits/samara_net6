namespace Bars.Gkh.Entities
{
    /// <summary>
    /// ТехПаспорт
    /// </summary>
    public class TehPassport : BaseGkhEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
