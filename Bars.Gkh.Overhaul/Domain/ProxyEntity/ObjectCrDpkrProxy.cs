namespace Bars.Gkh.Overhaul.Domain.ProxyEntity
{
    using Gkh.Entities;

    /// <summary>
    /// Прокси для строки из дпкр
    /// </summary>
    public class ObjectCrDpkrProxy
    {
        /// <summary>
        /// Объекта недвижимости
        /// </summary>
        public RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый год ремонта ООИ
        /// </summary>
        public int PlanYear { get; set; }

        /// <summary>
        /// Сумма ремонта
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Наименование ООИ
        /// </summary>
        public string CommonEstateObjectName { get; set; }
    }
}