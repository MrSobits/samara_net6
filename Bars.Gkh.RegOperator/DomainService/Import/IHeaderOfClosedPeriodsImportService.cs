namespace Bars.Gkh.RegOperator.DomainService.Import
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Entities;

    /// <summary>
    /// Сервис для работы с шапкой импорта в закрытый период
    /// </summary>
    public interface IHeaderOfClosedPeriodsImportService
    {
        /// <summary>
        /// Заполнить параметры для повторного импорта
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <param name="task">Задача сервера вычислений, которая разбирала импорт</param>
        void FillBaseParamsForReImport(BaseParams baseParams, TaskEntry task);
    }
}
