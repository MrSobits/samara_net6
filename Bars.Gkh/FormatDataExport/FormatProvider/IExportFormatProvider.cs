namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Провайдер работы с данными в системе ЖКХ
    /// </summary>
    public interface IExportFormatProvider
    {
        /// <summary>
        /// Событие уведомляет о смене прогесса выполнения
        /// </summary>
        event EventHandler<float> OnProgressChanged;

        /// <summary>
        /// Событие уведомляет о завершении экспорта
        /// </summary>
        event EventHandler<ICollection<string>> OnAfterExport;

        /// <summary>
        /// Пользователь предоставляющий данные
        /// </summary>
        User User { get; set; }

        /// <summary>
        /// Роль пользователя предоставляющего данные
        /// </summary>
        Role UserRole { get; }

        /// <summary>
        /// Контрагент предоставляющий данные
        /// </summary>
        Contragent Contragent { get; set; }

        /// <summary>
        /// Параметры фильтрации данных
        /// </summary>
        DynamicDictionary DataSelectorParams { get; }

        /// <summary>
        /// Выгрузить данные
        /// </summary>
        /// <param name="pathToSave">Путь для сохранения</param>
        IDataResult Export(string pathToSave);

        /// <summary>
        /// Список служебных секций
        /// </summary>
        IList<string> ServiceEntityCodes { get; }

        /// <summary>
        /// Версия формата
        /// </summary>
        string FormatVersion { get; }

        /// <summary>
        /// Обобщенная информация об ошибках
        /// </summary>
        string SummaryErrors { get; }

        /// <summary>
        /// Коды экспортируемых сущностей
        /// </summary>
        IList<string> EntityCodeList { get; }

        /// <summary>
        /// Коды выбранных пользователем экспортируемых сущностей
        /// </summary>
        ICollection<string> SelectedEntityCodeList { get; set; }

        /// <summary>
        /// Токен отмены операции
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Логи операций
        /// </summary>
        LogOperation LogOperation { get; set; }
    }
}