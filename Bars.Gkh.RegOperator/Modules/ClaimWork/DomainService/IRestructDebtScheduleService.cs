namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.ClaimWork.Dto;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Сервис для работы с графиком реструктуризации
    /// </summary>
    public interface IRestructDebtScheduleService
    {
        /// <summary>
        /// Создать график реструктуризации
        /// </summary>
        /// <param name="baseParams">
        /// <see cref="RestructDebtScheduleParam"/>
        /// </param>
        IDataResult CreateRestructSchedule(BaseParams baseParams);

        /// <summary>
        /// Список ЛС для создания графика реструктуризации
        /// </summary>
        /// <param name="baseParams">
        /// restructDebtId - идентификатор документа реструктуризации
        /// </param>
        IDataResult ListAccountInfo(BaseParams baseParams);

        /// <summary>
        /// Обновить суммы
        /// </summary>
        void SumsUpdate(IQueryable<DebtorClaimWork> debtorClaimWorks);
    }
}