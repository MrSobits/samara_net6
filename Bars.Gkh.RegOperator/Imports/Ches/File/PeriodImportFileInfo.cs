namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Базовый класс для файла импорта с периодом
    /// </summary>
    public abstract class PeriodImportFileInfo<TRow> : ImportFileInfo<TRow>, IPeriodImportFileInfo 
        where TRow : class, IRow, new()
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="period">Текущий расчетный период</param>
        protected PeriodImportFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, indicate)
        {
            this.Period = period;
        }

        /// <summary>
        /// Текущий расчетный период
        /// </summary>
        public ChargePeriod Period { get; }

        /// <summary>
        /// Вернуть список атрибутов для создания временной таблицы
        /// </summary>
        public virtual IDictionary<string, ColumnDef> GetColumns()
        {
            return this.Provider.GetProperties()
                .ToDictionary(
                    x => x.PropertyName,
                    x => new ColumnDef
                    {
                        Column = x.IsPrimary
                        ? new Column(x.PropertyName.ToUpper(),
                            MigratorUtils.ConvertToDbType(x.PropertyType),
                            ColumnProperty.PrimaryKeyWithIdentity)
                        : new Column(
                                x.PropertyName.ToUpper(),
                                MigratorUtils.ConvertToDbType(x.PropertyType),
                                x.Required // или поле необязательное или nullable тип
                                    ? ColumnProperty.NotNull
                                    : ColumnProperty.Null)
                            .With(column =>
                            {
                                // если указана длина, то передаем её
                                if (x.Length > 0)
                                {
                                    column.ColumnType.Length = x.Length;
                                }

                                return column;
                            }),
                        HasIndex = x.HasIndex,
                        HasImport = !x.IsIgnore
                    });
        }

        /// <inheritdoc />
        public virtual SummaryColumn[] GetSummaryColumns()
        {
            return this.GetColumns()
                .Where(x => x.Value.ColumnType.DataType == DbType.Decimal)
                .Select(x => new SummaryColumn
                {
                    PropertyName = x.Key,
                    ColumnName = x.Value.Name,
                    Formula = x.Value.Name
                })
                .ToArray();
        }
    }
}