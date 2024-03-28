namespace Bars.GkhExcel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    public interface IGkhExcelProvider : IDisposable
    {
        void Open(Stream stream);

        bool IsEmpty(int workbook, int worksheet);

        void UseVersionXlsx();

        List<GkhExcelCell[]> GetRows(int indexWorkbook, int indexWorksheet, WorksheetVisibility worksheetVisibility = WorksheetVisibility.Visible);

        GkhExcelCell[] GetRow(int indexWorkbook, int indexWorksheets, int indexRow);

        int GetRowsCount(int indexWorkbook, int indexWorksheets);

        int GetColumnsCount(int indexWorkbook, int indexWorksheets);
        
        int GetWorkSheetsCount(int indexWorkbook);

        GkhExcelCell GetCell(int indexWorkbook, int indexWorksheet, int indexRow, int indexCell);

        /// <summary>
        /// Снять защиту с ячейки
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="row">Номер строки</param>
        /// <param name="cell">Номер столбца</param>
        void UnlockCell(int workbook, int worksheet, int row, int cell);

        /// <summary>
        /// Снять защиту с диапазона ячеек
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        void UnlockCellRange(int workbook, int worksheet, string range);

        /// <summary>
        /// Защитить лист
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="password">Пароль для снятия защиты</param>
        void ProtectSheet(int workbook, int worksheet, string password);

        /// <summary>
        /// Сохранить содержимое в поток
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="outputStream">Поток для сохранения</param>
        void SaveAs(int workbook, Stream outputStream);

        /// <summary>
        /// Импортировать данные через DataTable
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="firstRow">Номер строки, с которой начинать импорт</param>
        /// <param name="firstColumn">Номер колонки, с которой начинать импорт</param>
        /// <param name="data">Данные</param>
        void ImportDataTable(int workbook, int worksheet, int firstRow, int firstColumn, DataTable data);

        /// <summary>
        /// Добавить формулу
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        /// <param name="formula">Формула</param>
        void AddFormula(int workbook, int worksheet, string range, string formula);

        /// <summary>
        /// Добавить внутренню и внешнюю рамку для диапазона ячеек
        /// </summary>
        /// <param name="workbook">Номер книги</param>
        /// <param name="worksheet">Номер листа</param>
        /// <param name="range">Диапазон ячеек</param>
        void AddTableBorder(int workbook, int worksheet, string range);
    }
}
