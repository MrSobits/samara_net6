namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Caching;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;

    /// <summary>
    /// Файл импорта для протоколов расчета
    /// </summary>
    public class CalcProtFileInfo : PeriodImportFileInfo<CalcProtFileInfo.Row>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="period">Текущий расчетный период</param>
        /// <param name="indicate"></param>
        public CalcProtFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                    new Dictionary<string, Tuple<int, bool>>
                    {
                        { "LsNum", Tuple.Create(0, true) },
                        { "PeriodStart", Tuple.Create(1, true) },
                        { "PeriodEnd", Tuple.Create(2, true) },
                        { "Tariff", Tuple.Create(3, true) },
                        { "CalcArea", Tuple.Create(4, true) },
                        { "Charge", Tuple.Create(5, true) },
                        { "IsValid", Tuple.Create(6, false) },
                        { "Id", Tuple.Create(7, false) },
                    },
                    new Dictionary<string, int>
                    {
                        { "LsNum", 20 }
                    })
                .AddIndex("LsNum");
        }

        /// <summary>
        /// Строка из файла импорта
        /// </summary>
        public class Row : IRow
        {
            public int RowNumber { get; set; }

            public string LsNum { get; set; }

            public DateTime PeriodStart { get; set; }

            public DateTime PeriodEnd { get; set; }

            public decimal Tariff { get; set; }

            public decimal CalcArea { get; set; }

            public decimal Charge { get; set; }

            [Ignore]
            public bool IsValid { get; set; }

            [PrimaryKey]
            [Ignore]
            public long Id { get; set; }

        }
    }
}