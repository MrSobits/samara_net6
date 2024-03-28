namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.GkhExcel;

    using FastMember;

    /// <summary>
    /// Выгрузка Сальдо ЛС в Excel через Excel Provider
    /// </summary>
    public class AccountSaldoCustomExcelReport : IDisposable
    {
        /// <summary>
        /// Шаблон
        /// </summary>
        protected byte[] Template => Resources.AccountSaldoReportExcelTemplate;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name => "Выгрузка сальдо";

        /// <summary>
        /// Описание
        /// </summary>
        public string Description => this.Name;

        private readonly IGkhExcelProvider excelProvider;
        private readonly IList<PersonalAccountSaldoInfo> data;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountSaldoCustomExcelReport(IGkhExcelProvider excelProvider, IList<PersonalAccountSaldoInfo> data)
        {
            this.excelProvider = excelProvider;
            this.data = data;
        }

        /// <summary>
        /// Получить отчет
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetReportStream()
        {
            var outputStream = new MemoryStream();

            this.excelProvider.UseVersionXlsx();
            this.excelProvider.Open(new MemoryStream(this.Template));

            var dataTable = new DataTable();
            using (var reader = ObjectReader.Create(
                this.data,
                "Municipality",
                "Address",
                "AccountNumber",
                "State",
                "SaldoByBaseTariff",
                "SaldoByDecisionTariff",
                "SaldoByPenalty"))
            {
                dataTable.Load(reader);
            }

            //1. импортируем данные
            //2. проставляем формулы
            //3. оформляем
            //4. снимаем защиту с редактируемых ячеек
            //5. защищаем лист
            //6. сохраняем в поток

            var workbook = 0;
            var worksheet = 0;
            var firstRow = 4;
            var firstCell = 1;

            this.excelProvider.ImportDataTable(workbook, worksheet, firstRow, firstCell, dataTable);

            var rowsCount = this.excelProvider.GetRowsCount(workbook, worksheet);
            for (int i = firstRow; i <= rowsCount; i++)
            {
                this.excelProvider.AddFormula(workbook, worksheet, $"H{i}", $"=E{i}+F{i}+G{i}");
            }

            this.excelProvider.AddTableBorder(workbook, worksheet, $"A{firstRow - 1}:H{rowsCount}");
            
            this.excelProvider.UnlockCellRange(workbook, worksheet, $"A1:G{rowsCount}");
            this.excelProvider.ProtectSheet(workbook, worksheet, password: "");

            this.excelProvider.SaveAs(workbook, outputStream);

            return outputStream;
        }

        public void Dispose()
        {
            this.excelProvider.Dispose();
        }
    }
}