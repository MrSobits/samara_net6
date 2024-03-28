namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using Bars.B4;

    /// <summary>
    /// Билдер обработчика импорта с временными таблицами
    /// </summary>
    public interface IChesTempDataProviderBuilder
    {
        /// <summary>
        /// Задать параметры
        /// </summary>
        /// <param name="baseParams"></param>
        IChesTempDataProviderBuilder SetParams(BaseParams baseParams);

        /// <summary>
        /// Получить импортер
        /// </summary>
        /// <param name="fileInfo"><see cref="ImportFileInfo"/></param>
        /// <returns></returns>
        IChesTempDataProvider Build(IPeriodImportFileInfo fileInfo);
    }
}