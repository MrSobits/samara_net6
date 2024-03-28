namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Застрахованный риск
    /// </summary>
    public class BelayPolicyRisk : BaseGkhEntity
    {
        /// <summary>
        /// Виды рисков
        /// </summary>
        public virtual KindRisk KindRisk { get; set; }

        /// <summary>
        /// Страховой полис
        /// </summary>
        public virtual BelayPolicy BelayPolicy { get; set; }
    }
}