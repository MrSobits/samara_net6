namespace Bars.Gkh.Import
{
    using System.Collections.Generic;
    using B4.Modules.Tasks.Common.Service;
    using Bars.B4;

    /// <summary>
    /// Интерфейс для управлениями импортами
    /// </summary>
    public interface IGkhImportService : ITaskProvider
    {
        /// <summary>
        /// Вернуть список импортов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        List<GkhImportInfo> GetImportInfoList(BaseParams baseParams);

        /// <summary>
        /// Поиск необходимого импорта и его запуск
        /// </summary>
        IDataResult Import(BaseParams baseParams);

        /// <summary>
        /// Запуск множественного импорта файлов
        /// </summary>
        IDataResult MultiImport(BaseParams baseParams);
    }
}