namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;

    /// <summary>
    /// Интерфейс сервиса для работы с данными Импорта ЧЭС
    /// </summary>
    public interface IChesImportService
    {
        /// <summary>
        /// Вернуть информацию по начислениям за период
        /// </summary>
        IDataResult ListChargeInfo(BaseParams baseParams);

        /// <summary>
        /// Вернуть информацию по оплатам за период
        /// </summary>
        IDataResult ListPaymentInfo(BaseParams baseParams);

        /// <summary>
        /// Вернуть информацию по изменениям сальдо за период
        /// </summary>
        IDataResult ListSaldoChangeInfo(BaseParams baseParams);

        /// <summary>
        /// Вернуть информацию по перерасчетам за период
        /// </summary>
        IDataResult ListRecalcInfo(BaseParams baseParams);

        /// <summary>
        /// Экспортировать данные
        /// </summary>
        IDataResult Export(BaseParams baseParams);

        /// <summary>
        /// Вернуть импортер указанного типа файла
        /// </summary>
        IChesTempDataProvider GetImporter(BaseParams baseParams, FileType fileType);

        /// <summary>
        /// Удалить загруженную секцию
        /// </summary>
        IDataResult DeleteSection(BaseParams baseParams);

        /// <summary>
        /// Получить дни оплат за период
        /// </summary>
        /// <param name="baseParams">periodId - идентификатор периода</param>
        IList<int> GetPaymentDays(BaseParams baseParams);

        /// <summary>
        /// Получить оплаты за период
        /// </summary>
        /// <param name="baseParams">periodId - идентификатор периода</param>
        IDataResult PaymentsList(BaseParams baseParams);

        /// <summary>
        /// Получить сверку сальдо за период
        /// </summary>
        /// <param name="baseParams">periodId - идентификатор периода</param>
        IDataResult ListSaldoCheck(BaseParams baseParams);

        /// <summary>
        /// Получить сверку сальдо за период
        /// </summary>
        /// <param name="baseParams">
        /// periodId - идентификатор периода
        /// ids - идентификаторы записей начислений <see cref="CalcFileInfo"/>
        /// </param>
        IDataResult ImportSaldo(BaseParams baseParams);

        /// <summary>
        /// Запустить проверку 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult RunCheck(BaseParams baseParams);
    }
}