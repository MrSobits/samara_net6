namespace Bars.Gkh.Entities.HousingInspection
{
    /// <summary>
    /// Жилищная инспекция
    /// </summary>
    public class HousingInspection : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}