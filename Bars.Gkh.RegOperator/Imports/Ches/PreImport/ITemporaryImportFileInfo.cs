namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Файл с периодическими данными
    /// </summary>
    public interface IPeriodImportFileInfo
    {
        /// <summary>
        /// Период начислений
        /// </summary>
        ChargePeriod Period { get; }

        /// <summary>
        /// Вернуть колонки таблицы
        /// </summary>
        IDictionary<string, ColumnDef> GetColumns();

        /// <summary>
        /// Вернуть колонки таблицы
        /// </summary>
        SummaryColumn[] GetSummaryColumns();

        /// <summary>
        /// Тип файла
        /// </summary>
        FileType FileType { get; }

        /// <summary>
        /// Файл
        /// </summary>
        FileData FileData { get; }
    }

    /// <summary>
    /// Описание для колонки представления
    /// </summary>
    public class SummaryColumn
    {
        /// <summary>
        /// Имя свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Имя колонки
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Формула вычисления столбца sum(FORMULA)
        /// </summary>
        public string Formula { get; set; }
    }

    /// <summary>
    /// Описание колонки
    /// </summary>
    public class ColumnDef
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name => this.Column.Name;

        /// <summary>
        /// Тип
        /// </summary>
        public ColumnType ColumnType => this.Column.ColumnType;

        /// <summary>
        /// Описание колонки
        /// </summary>
        public Column Column { get; set; }

        /// <summary>
        /// Строить индекс
        /// </summary>
        public bool HasIndex { get; set; }

        /// <summary>
        /// Импортируемое поле
        /// </summary>
        public bool HasImport { get; set; }
    }
}