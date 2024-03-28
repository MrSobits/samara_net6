namespace Bars.Gkh.RegOperator.DomainService.BankDocumentImport
{
    using System.Collections.Generic;

    using B4;

    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс сервиса для документов, загруженных из банка
    /// </summary>
    public interface IBankDocumentImportService : ICancellableSourceProvider
    {
        /// <summary>
        /// Задача для подтверждения оплат реестра
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат подтверждения оплат реестра</returns>
        IDataResult TaskAcceptDocuments(BaseParams baseParams);

        /// <summary>
        /// Задача для проверки реестра оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат проверки</returns>
        IDataResult TaskCheckPayments(BaseParams baseParams);

        /// <summary>
        /// Задача для отмены подтверждения оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат отмены подтверждения оплат</returns>
        IDataResult TaskCancelDocuments(BaseParams baseParams);

        /// <summary>
        /// Подтверждение оплат реестра
        /// </summary>
        /// <param name="bankDocumentImports">Список документов</param>
        /// <param name="indicator">Индикация процесса подтвреждения</param>
        /// <returns>Результат подтверждения оплат реестра</returns>
        IDataResult AcceptDocuments(List<BankDocumentImport> bankDocumentImports, IProgressIndicator indicator);

        /// <summary>
        /// Подтверждение внутренних оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат подтверждения внутренних оплат</returns>
        IDataResult AcceptInternalPayments(BaseParams baseParams);

        /// <summary>
        /// Отмена внутренних оплат реестра
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат подтверждения внутренних оплат</returns>
        IDataResult CancelInternalPayments(BaseParams baseParams);

        IDataResult CancelInternalPayments(
            long[] importedPaymentIds,
            BankDocumentImport bankDocumentImport,
            ChargePeriod chargePeriod,
            IProgressIndicator progressIndicator = null);

        /// <summary>
        /// Отмена подтвержденных оплат реестра
        /// </summary>
        /// <param name="bankDocumentImports">Реестры оплат для отмены</param>
        /// <param name="indicator">Индикатор процесса</param>
        /// <returns>Результат отмены подтвержденных оплат реестра</returns>
        IDataResult CancelPayments(List<BankDocumentImport> bankDocumentImports, IProgressIndicator indicator = null);

        /// <summary>
        /// Удаление реестра оплат
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат удаления реестра оплат</returns>
        IDataResult DeletePayments(BaseParams baseParams);

        /// <summary>
        /// Подтвердить выбранные платежи в пакете в реестре оплат
        /// </summary>
        /// <param name="importedPaymentIds">Идентификаторы платежей</param>
        /// <param name="bankDocumentImport">Документы, загруженные из банка</param>
        /// <param name="chargePeriod">Период начислений</param>
        /// <param name="progressIndicator">Индикатор прогресса</param>
        /// <returns>результат подтверждения</returns>
        IDataResult AcceptPayments(long[] importedPaymentIds, BankDocumentImport bankDocumentImport, ChargePeriod chargePeriod, IProgressIndicator progressIndicator = null);

        /// <summary>
        /// Проверка реестра оплат
        /// </summary>
        /// <param name="bankDocumentImports">Список документов</param>
        /// <returns>Результат операции</returns>
        IDataResult CheckPayments(List<BankDocumentImport> bankDocumentImports);

        /// <summary>
        /// Метод постановки импорта реестра
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат постановки задачи</returns>
        IDataResult TaskImport(BaseParams baseParams);

        /// <summary>
        /// Проставить верные статусы после неуспешного подтверждения реестра оплат
        /// </summary>
        /// <param name="document">Документ</param>
        /// <returns>Результат операции</returns>
        IDataResult SetPaymentsNotDistributed(BankDocumentImport document);
    }
}
