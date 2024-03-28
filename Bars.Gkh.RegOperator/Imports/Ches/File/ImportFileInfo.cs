namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Modules.Caching;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;

    public abstract class ImportFileInfo
    {
        /// <summary>
        /// Тип импортируемого файла
        /// </summary>
        public FileType FileType { get; }

        /// <summary>
        /// Содержимое файла импорта
        /// </summary>
        public FileData FileData { get; }

        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogImport LogImport { get; }

        /// <summary>
        /// Провалидировать файл импорта
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }

        public Action<int, string> Indicate { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="indicate"></param>
        protected ImportFileInfo(FileType fileType, FileData fileData, ILogImport logImport, Action<int, string> indicate)
        {
            this.FileType = fileType;
            this.FileData = fileData;
            this.LogImport = logImport;
            this.Indicate = indicate;
        }

        public abstract bool FillRows(StreamReader streamReader);

        /// <summary>
        /// Добавить строку в <see cref="Rows"/>
        /// </summary>
        /// <param name="data">Данные строки</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <returns>Результат выполнения</returns>
        protected abstract bool AddRow(string[] data, int rowNumber);

        /// <summary>
        /// Получить данные строки
        /// </summary>
        /// <param name="line">Строка</param>
        /// <returns>Данные строки</returns>
        protected virtual string[] GetRowData(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            return line.Split(';').Select(x => (x ?? "").Trim('"', ' ')).ToArray();
        }

        /// <summary>
        /// Добавить ошибку, полученную в результате разбора строки
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        protected virtual void AddRowExtractError(string fieldName, int rowNumber, string errorMessage)
        {
            this.LogImport.Error("Ошибка", $"Произошла ошибка при разборе строки. Поле {fieldName}. Ошибка: {errorMessage}. Строка {rowNumber}");
        }
    }

    /// <summary>
    /// Базовый класс для файла импорта
    /// </summary>
    public abstract class ImportFileInfo<TRow> : ImportFileInfo where TRow : class, IRow, new()
    {
        /// <summary>
        /// Список строк из файла импорта
        /// </summary>
        public IList<TRow> Rows { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileType">Тип импортируемого файла</param>
        /// <param name="fileData">Содержимое файла импорта</param>
        /// <param name="logImport">Логгер</param>
        /// <param name="indicate"></param>
        protected ImportFileInfo(FileType fileType, FileData fileData, ILogImport logImport, Action<int, string> indicate)
            : base(fileType, fileData, logImport, indicate)
        {
            this.Rows = new List<TRow>();
        }

        protected DataProvider Provider { get; set; }

        /// <summary>
        /// Заполнить <see cref="Rows"/>
        /// </summary>
        /// <returns></returns>
        public override bool FillRows(StreamReader streamReader)
        {
            var rowNumber = 0;
            while (!streamReader.EndOfStream)
            {
                rowNumber++;

                var line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var row = this.GetRowData(line);
                this.AddRow(row, rowNumber);
            }

            return false;
        }

        /// <inheritdoc />
        protected override bool AddRow(string[] data, int rowNumber)
        {
            var row = this.Provider.GetObject(data, rowNumber, this.AddRowExtractError);
            if (row != null && row.IsValid)
            {
                row.RowNumber = rowNumber;
                this.Rows.Add(row);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Добавить ошибку для обязательного незаполненного поля
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="row">Строка</param>
        protected virtual void AddFieldRequiredError(string fieldName, TRow row)
        {
            this.AddErrorRow(row, $"Не заполнено или имеет неверный формат обязательное поле {fieldName}. Строка {row.RowNumber}");
        }

        /// <summary>
        /// Добавить строку с ошибкой в файл
        /// </summary>
        /// <param name="rowEntity">Строка ошибкой</param>
        /// <param name="description"></param>
        /// <param name="args"></param>
        public void AddErrorRow(TRow rowEntity, string description, params string[] args)
        {
            if (this.LogImport.FileErrorsRow.Length == 0)
            {
                var header = this.Provider.GetProperties().Select(x => x.PropertyName).ToList();
                header.Add($"Описание ошибки{Environment.NewLine}");

                this.LogImport.AddErrorsHeader(header.ToArray());
            }

            this.LogImport.Error("Ошибка", description, args);
            this.LogImport.AddErrorsRow($"{this.Provider.ToRow(rowEntity)};{description}");
        }

        /// <summary>
        /// Экстрактор объектов из строк
        /// </summary>
        protected class DataProvider
        {
            private readonly IDictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();
            private readonly IDictionary<string, Tuple<int, bool>> propertyMapping;
            private readonly IDictionary<string, int> lenghtDict;
            private readonly HashSet<string> indexProperties;

            public DataProvider(Dictionary<string, Tuple<int, bool>> mapping, IDictionary<string, int> lenghtDict = null)
            {
                this.propertyMapping = mapping;
                this.lenghtDict = lenghtDict ?? new Dictionary<string, int>();
                this.indexProperties = new HashSet<string>();
            }

            public DataProvider AddIndex(string propertyName)
            {
                this.indexProperties.Add(propertyName);
                return this;
            }

            /// <summary>
            /// Извлечь объект из строки
            /// </summary>
            /// <returns></returns>
            public TRow GetObject(string[] splits, int rowNumber, Action<string, int, string> logAction)
            {
                try
                {
                    var obj = new TRow();
                    foreach (var mapping in this.propertyMapping)
                    {
                        var errorsMsg = this.TryExtractProperty(obj, mapping.Key, splits[mapping.Value.Item1], mapping.Value.Item2);
                        if (errorsMsg.IsNotEmpty())
                        {
                            logAction?.Invoke(mapping.Key, rowNumber, errorsMsg);
                            return null;
                        }
                    }

                    return obj;
                }
                catch (IndexOutOfRangeException)
                {
                    logAction?.Invoke("Ошибка", rowNumber, "Неверное количество столбцов");
                    return null;
                }
            }

            /// <summary>
            /// Вернуть список свойств
            /// </summary>
            public ColumnInfo[] GetProperties()
            {
                return this.propertyMapping
                    .OrderBy(x => x.Value.Item1)
                    .Select(x =>
                    {
                        var property = this.GetProperty(x.Key);
                        return new ColumnInfo(x, property.PropertyType, this.lenghtDict.Get(x.Key))
                        {
                            HasIndex = this.indexProperties.Contains(x.Key),
                            IsIgnore = property.HasAttribute<IgnoreAttribute>(true),
                            IsPrimary = property.HasAttribute<PrimaryKeyAttribute>(true)
                        };
                    })
                    .ToArray();
            }

            /// <summary>
            /// Приввести к строке для файла с ошибочными строками
            /// </summary>
            /// <param name="row"></param>
            public string ToRow(TRow row)
            {
                return string.Join(";",
                    this.propertyMapping.Select(
                            x => new
                            {
                                Order = x.Value.Item1,
                                Value = this.GeValue(row, x)
                            })
                        .OrderBy(x => x.Order)
                        .Select(x => x.Value));
            }

            private string GeValue(TRow row, KeyValuePair<string, Tuple<int, bool>> x)
            {
                var propertyInfo = this.GetProperty(x.Key);
                var value = propertyInfo.GetValue(row);

                if (propertyInfo.PropertyType.IsEnum)
                {
                    value = (int)value;
                }

                return value.ToStr().Trim();
            }

            private string TryExtractProperty(TRow obj, string propertyName, string value, bool required)
            {
                if (required && value.IsEmpty())
                {
                    return "Не заполнено обязательное поле";
                }

                var propertyInfo = this.GetProperty(propertyName);

                object convertedValue;
                if (propertyInfo.PropertyType == typeof(decimal))
                {
                    decimal val;
                    if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out val))
                    {
                        return "Входная строка имела неверный формат";
                    }

                    convertedValue = val;
                }
                else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    convertedValue = ConvertHelper.ConvertTo(value == "t" ? "1" : value, propertyInfo.PropertyType);
                }
                else
                {
                    convertedValue = ConvertHelper.ConvertTo(value, propertyInfo.PropertyType);

                    if (convertedValue.Is<string>())
                    {
                        convertedValue = string.Intern((string)convertedValue);
                    }
                }

                propertyInfo.SetValue(obj, convertedValue);
                return null;
            }

            private PropertyInfo GetProperty(string propertyName)
            {
                PropertyInfo propertyInfo;
                if (!this.propertyInfos.TryGetValue(typeof(TRow).Name + propertyName, out propertyInfo))
                {
                    propertyInfo = typeof(TRow).GetProperty(propertyName);
                    this.propertyInfos.Add(typeof(TRow).Name + propertyName, propertyInfo);
                }
                return propertyInfo;
            }
        }
    }

    public class ColumnInfo
    {
        public ColumnInfo(KeyValuePair<string, Tuple<int, bool>> kvp, Type propertyType, int length)
        {
            this.PropertyName = kvp.Key;
            this.PropertyType = propertyType;
            this.Required = kvp.Value.Item2;
            this.Length = length;
        }

        public int Length { get; set; }

        public bool Required { get; set; }

        public Type PropertyType { get; set; }

        public string PropertyName { get; set; }
        public bool HasIndex { get; set; }
        public bool IsIgnore { get; set; }
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// Строка из файла
    /// </summary>
    public interface IRow
    {
        /// <summary>
        /// Номер строки
        /// </summary>
        int RowNumber { get; set; }

        bool IsValid { get; set; }
    }
}