using DbfDataReader;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.RegOperator.Utils;

/// <summary>
/// Методы расширения для библиотеки DbfDataReader
/// </summary>
public static class DbfDataReaderExtensions
{
    /// <summary>
    /// Считать строку, как объект модели
    /// </summary>
    /// <param name="dbfRecord"><see cref="DbfRecord"/>Текущая запись</param>
    /// <param name="table"><see cref="DbfRecord"/>Таблица, из которой считываются данные</param>
    /// <typeparam name="T">Тип модели, в которую будет преобразована строка</typeparam>
    /// <returns>Объект модели</returns>
    public static T ReadToObject<T>(this DbfRecord dbfRecord, DbfTable table)
        where T : new()
    {
        var type = typeof(T);
        var item = new T();

        var properties = type.GetProperties()
            .Where(x => x.GetCustomAttributes(true).Any(a => a is DbfColumnAttribute))
            .ToList();

        var modelColumns = properties
            .Select(x => (DbfColumnAttribute)x.GetCustomAttributes(typeof(DbfColumnAttribute), true).First())
            .Select(x => x.Name)
            .ToList();
        
        var tableColumns = table.Columns.Select(col => col.ColumnName).ToList();
        for (var i = 0; i < properties.Count; i++)
        {
            var property = properties[i];

            var value = dbfRecord.GetValueOrDefault(modelColumns[i], tableColumns);
            
            property.SetValue(item, value);
        }

        return item;
    }
    
    /// <summary>
    /// Получить значение их строки, соответствующее заданной колонке
    /// </summary>
    /// <param name="dbfRecord"><see cref="DbfRecord"/>Запись</param>
    /// <param name="column"><see cref="DbfRecord"/>Колонка, из которой мы получаем значение</param>
    /// <param name="columnNames"><see cref="DbfRecord"/>Список колонок таблицы</param>
    /// <returns>Значение</returns>
    public static object GetValueOrDefault(this DbfRecord dbfRecord, string column, List<string> columnNames)
    {
        for (int i = 0; i < dbfRecord.Values.Count; i++)
        {
            if (column == columnNames[i])
            {
                return dbfRecord.Values[i].GetValue();
            }
        }
        
       return null;
    }
}