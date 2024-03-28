namespace Bars.Gkh.RegOperator.DomainService.Import
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;

    /// <summary>
    /// Сервис для работы с импортами в закрытый период
    /// </summary>
    public interface IClosedPeriodsImportService
    {
        /// <summary>
        /// Подтвердить атоматическое сопоставление лицевого счёта
        /// </summary>
        /// <param name="baseParams">Параметры от браузера. В частности, warnings - перечень идентификаторов записей журнала.</param>
        /// <returns>Удалось ли подтвердить</returns>
        ActionResult ConfirmAutoComparePersonalAccount(BaseParams baseParams);

        /// <summary>
        /// Получить список логов
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        IDataResult LogsList(BaseParams baseParams);

        /// <summary>
        /// Сопоставить лицевой счёт вручуню
        /// </summary>
        /// <param name="baseParams">Параметры от браузера. В частности warningId и compareToAccountId: какую запись журанала на какой ЛС сопоставить.</param>
        /// <returns>Удалось ли сопоставить</returns>
        ActionResult ManualComparePersonalAccount(BaseParams baseParams);

        /// <summary>
        /// Повторить импорт
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Удалось ли запустить</returns>
        ActionResult ReImport(BaseParams baseParams);

        /// <summary>
        /// Получить список задач по разбору импорта, выполняемых в текущий момент на сервере вычислений
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        IDataResult RunningTasksList(BaseParams baseParams);
    }
}
