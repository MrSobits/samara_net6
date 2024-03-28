namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    public class DpkrGroupedYear : BaseImportableEntity
    {
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}