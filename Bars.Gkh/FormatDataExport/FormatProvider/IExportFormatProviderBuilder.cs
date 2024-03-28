namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Билдер провайдера экспорта
    /// </summary>
    public interface IExportFormatProviderBuilder
    {
        /// <summary>
        /// Задать лог операции
        /// </summary>
        IExportFormatProviderBuilder SetLogOperation(LogOperation logOperation);

        /// <summary>
        /// Задать токен отмены
        /// </summary>
        IExportFormatProviderBuilder SetCancellationToken(CancellationToken token);

        /// <summary>
        /// Задать параметры создания
        /// </summary>
        IExportFormatProviderBuilder SetParams(BaseParams baseParams);

        /// <summary>
        /// Задать список экспортируемых секций
        /// </summary>
        IExportFormatProviderBuilder SetEntytyGroupCodeList(IList<string> entityGroupCodeList);

        /// <summary>
        /// Создать провайдер
        /// </summary>
        /// <typeparam name="T"><see cref="BaseFormatProvider"/></typeparam>
        IExportFormatProvider Build<T>() where T : BaseFormatProvider, new();
    }
}