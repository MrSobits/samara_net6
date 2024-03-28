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
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;

    /// <summary>
    /// Импорт оплат
    /// </summary>
    public class PayFileInfo : PeriodImportFileInfo<PayFileInfo.Row>
    {
        /// <summary>
        /// Дата загрузки оплат
        /// </summary>
        public DateTime? PaymentDay { get; set; }

        /// <summary>
        /// Версия файла
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="year">Год начислений</param>
        /// <param name="month">Месяц начислений</param>
        /// <param name="period">Текущий расчетный период</param>
        public PayFileInfo(FileType fileType, FileData fileData, ILogImport logImport, ChargePeriod period, Action<int, string> indicate)
            : base(fileType, fileData, logImport, period, indicate)
        {
            this.Provider = new DataProvider(
                new Dictionary<string, Tuple<int, bool>>
                {
                    {"LsNum", Tuple.Create(0, true)},
                    {"PaymentDate", Tuple.Create(1, true)},
                    {"TariffPayment", Tuple.Create(2, true)},
                    {"TariffDecisionPayment", Tuple.Create(3, true)},
                    {"PenaltyPayment", Tuple.Create(4, true)},
                    {"PaymentType", Tuple.Create(5, true)},
                    {"RegistryNum", Tuple.Create(6, false)},
                    {"RegistryDate", Tuple.Create(7, true)},

                    {"Id", Tuple.Create(12, false)},
                    {"PaymentDay", Tuple.Create(8, false)},
                    {"Version", Tuple.Create(9, false)},
                    {"IsValid", Tuple.Create(10, false)},
                    {"Reason", Tuple.Create(11, false)},
                    {"IsImported", Tuple.Create(12, false)},
                    {"State", Tuple.Create(13, false)},
                },
                new Dictionary<string, int>
                {
                    { "LsNum", 20 }
                })
                .AddIndex("LsNum");
        }

        /// <inheritdoc />
        public override SummaryColumn[] GetSummaryColumns()
        {
            return new []
            {
                new SummaryColumn
                {
                    PropertyName = "PaidBase",
                    ColumnName = "PAIDBASE",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.Payment} THEN TARIFFPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "CancelBase",
                    ColumnName = "CANCELBASE",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.CancelPayment} THEN TARIFFPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "RefundBase",
                    ColumnName = "REFUNDBASE",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.ReturnPayment} THEN TARIFFPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "PaidTr",
                    ColumnName = "PAIDTR",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.Payment} THEN TARIFFDECISIONPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "CancelTr",
                    ColumnName = "CANCELTR",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.CancelPayment} THEN TARIFFDECISIONPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "RefundTr",
                    ColumnName = "REFUNDTR",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.ReturnPayment} THEN TARIFFDECISIONPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "PaidPeni",
                    ColumnName = "PAIDPENI",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.Payment} THEN PENALTYPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "CancelPeni",
                    ColumnName = "CANCELPENI",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.CancelPayment} THEN PENALTYPAYMENT ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "RefundPeni",
                    ColumnName = "REFUNDPENI",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.ReturnPayment} THEN PENALTYPAYMENT ELSE 0 END)"
                },

                new SummaryColumn
                {
                    PropertyName = "PaymentCount",
                    ColumnName = "PAYMENTCOUNT",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.Payment} THEN 1 ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "CancelCount",
                    ColumnName = "CANCELCOUNT",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.CancelPayment} THEN 1 ELSE 0 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "ReturnCount",
                    ColumnName = "RETURNCOUNT",
                    Formula = $"(CASE PAYMENTTYPE WHEN {(int)ImportPaymentType.ReturnPayment} THEN 1 ELSE 0 END)"
                },

                new SummaryColumn
                {
                    PropertyName = "BaseCount",
                    ColumnName = "BASECOUNT",
                    Formula = "(CASE TARIFFPAYMENT WHEN 0 THEN 0 ELSE 1 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "DecisionCount",
                    ColumnName = "DECISIONCOUNT",
                    Formula = "(CASE TARIFFDECISIONPAYMENT WHEN 0 THEN 0 ELSE 1 END)"
                },
                new SummaryColumn
                {
                    PropertyName = "PenaltyCount",
                    ColumnName = "PENALTYCOUNT",
                    Formula = "(CASE PENALTYPAYMENT WHEN 0 THEN 0 ELSE 1 END)"
                },
            };
        }

        public class Row : IRow
        {
            public int RowNumber { get; set; }
            public string LsNum { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal TariffDecisionPayment { get; set; }
            public decimal TariffPayment { get; set; }
            public decimal PenaltyPayment { get; set; }
            public ImportPaymentType PaymentType { get; set; }
            public string RegistryNum { get; set; }
            public DateTime RegistryDate { get; set; }

            [Ignore]
            [PrimaryKey]
            public long Id { get; set; }
            [Ignore]
            public int? PaymentDay { get; set; }
            [Ignore]
            public int? Version { get; set; }
            [Ignore]
            public bool IsValid { get; set; }
            [Ignore]
            public string Reason { get; set; }
            [Ignore]
            public bool IsImported { get; set; }
            [Ignore]
            public DateTime? ReportDate { get; set; }
            [Ignore]
            public ChesImportPaymentsState State { get; set; }
        }
    }
}