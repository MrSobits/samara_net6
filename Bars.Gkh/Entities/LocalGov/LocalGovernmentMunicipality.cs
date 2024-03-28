namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Органы местного самоуправления мун образований
    /// </summary>
    public class LocalGovernmentMunicipality : BaseGkhEntity
    {
        /// <summary>
        /// Орган местного самоуправления
        /// </summary>
        public virtual LocalGovernment LocalGovernment { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
