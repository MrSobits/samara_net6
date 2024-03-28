using Bars.GkhExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bars.Gkh.Helpers.Impl
{
    public class ImportHelper : IImportHelper
    {
        public IGkhExcelProvider Excel { get; set; }

        private int headerRow = -1;

        private Dictionary<string, int> headers = null;

        /// <summary>
        /// Получить количество строк в экселине
        /// </summary>
        /// <returns></returns>
        public int GetRowsCount(int workbook = 0, int worksheet = 0)
        {
            return Excel.GetRowsCount(0, 0);
        }

        /// <summary>
        /// Поиск заголовка как первой строки со всеми заполненными столбцами
        /// </summary>
        /// <param name="mincolumncount">Минимальное количество столбиков с заголовком</param>
        /// <param name="maxdepth">Количество перебираемых строк при поиске</param>
        /// <returns>Номер строки</returns>
        public int FindHeader(int mincolumncount, int maxdepth)
        {
            for (int row = 0; row < maxdepth; row++)
            {
                int column = 0;
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        var v = Excel.GetCell(0, 0, row, i).Value;
                    }
                    catch (Exception e)
                    {
                        mincolumncount = i - 1;
                        break;
                    }
                }
                while (column <= mincolumncount && !String.IsNullOrWhiteSpace(Excel.GetCell(0, 0, row, column).Value))
                {
                    column++;
                }
                if (column >8)
                {
                    headerRow = row;
                    return row;
                }
            }
            throw new ApplicationException($"Не найден заголовок по параметрам: минимальное число столбцов {mincolumncount}, число перебираемых строк {maxdepth}");
        }

        public int FindHeaderByText(string value, int maxdepth)
        {
            var columnscount = Excel.GetColumnsCount(0, 0);

            for (int row = 0; row < maxdepth; row++)
            {
                int column = 0;
                while (column < columnscount && Excel.GetCell(0, 0, row, column).Value.ToLower()!= value.ToLower())
                {
                    column++;
                }
                if (column < columnscount)
                {
                    headerRow = row;
                    return row;
                }
            }

            throw new ApplicationException($"Не найден заголовок по параметрам: значение {value}, число перебираемых строк {maxdepth}");
        }

        /// <summary>
        /// Собирает словарь заголовок(.Trim().ToLower()) - номер столбца, используя номер строки, найденный FindHeader
        /// Возвращаемый словарь не копия, можно править, и это учтется при парсинге
        /// </summary>
        public Dictionary<string, int> GetHeaders()
        {
            if (headerRow == -1)
                throw new ApplicationException($"Вызовите FindHeader перед сбором заголовка или используйте GetHeaders с явным указанием номера строки заголовков");

            var headers = new Dictionary<string, int>();

            int column = 0;
            var count = Excel.GetColumnsCount(0, 0);
            while (column < count)
            {
                var value = Excel.GetCell(0, 0, headerRow, column).Value.Trim().ToLower();
                if (String.IsNullOrEmpty(value))
                {
                    this.headers = headers;
                    return headers;
                }

                if (headers.ContainsKey(value))
                    throw new ApplicationException($"Дублированный заголовок столбца: {value}");

                headers.Add(value, column);

                column++;
            }

            this.headers = headers;
            return headers;
        }

        /// <summary>
        /// Собирает словарь заголовок(.Trim().ToLower()) - номер столбца, используя произвольный номер строки
        /// Возвращаемый словарь не копия, можно править, и это учтется при парсинге
        /// </summary>
        /// <param name="row">Номер строки</param>
        public Dictionary<string, int> GetHeaders(int row)
        {
            var headers = new Dictionary<string, int>();

            int column = 0;
            var count = Excel.GetColumnsCount(0, 0);
            while (column < count)
            {
                var value = Excel.GetCell(0, 0, row, column).Value.Trim().ToLower();
                if (String.IsNullOrEmpty(value))
                {
                    this.headers = headers;
                    return headers;
                }

                if (headers.ContainsKey(value))
                    throw new ApplicationException($"Дублированный заголовок столбца: {value}");

                headers.Add(value, column);

                column++;
            }

            this.headers = headers;
            return headers;
        }

        public Dictionary<string, int> GetComplexHeaders(int rowscount = 1)
        {
            if (headerRow == -1)
                throw new ApplicationException($"Вызовите FindHeader перед сбором заголовка или используйте GetHeaders с явным указанием номера строки заголовков");

            var headers = new Dictionary<string, int>();

            int column = 0;
            var count = Excel.GetColumnsCount(0, 0);
            while (column < count)
            {
                string value = null;
                for (int i = 0; i < rowscount; i++)
                {
                    var currentvalue = Excel.GetCell(0, 0, headerRow + i, column).Value.Trim().ToLower();
                    if (!String.IsNullOrEmpty(currentvalue))
                    {
                        if (value == null)
                            value = currentvalue;
                        else
                            value = value + "-"+currentvalue;
                    }
                }

                if (String.IsNullOrEmpty(value))
                {
                    column++;
                    continue;
                }                    

                if (headers.ContainsKey(value))
                    throw new ApplicationException($"Дублированный заголовок столбца: {value}");

                headers.Add(value, column++);                
            }

            this.headers = headers;
            return headers;
        }

        /// <summary>
        /// Считать примитивное значение
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        public T Read<T>(int row, string header, bool required = false)
        {
            if (headers == null)
                throw new ApplicationException($"Нет словаря заголовков. Сначала вызовите метод GetHeaders");

            string value = GetValue(row, header, required);

            try
            {
                //string
                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(value, typeof(T));

                //nullable primitive
                else if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    if (string.IsNullOrEmpty(value))
                        return default(T);

                    var type = Nullable.GetUnderlyingType(typeof(T));
                    var parseMethod = type.GetMethod("Parse", new[] { typeof(string) });
                    var result = parseMethod.Invoke(null, new object[] { value });

                    return (T)Convert.ChangeType(result, type);
                }
                //primitive        
                else
                {
                    var parse = typeof(T).GetMethod("Parse", new[] { typeof(string) });
                    return (T)Convert.ChangeType(parse.Invoke(null, new object[] { value }), typeof(T));
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Не удалось привести значение {value} к типу {GetDescription(typeof(T))}, cтолбец {header} : {e.Message}");
            }
        }

        /// <summary>
        /// Считать значение DateTime
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        /// <param name="format">Формат даты <see cref="DateTime.ParseExact"/></param>
        public DateTime ReadDateTime(int row, string header, bool required = false, string format = null)
        {
            if (headers == null)
                throw new ApplicationException($"Нет словаря заголовков. Сначала вызовите метод GetHeaders");

            string value = GetValue(row, header, required);
            if (string.IsNullOrWhiteSpace(value))
                throw new ApplicationException($"Пустое значение применяется к неnullable типу DateTime, cтолбец {header}");

            try
            {
                if (format == null)
                    return DateTime.Parse(value);
                else
                    return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Не удалось привести значение {value} к типу DateTime, cтолбец {header} : {e.Message}");
            }
        }

        /// <summary>
        /// Считать значение ateTime?
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        /// <param name="format">Формат даты <see cref="DateTime.ParseExact"/></param>
        public DateTime? ReadDateTimeNullable(int row, string header, bool required = false, string format = null)
        {
            if (headers == null)
                throw new ApplicationException($"Нет словаря заголовков. Сначала вызовите метод GetHeaders");

            string value = GetValue(row, header, required);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                if (format == null)
                    return DateTime.Parse(value);
                else
                    return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Не удалось привести значение {value} к типу DateTime?, cтолбец {header} : {e.Message}");
            }
        }

        /// <summary>
        /// Считать значение Guid
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        public Guid ReadGuid(int row, string header, bool required = false)
        {
            if (headers == null)
                throw new ApplicationException($"Нет словаря заголовков. Сначала вызовите метод GetHeaders");

            string value = GetValue(row, header, required);
            if (string.IsNullOrEmpty(value))
                throw new ApplicationException($"Пустое значение применяется к неnullable типу Guid, cтолбец {header}");

            try
            {
                return Guid.Parse(value);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Не удалось привести значение {value} к типу Guid, cтолбец {header} : {e.Message}");
            }
        }

        /// <summary>
        /// Считать значение Guid?
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="header">Заголовок столбца</param>
        /// <param name="required">Обязательный столбец?</param>
        public Guid? ReadtGuidNullable(int row, string header, bool required = false)
        {
            if (headers == null)
                throw new ApplicationException($"Нет словаря заголовков. Сначала вызовите метод GetHeaders");

            string value = GetValue(row, header, required);
            if (string.IsNullOrEmpty(value))
                return null;

            try
            {
                return Guid.Parse(value);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Не удалось привести значение {value} к типу Guid?, cтолбец {header} : {e.Message}");
            }
        }

        private string GetValue(int row, string header, bool required = false)
        {
            header = header.Trim().ToLower();

            if (headers.ContainsKey(header))
                return Excel.GetCell(0, 0, row, headers[header]).Value;

            var keyPairs = headers.Where(x => x.Key.Contains(header)).ToArray();
            if (keyPairs.Length > 1)
                throw new ApplicationException($"Не удалось однозначно сопоставить заголовок {header} со стобцом");
            else if (keyPairs.Length == 1)
                return Excel.GetCell(0, 0, row, keyPairs[0].Value).Value;
            else
            {
                if (required)
                    throw new ApplicationException($"Не найден обязательный столбец {header}");
                else
                    return "";
            }
        }


        private string GetDescription(Type type)
        {
            if (type == null)
                return "null";

            var u = Nullable.GetUnderlyingType(type);
            if (u == null)
                return type.Name;
            else
                return u.Name + "?";
        }

        //это не обязательно, мне просто нравится использовать хелпер через using
        public void Dispose()
        {
            Excel = null;
            headers = null;
        }
    }
}
