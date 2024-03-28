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
    /// Файл импорта для перерасчетов
    /// </summary>
    public class RecalcFileInfo : PeriodImportFileInfo<RecalcFileInfo.Row>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="period">Текущий расчетный период</param>
        public RecalcFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                new Dictionary<string, Tuple<int, bool>>
                {
                    {"LsNum", Tuple.Create(0, true)},
                    {"BaseRecalc", Tuple.Create(1, false)},
                    {"TariffDecisionRecalc", Tuple.Create(2, true)},
                    {"PenaltyRecalcRecalc", Tuple.Create(3, true)},
                    {"RecalcMonth", Tuple.Create(4, true)},
                    {"OperationDate", Tuple.Create(5, false)},
                    {"IsValid", Tuple.Create(6, false)},
                    {"Id", Tuple.Create(7, false)}
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
            public decimal BaseRecalc { get; set; }
            public decimal TariffDecisionRecalc { get; set; }
            public decimal PenaltyRecalcRecalc { get; set; }
            public DateTime RecalcMonth { get; set; }
            public DateTime? OperationDate { get; set; }
            [Ignore]
            public bool IsValid { get; set; }
            [PrimaryKey]
            [Ignore]
            public long Id { get; set; }
        }
    }
}