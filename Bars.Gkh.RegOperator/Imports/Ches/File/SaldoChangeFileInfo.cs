namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Caching;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Файл импорта для изменения сальдо
    /// </summary>
    public class SaldoChangeFileInfo : PeriodImportFileInfo<SaldoChangeFileInfo.Row>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="year">Год начислений</param>
        /// <param name="month">Месяц начислений</param>
        /// <param name="period">Текущий расчетный период</param>
        public SaldoChangeFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                new Dictionary<string, Tuple<int, bool>>
                {
                    {"LsNum", Tuple.Create(0, true)},
                    {"SaldoChangeBase", Tuple.Create(1, false)},
                    {"SaldoChangeTr", Tuple.Create(2, true)},
                    {"SaldoChangePeni", Tuple.Create(3, true)},
                    {"ChangeType", Tuple.Create(4, true)},
                    {"ChangeMonth", Tuple.Create(5, false)},
                    {"ChangeDate", Tuple.Create(6, true)},
                    {"IsValid", Tuple.Create(7, false)},
                    {"Id", Tuple.Create(8, false)}
                },
                new Dictionary<string, int>
                {
                    { "LsNum", 20 }
                })
                .AddIndex("LsNum");
        }

        /// <summary>
        /// Добавить строку в <see cref="Rows" />
        /// </summary>
        /// <param name="data">Данные строки</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <returns>Результат выполнения</returns>
        protected override bool AddRow(string[] data, int rowNumber)
        {
            var row = this.Provider.GetObject(data, rowNumber, this.AddRowExtractError);
            if (row != null && row.IsValid)
            {
                row.RowNumber = rowNumber;
                this.Rows.Add(row);

                if (row.ChangeType == TypeChargeSource.ImportCancelCharge && !row.ChangeMonth.HasValue)
                {
                    this.AddFieldRequiredError("change_month", row);
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Строка из файла импорта
        /// </summary>
        public class Row : IRow
        {
            /// <summary>
            /// Номер строки
            /// </summary>
            public int RowNumber { get; set; }

            /// <summary>
            /// Номер ЛС
            /// </summary>
            public string LsNum { get; set; }

            /// <summary>
            /// Изменение сальдо по базовому тарифу
            /// </summary>
            public decimal SaldoChangeBase { get; set; }

            /// <summary>
            /// Изменение сальдо по тарифу решения
            /// </summary>
            public decimal SaldoChangeTr { get; set; }

            /// <summary>
            /// Изменение сальдо по пени
            /// </summary>
            public decimal SaldoChangePeni { get; set; }

            /// <summary>
            /// Тип операции изменения
            /// </summary>
            public TypeChargeSource ChangeType { get; set; }

            /// <summary>
            /// Месяц, за который произведены изменения
            /// </summary>
            public DateTime? ChangeMonth { get; set; }

            /// <summary>
            /// Дата операции
            /// </summary>
            public DateTime ChangeDate { get; set; }

            /// <summary>
            /// Проверка пройдена
            /// </summary>
            [Ignore]
            public bool IsValid { get; set; }

            /// <summary>
            /// Id
            /// </summary>
            [PrimaryKey]
            [Ignore]
            public long Id { get; set; }
        }
    }
}