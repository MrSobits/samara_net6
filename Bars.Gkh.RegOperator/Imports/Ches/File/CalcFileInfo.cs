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
    /// Файл импорта для начислений
    /// </summary>
    public class CalcFileInfo : PeriodImportFileInfo<CalcFileInfo.Row>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="period">Текущий расчетный период</param>
        /// <param name="indicate"></param>
        public CalcFileInfo(
            FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                new Dictionary<string, Tuple<int, bool>>
                {
                    {"LsNum", Tuple.Create(0, true)},
                    {"PeriodDay", Tuple.Create(1, true)},
                    {"SaldoIn", Tuple.Create(2, true)},
                    {"SaldoInTr", Tuple.Create(3, true)},
                    {"SaldoInBase", Tuple.Create(4, true)},
                    {"SaldoInPeni", Tuple.Create(5, true)},
                    {"ChargeTr", Tuple.Create(6, true)},
                    {"ChargeBase", Tuple.Create(7, true)},
                    {"ChargePeni", Tuple.Create(8, true)},
                    {"ChangeTr", Tuple.Create(9, true)},
                    {"ChangeBase", Tuple.Create(10, true)},
                    {"ChangePeni", Tuple.Create(11, true)},
                    {"RecalcTr", Tuple.Create(12, true)},
                    {"RecalcBase", Tuple.Create(13, true)},
                    {"RecalcPeni", Tuple.Create(14, true)},
                    {"PaymentTr", Tuple.Create(15, true)},
                    {"PaymentBase", Tuple.Create(16, true)},
                    {"PaymentPeni", Tuple.Create(17, true)},
                    {"SaldoOuth", Tuple.Create(18, true)},
                    {"SaldoOuthTr", Tuple.Create(19, true)},
                    {"SaldoOuthBase", Tuple.Create(20, true)},
                    {"SaldoOuthPeni", Tuple.Create(21, true)},

                    {"IsImported", Tuple.Create(22, false)},
                    {"Id", Tuple.Create(23, false)},
                    {"IsValid", Tuple.Create(24, false)}
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

            public int PeriodDay { get; set; }

            public decimal SaldoIn { get; set; }
            public decimal SaldoInTr { get; set; }
            public decimal SaldoInBase { get; set; }
            public decimal SaldoInPeni { get; set; }

            public decimal ChargeTr { get; set; }
            public decimal ChargeBase { get; set; }
            public decimal ChargePeni { get; set; }

            public decimal ChangeTr { get; set; }
            public decimal ChangeBase { get; set; }
            public decimal ChangePeni { get; set; }

            public decimal RecalcTr { get; set; }
            public decimal RecalcBase { get; set; }
            public decimal RecalcPeni { get; set; }

            public decimal PaymentTr { get; set; }
            public decimal PaymentBase { get; set; }
            public decimal PaymentPeni { get; set; }

            public decimal SaldoOuth { get; set; }
            public decimal SaldoOuthTr { get; set; }
            public decimal SaldoOuthBase { get; set; }
            public decimal SaldoOuthPeni { get; set; }

            [Ignore]
            public bool IsImported { get; set; }
            [Ignore]
            public bool IsValid { get; set; }

            [PrimaryKey]
            [Ignore]
            public long Id { get; set; }
        }

        public class SaldoCheckDto
        {
            public long Id { get; set; }
            public long PeriodSummaryId { get; set; }
            public string Municipality { get; set; }
            public string Address { get; set; }
            public string LsNum { get; set; }
            public string ChesLsNum { get; set; }
            public decimal Saldo { get; set; }
            public decimal ChesSaldo { get; set; }
            public decimal DiffSaldo { get; set; }
            public bool IsImported { get; set; }
        }
    }
}