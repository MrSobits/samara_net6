namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерфейс сервиса получения информации о денежных движениях по управлению домами
    /// </summary>
    public interface IDisclosureInfoHouseManagingMoneys
    {
        /// <summary>
        /// Метод возвращает информацию о денежных движениях по управлению домами
        /// </summary>
        /// <param name="diQuery">Запрос фильтрации</param>
        /// <returns>Словарь</returns>
        IDictionary<long, DisclosureInfoHouseManagingInfo> GetDisclosureInfoHouseManagingInfo(IQueryable<DisclosureInfo> diQuery);
    }

    /// <summary>
    /// Инфморация о денежных движениях по управлению домами
    /// </summary>
    public class DisclosureInfoHouseManagingInfo
    {
        /// <summary>
        /// Идентификатор УО
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Сумма фактических расходов
        /// </summary>
        public decimal SumFactExpense { get; set; }

        /// <summary>
        /// Сумма дохода от управления
        /// </summary>
        public decimal SumIncomeManage { get; set; }

        /// <summary>
        /// Получено за предоставленные услуги
        /// </summary>
        public decimal ReceivedProvidedService { get; set; }

        /// <summary>
        /// Предъявлено к оплате
        /// </summary>
        public decimal PresentedToRepay { get; set; }
    }
}