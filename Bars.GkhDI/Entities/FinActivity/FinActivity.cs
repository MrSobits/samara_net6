namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Финансовая деятельность
    /// </summary>
    public class FinActivity : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual TaxSystem TaxSystem { get; set; }

        /// <summary>
        /// Величина чистых активов
        /// </summary>
        public virtual decimal? ValueBlankActive { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Иск по нанесенному ущербу(тыс руб)
        /// </summary>
        public virtual decimal? ClaimDamage { get; set; }

        /// <summary>
        /// Иск по неоказанию услуг(тыс руб)
        /// </summary>
        public virtual decimal? FailureService { get; set; }

        /// <summary>
        /// Иск по недопоставки услуг(тыс руб)
        /// </summary>
        public virtual decimal? NonDeliveryService { get; set; }
    }
}
