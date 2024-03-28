namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Collections.Generic;
    using B4;

    using Dto;
    using Gkh.Entities;

    /// <summary>
    /// Репозиторий источников займа
    /// </summary>
    public interface ILoanSourceRepository
    {
        /// <summary>
        /// Получить список доступных источников для займа
        /// </summary>
        IDataResult ListAvailableSources(BaseParams baseParams);

        /// <summary>
        /// Получить список доступных источников для займа
        /// </summary>
        IEnumerable<LoanAvailableSource> ListAvailableSources(RealityObject[] robjects);

        /// <summary>
        /// Сальдо регоператора
        /// </summary>
        /// <returns></returns>
        RegopLoanInfo GetRegoperatorSaldo(long muId);
    }

    /// <summary>
    /// Класс информации о состоянии счёта Регопа (для раздела Управление займами)
    /// </summary>
    public class RegopLoanInfo
    {
        /// <summary>
        /// Доступные средства для займа
        /// </summary>
        public decimal AvailableLoanSum { get; set; }

        /// <summary>
        /// Потребность всех домов
        /// </summary>
        public decimal NeedSum { get; set; }

        /// <summary>
        /// Текущее сальдо
        /// </summary>
        public decimal AvailableSum { get; set; }

        /// <summary>
        /// Сумма заблокированных средств = сумма текущих неоплаченных займов + сальдо без учета займа на домах, у которых есть ненулевая потребность потребность + сальдо домов, у которых есть непогашенный займ
        /// </summary>
        public decimal BlockedSum { get; set; }
    }
}
