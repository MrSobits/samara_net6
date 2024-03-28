namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс фильтрации займов
    /// </summary>
    public interface IRealityObjectLoanViewService
    {
        /// <summary>
        /// Метод возвращает запрос согласно фильтрам
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <param name="fetchWalletNames">Подгрузить имена кошельков (нужно для счета оплат дома)</param>
        /// <returns>Подзапрос</returns>
        IQueryable<RealityObjectLoanDto> List(BaseParams baseParams, bool fetchWalletNames = false);
    }

    /// <summary>
    /// Класс DTO для <see cref="RealityObjectLoan"/>
    /// </summary>
    public class RealityObjectLoanDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата взятия займа
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Программа КР
        /// </summary>
        public string ProgramCr { get; set; }

        /// <summary>
        /// Получатель займа
        /// </summary>
        public string LoanReceiver { get; set; }

        /// <summary>
        /// МР
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// Сумма займа
        /// </summary>
        public decimal LoanSum { get; set; }

        /// <summary>
        /// Сумма погашенного займа
        /// </summary>
        public decimal LoanReturnedSum { get; set; }

        /// <summary>
        /// Задолженность по займу
        /// </summary>
        public decimal DebtSum { get; set; }

        /// <summary>
        /// Планируемое количество месяцев для возврата
        /// </summary>
        public int PlanLoanMonthCount { get; set; }

        /// <summary>
        /// Фактическая дата возврата
        /// </summary>
        public DateTime? FactEndDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public long? Document { get; set; }

        /// <summary>
        /// Источники займа
        /// </summary>
        public string Sources { get; set; }

        /// <summary>
        /// Задолженность по займу
        /// </summary>
        public decimal Saldo { get; set; }
    }
}