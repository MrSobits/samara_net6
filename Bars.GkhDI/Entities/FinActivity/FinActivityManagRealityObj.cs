namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Управление домами финансовой деятельности
    /// </summary>
    public class FinActivityManagRealityObj : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Предъявлено к оплате
        /// </summary>
        public virtual decimal? PresentedToRepay { get; set; }

        /// <summary>
        /// Получено за предоставленные услуги
        /// </summary>
        public virtual decimal? ReceivedProvidedService { get; set; }

        /// <summary>
        /// Сумма задолжности
        /// </summary>
        public virtual decimal? SumDebt { get; set; }

        /// <summary>
        /// Сумма фактических расходов
        /// </summary>
        public virtual decimal? SumFactExpense { get; set; }

        /// <summary>
        /// Сумма дохода от управления
        /// </summary>
        public virtual decimal? SumIncomeManage { get; set; }
    }
}
