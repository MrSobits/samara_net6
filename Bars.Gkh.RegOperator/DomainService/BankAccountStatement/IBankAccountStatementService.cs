namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Сервис для банковской выписки
    /// </summary>
    public interface IBankAccountStatementService
    {
        /// <summary>
        /// Привязать документ.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult LinkDocument(BaseParams baseParams);

        /// <summary>
        /// Показать детализацию.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListDetail(BaseParams baseParams);

        /// <summary>
        /// Сохранить выписку с документом.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult SaveStatement(BaseParams baseParams);

        /// <summary>
        /// Список доступных Р/С для распределения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список Р/С</returns>
        IDataResult ListAccountNumbers(BaseParams baseParams);

        /// <summary>
        /// Экспорт в Excel
        /// </summary>
        /// <param name="viewModel">Вьюмодель</param>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список Р/С</returns>
        ActionResult Export(IViewModel<BankAccountStatement> viewModel, BaseParams baseParams);
    }
}