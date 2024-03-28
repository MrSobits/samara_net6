using Bars.GkhExcel;
using System;
using System.Collections.Generic;

namespace Bars.Gkh.Helpers
{
    public interface IImportHelper
    {
        IGkhExcelProvider Excel { get;}

        /// <summary>
        /// Поиск заголовка как первой строки со всеми заполненными столбцами
        /// </summary>
        /// <param name="mincolumncount">Минимальное количество столбиков с заголовком</param>
        /// <param name="maxdepth">Количество перебираемых строк при поиске</param>
        /// <returns>Номер строки</returns>
        int FindHeader(int mincolumncount, int maxdepth);

        /// <summary>
        /// Поиск заголовка по значению
        /// </summary>
        /// <param name="value">Заголовок первого столбца</param>
        /// <param name="maxdepth">Количество перебираемых строк при поиске</param>
        /// <returns>Номер строки</returns>
        int FindHeaderByText(string value, int maxdepth);

        /// <summary>
        /// Собирает многострочный словарь заголовков (все значения через тире.Trim().ToLower()) - номер столбца, используя номер строки, найденный FindHeader
        /// Возвращаемый словарь не копия, можно править, и это учтется при парсинге
        /// </summary>
        Dictionary<string, int> GetComplexHeaders(int rowscount = 1);

        /// <summary>
        /// Собирает словарь заголовок(.Trim().ToLower()) - номер столбца, используя произвольный номер строки
        /// Возвращаемый словарь не копия, можно править, и это учтется при парсинге
        /// </summary>
        /// <param name="row">Номер строки</param>
        Dictionary<string, int> GetHeaders();
        Dictionary<string, int> GetHeaders(int row);

        /// <summary>
        /// Считать примитивное значение
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        T Read<T>(int row, string header, bool required = false);

        /// <summary>
        /// Считать значение DateTime
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        /// <param name="format">Формат даты для ParseExact</param>
        DateTime ReadDateTime(int row, string header, bool required = false, string format = null);

        /// <summary>
        /// Считать значение ateTime?
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        /// <param name="format">Формат даты для ParseExact</param>
        DateTime? ReadDateTimeNullable(int row, string header, bool required = false, string format = null);

        /// <summary>
        /// Считать значение Guid
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        Guid ReadGuid(int row, string header, bool required = false);

        /// <summary>
        /// Считать значение Guid?
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        Guid? ReadtGuidNullable(int row, string header, bool required = false);
        
    }
}
