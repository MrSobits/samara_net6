namespace Bars.GkhExcel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    using B4.Utils;
    using Bars.B4;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    using Syncfusion.XlsIO;

    public class GkhExcelProvider : IGkhExcelProvider
    {
        private ExcelEngine excelEngine;

        private ExcelVersion defaultVersion = ExcelVersion.Excel97to2003;

        public IWindsorContainer Container { get; set; }

        public void UseVersionXlsx()
        {
            this.defaultVersion = ExcelVersion.Excel2007;
        }

        public void Open(Stream stream)
        {
            this.excelEngine = new ExcelEngine();
            this.excelEngine.Excel.DefaultVersion = this.defaultVersion;

            try
            {
                this.excelEngine.Excel.Workbooks.Open(stream);
            }
            catch (Exception exp)
            {
                this.excelEngine = null;
                this.Container.Resolve<ILogger>().LogError(exp, "Загрузка Excel файла".Localize());
            }
        }

        public bool IsEmpty(int workbook, int worksheet)
        {
            if (this.excelEngine == null)
            {
                return true;
            }

            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            return page == null || page.Rows.Length == 0;
        }

        public List<GkhExcelCell[]> GetRows(int indexWorkbook, int indexWorksheet, WorksheetVisibility worksheetVisibility = WorksheetVisibility.Visible)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            var result = new List<GkhExcelCell[]>();

            var allWorksheets = this.excelEngine.Excel.Workbooks[indexWorkbook].Worksheets;

            var worksheets = new List<IWorksheet>();
            foreach (IWorksheet sheet in allWorksheets)
            {
                switch (worksheetVisibility)
                {
                    case WorksheetVisibility.Hidden:
                        if (sheet.Visibility == Syncfusion.XlsIO.WorksheetVisibility.Hidden)
                        {
                            worksheets.Add(sheet);
                        }

                        break;
                    case WorksheetVisibility.Visible:
                        if (sheet.Visibility == Syncfusion.XlsIO.WorksheetVisibility.Visible)
                        {
                            worksheets.Add(sheet);
                        }

                        break;
                    case WorksheetVisibility.StrongHidden:
                        if (sheet.Visibility == Syncfusion.XlsIO.WorksheetVisibility.StrongHidden)
                        {
                            worksheets.Add(sheet);
                        }

                        break;
                }
            }

            var worksheet = worksheets[indexWorksheet];

            foreach (var row in worksheet.Rows)
            {
                if (row.Cells.Length == 0)
                {
                    continue;
                }

                var cells = new GkhExcelCell[row.Cells.Length];
                for (var index = 0; index < row.Cells.Length; index++)
                {
                    var cell = row.Cells[index];
                    cells[index] = new GkhExcelCell(this.ExtractCellValue(cell), cell.IsMerged, cell.CellStyle.Font.Bold);
                }

                result.Add(cells);
            }

            return result;
        }

        public GkhExcelCell[] GetRow(int workbook, int worksheets, int rowIndex)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheets];
            var row = page.Rows[rowIndex];

            if (row.Cells.Length == 0)
            {
                return new GkhExcelCell[0];
            }

            var cells = new GkhExcelCell[row.Cells.Length];
            for (var index = 0; index < row.Cells.Length; index++)
            {
                var cell = row.Cells[index];
                cells[index] = new GkhExcelCell(this.ExtractCellValue(cell), cell.IsMerged, cell.CellStyle.Font.Bold);
            }

            return cells;
        }

        public int GetRowsCount(int workbook, int worksheets)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheets];
            return page.Rows.Length;
        }

        public int GetColumnsCount(int workbook, int worksheets)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheets];
            return page.Columns.Length;
        }

        public int GetWorkSheetsCount(int workbook)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            return this.excelEngine.Excel.Workbooks[workbook].Worksheets.Count;
        }

        public GkhExcelCell GetCell(int workbook, int worksheets, int rowIndex, int cellIndex)
        {
            if (this.excelEngine == null || this.excelEngine.Excel == null)
            {
                throw new Exception("Не загружен Excel файл");
            }

            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheets];
            var cell = page.Rows[rowIndex].Cells[cellIndex];

            return new GkhExcelCell(this.ExtractCellValue(cell), cell.IsMerged, cell.CellStyle.Font.Bold);
        }

        /// <summary>
        /// Снять защиту с ячейки
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="row">Номер строки</param>
        /// <param name="cell">Номер столбца</param>
        public void UnlockCell(int workbook, int worksheet, int row, int cell)
        {
            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            page.Rows[row].Cells[cell].CellStyle.Locked = false;
        }

        /// <summary>
        /// Снять защиту с диапазона ячеек
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        public void UnlockCellRange(int workbook, int worksheet, string range)
        {
            var page = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            page.Range[range].CellStyle.Locked = false;
        }

        /// <summary>
        /// Защитить лист
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="password">Пароль для снятия защиты</param>
        public void ProtectSheet(int workbook, int worksheet, string password)
        {
            var sheet = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            sheet.Protect(password);
        }

        /// <summary>
        /// Сохранить содержимое в поток
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="outputStream">Поток для сохранения</param>
        public void SaveAs(int workbook, Stream outputStream)
        {
            var excelWorkbook = this.excelEngine.Excel.Workbooks[workbook];

            excelWorkbook.Version = this.defaultVersion;
            excelWorkbook.SaveAs(outputStream);
        }

        /// <summary>
        /// Импортировать данные через DataTable
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="firstRow">Номер строки, с которой начинать импорт</param>
        /// <param name="firstColumn">Номер колонки, с которой начинать импорт</param>
        /// <param name="data">Данные</param>
        public void ImportDataTable(int workbook, int worksheet, int firstRow, int firstColumn, DataTable data)
        {
            var sheet = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            sheet.ImportDataTable(data, false, firstRow, firstColumn);
        }

        /// <summary>
        /// Добавить формулу
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        /// <param name="formula">Формула</param>
        public void AddFormula(int workbook, int worksheet, string range, string formula)
        {
            var sheet = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];
            sheet.Range[range].Formula = formula;
        }

        /// <summary>
        /// Добавить внутренню и внешнюю рамку для диапазона ячеек
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        public void AddTableBorder(int workbook, int worksheet, string range)
        {
            var sheet = this.excelEngine.Excel.Workbooks[workbook].Worksheets[worksheet];

            sheet.Range[range].BorderAround(ExcelLineStyle.Thin);
            sheet.Range[range].BorderInside(ExcelLineStyle.Thin);
        }

        public void Dispose()
        {
            this.excelEngine.Dispose();
        }

        /// <summary>
        /// Извлечь значение ячейки
        /// </summary>
        /// <remarks>
        /// 1. Учитывается, что в ячейках могут быть формулы.
        /// 2. Учитывается, что дата извлекается некорректно. Делается преобразование даты.
        /// </remarks>
        /// <param name="cell">Ячейка</param>
        /// <returns>Текстовое представление значения</returns>
        private string ExtractCellValue(IRange cell)
        {
            if (!cell.Formula.IsEmpty())
            {
                return cell.FormulaStringValue;
            }
            return cell.Value;
        }
    }

    public enum WorksheetVisibility
    {
        Visible,
        Hidden,
        StrongHidden
    }
}