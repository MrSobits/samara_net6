namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System.Data;
    using System.Globalization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Dapper;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;

    /// <summary>
    /// Типы данных 
    /// </summary>
    public enum Types
    {
        String = 1,
        DateTime = 2,
        Date = 3,
        Int = 4,
        NumericMoney = 5,
        Numeric = 6
    }

    public enum UploadStatuses
    {
        Upload = 1,
        Uploaded = 2,
        UploadedWithErrors = 3,
        NotUploaded = 3,
    }

    public class UploadErrors
    {
        public IDbConnection Connection { get; set; }
        public FormatTemplate FormatTemplate { get; set; }
        public void GetErrorMessage(string fileName, int? rowNumber, int error, string message)
        {
            fileName = fileName.ToUpper();
            fileName = !fileName.Contains(".CSV") ? fileName + ".CSV" : fileName;
            string sql = "";
            if (error == 998 || error == 999)
            {
                sql = string.Format("insert into upload.system_file_log(file_id,error_id,error_text) values({0},{1},'{2}');",
                    FormatTemplate.UploadId == 0 ? "null" : FormatTemplate.UploadId.ToString(), error, string.Format("{0};{1};{2};\"{5}.Ошибка,{3}.{4}\"",
                 fileName, error, rowNumber == null ? "" : rowNumber.ToString(), Types[error], message.Replace("\'", "#").Replace("\"", "\"\""), "Идентификатор файла:" + FormatTemplate.UploadId));
                if (FormatTemplate.UploadId != 0)
                {
                    sql += string.Format("insert into upload_part.file_log_{0}(file_id,error_id,str_number,error_text,file_name) values({0},{1},{2},'{3}','{4}')",
                 FormatTemplate.UploadId, error, rowNumber == null ? "null" : rowNumber.ToString(), string.Format("\"{2}.Ошибка,{0}.{1}\"",
                 Types[error], message.Replace("\'", "#").Replace("\'", "#").Replace("\"", "\"\""), "Идентификатор файла:" + FormatTemplate.UploadId), fileName);
                }
            }
            else if (error == 997)
            {
                sql = string.Format("insert into upload.system_file_log(file_id,error_id,error_text) values({0},{1},'{2}');",
                    FormatTemplate.UploadId == 0 ? "null" : FormatTemplate.UploadId.ToString(), error, string.Format("{0};{1};{2};\"{5}.Ошибка,{3}.{4}\"",
                 fileName, error, rowNumber == null ? "" : rowNumber.ToString(), Types[error], message.Replace("\'", "#").Replace("\"", "\"\""), "Идентификатор файла:" + FormatTemplate.UploadId));
                if (FormatTemplate.UploadId != 0)
                {
                    sql += string.Format("insert into upload_part.file_log_{0}(file_id,error_id,str_number,error_text,file_name) values({0},{1},{2},'{3}','{4}')",
                 FormatTemplate.UploadId, error, rowNumber == null ? "null" : rowNumber.ToString(), string.Format("\"{1}.Ошибка,{0}\"",
                 Types[error], "Идентификатор файла:" + FormatTemplate.UploadId), fileName);
                }
            }
            else
                sql =
                    string.Format(
                        "insert into upload_part.file_log_{0}(file_id,error_id,str_number,error_text,file_name) values({0},{1},{2},'{3}','{4}')",
                        FormatTemplate.UploadId, error, rowNumber == null ? "null" : rowNumber.ToString(),
                        string.Format("\"{2}.Ошибка,{0}.{1}\"",
                            Types[error], message.Replace("\'", "#").Replace("\'", "#").Replace("\"", "\"\""), "Идентификатор файла:" + FormatTemplate.UploadId), fileName);
            Connection.Execute(sql);
        }
        public void GetWarningMessage(string fileName, int? rowNumber, int error, string message)
        {
            fileName = fileName.ToUpper();
            fileName = !fileName.Contains(".CSV") ? fileName + ".CSV" : fileName;
            var sql = string.Format("insert into upload_part.file_log_{0}(file_id,error_id,str_number,error_text,file_name) values({0},{1},{2},'{3}','{4}')",
              FormatTemplate.UploadId, error, rowNumber == null ? "null" : rowNumber.ToString(), string.Format("\"Предупреждение,{0}.{1}\"",
               Types[error], message.Replace("\'", "#").Replace("\"", "\"\"")), fileName);
            Connection.Execute(sql);
        }

        public static Dictionary<int, string> Types = new Dictionary<int, string>
        {
            //Системные ошибки
            {997,"Ошибка при сохранении данных"},
            {998,"Данные архива не соответствуют формату"},
            {999,"Не удалось выполнить загрузку. Обратитесь к разработчику"},
            //Проверки перед загрузкой файла
            {1000,"ОГРН поставщика данных из файла не совпадает с ОГРН из МЖФ"},
            {1001,"ИНН и КПП поставщика данных из файла не совпдают с ИНН и КПП из МЖФ"},
            {1002,"Ключ банка данных из файла не совпадает с ключом банка из МЖФ"},
            //Проверки состава файла
            {1500,"Имя файла не соответствует формату"},
            {1501,"Не найден файл указанный в описи"},
            {1502,"Обнаружены файлы с одинаковыми именами"},
            //Проверки состава файла
            {2001,"Неверное количество полей"},
            {2002,"Не заполнено обязательное поле"},
            {2003,"Неверный тип данных"},
            {2004,"Значение поля меньше минимально допустимого значения"},
            {2005,"Значение поля больше максимально допустимого значения"},
            {2006,"Дробная часть превышает допустимое количество знаков"},
            {2007,"Длина строки превышает максимально допустимую длину"},
            {2008,"Переполнение дробного числа"},
            {2009,"Антипереполнение дробного числа"},
            //Проверки загруженных данных
            {2500,"Неуникальные значения в уникальном поле"},
            {2501,"Не обеспечена уникальность значений полей"},
            {2502,"Не обеспечена ссылочная целостность"},
            {2503,"Не обеспечена ссылочная целостность, отсутствует секция"},
            //Проверки загруженных данных
            {3001,"Неуникальные значения полей"},
            {3002,"Некорректный период"},
            {3003,"Некорретная сумма"},
            {3004,"Некорретное количество строк"},
            {3005,"Переход показаний ПУ через ноль"},
            {3006,"Не указаны ЛС для общеквартирного или группового ПУ"},
            {3007,"Некорректное значение поля"}
        };
    }

    public class Template
    {
        public Template()
        {
        }
        //Использовался ранее для статических словарей
        public Template(string fieldName, string columnName, string sectionName, Types type, int? minLength, int? maxLength, int place, bool necessarily = true, bool isUnique = false, int column = 0, string section = null, params string[] correctSymbols)
        {
            FieldName = fieldName;
            MinLength = minLength;
            MaxLength = maxLength;
            Type = type;
            SectionName = sectionName;
            ColumnName = columnName;
            Place = place;
            IsUnique = isUnique;
            //this.mantissa = mantissa;
            Necessarily = necessarily;
            CorrectSymbols = correctSymbols;
            Section = section;
            Column = column;
        }

        /// <summary>
        /// Наименование поля
        /// </summary>
        public string FieldName { get; set; }

        public string ColumnName { get; set; }

        public string SectionName { get; set; }
        public int Place { get; set; }
        public bool IsUnique { get; set; }

        /// <summary>
        /// Тип данных, которым должно являться передаваемое поле
        /// </summary>
        public Types Type { get; set; }
        /// <summary>
        /// Символы которые, могут встречаться во входящем объекте
        /// </summary>
        public string[] CorrectSymbols { get; set; }
        /// <summary>
        /// Минимальная длина(для чисел минимальное значение)
        /// </summary>
        public int? MinLength { get; set; }
        /// <summary>
        /// Максимальная длина(для чисел максимальное значение)
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// Обязательность поля
        /// </summary>
        public bool Necessarily { get; set; }
        /// <summary>
        /// Мантисса(в данный момент не используется)
        /// </summary>
        public int Mantissa { get; set; }
        /// <summary>
        /// Порядковый номер зависимой колонки
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// Номер зависимой секции
        /// </summary>
        public string Section { get; set; }

        public bool IsRef { get; set; }
        public int NumericPrecission { get; set; }
        public int NumericScale { get; set; }
        public string RefSchema { get; set; }
        public string RefTable { get; set; }
        public string RefColumn { get; set; }

        /// <summary>
        /// Проверка объекта
        /// </summary>
        /// <param name="uploadErrors"></param>
        /// <param name="value">Значение объекта</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <param name="section">Номер секции</param>
        /// <returns>Возвращает текст ошибки или null</returns>
        public void CheckValues(UploadErrors uploadErrors, string value, int rowNumber, string section, string fileName)
        {
            if (value.Trim().Length == 0)
            {
                if (Necessarily)
                    uploadErrors.GetErrorMessage(fileName, rowNumber, 2002,
                        string.Format("Секция {1},Не заполнено обязательное поле:{0}", FieldName, section));
                return;
            }
            var newValue = value.Trim();
            //CorrectSymbols.ToList().ForEach(s => newValue = newValue.Replace(s, ""));
            newValue = newValue.Replace("\"", "").Replace("\'", "");
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            switch (Type)
            {
                case Types.Int:
                    {
                        Int32 i_int;
                        if (!Int32.TryParse(newValue, out i_int))
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2003, string.Format("Секция {2}, Поле имеет неверный формат. Значение = {0}, имя поля: {1}", value, FieldName, section));
                            return;
                        }
                        if (MinLength.HasValue)
                            if (i_int < MinLength.Value)
                            {
                                uploadErrors.GetErrorMessage(fileName, rowNumber, 2004, string.Format("Секция {3}, Поле имеет неверное значение (меньше {2}). Значение = {0}, имя поля: {1}", value, FieldName, MinLength, section));
                                return;
                            }
                        if (MaxLength.HasValue)
                            if (i_int > MaxLength.Value)
                            {
                                uploadErrors.GetErrorMessage(fileName, rowNumber, 2005, string.Format("Секция {3}, Поле имеет неверное значение (превышает {2}). Значение = {0}, имя поля: {1}", value, FieldName, MaxLength, section));
                                return;
                            }
                    } break;
                case Types.Numeric:
                    {
                        Decimal d_decimal;
                        newValue = newValue.Replace(",", ".");
                        if (!Decimal.TryParse(newValue, out d_decimal))
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2003, string.Format("Секция {2}, Поле имеет неверный формат. Значение = {0}, имя поля: {1}", value, FieldName, section));
                            return;
                        }
                        if (Math.Abs(d_decimal - decimal.Truncate(d_decimal)).ToString(CultureInfo.InvariantCulture).Length > 20)
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2008, string.Format("Секция {2}, Поле имеет неверный формат(дробная часть превышает 20 знаков). Значение = {0}, имя поля: {1}", value, FieldName, section));
                            return;
                        }
                        if (NumericPrecission != 0)
                            if (d_decimal.ToString().Replace(".", "").Count() > NumericPrecission)
                            {
                                uploadErrors.GetErrorMessage(fileName, rowNumber, 2005, string.Format("Секция {3}, Поле имеет неверное значение (превышает {2}). Значение = {0}, имя поля: {1}", value, FieldName, NumericPrecission, section));
                                return;
                            }
                    } break;
                case Types.NumericMoney:
                    {
                        float d_decimal;
                        newValue = newValue.Replace(",", ".");
                        if (!float.TryParse(newValue, out d_decimal))
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2003, string.Format("Секция {2}, Поле имеет неверный формат. Значение = {0}, имя поля: {1}", value, FieldName, section));
                            return;
                        }
                    } break;
                case Types.String:
                    {
                        newValue = newValue.Replace("«", "\"").Replace("»", "\"").Replace("'", "\"");
                        if (newValue.Length > MaxLength)
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2007, string.Format("Секция {3},cтрока {1}: Длина текста превышает заданный формат ({2}). Значение = {0}, имя поля: {1}", value, FieldName, MaxLength, section));
                            return;
                        }
                    } break;
                case Types.Date:
                case Types.DateTime:
                    {
                        DateTime date;
                        if (!DateTime.TryParse(newValue, out date))
                        {
                            uploadErrors.GetErrorMessage(fileName, rowNumber, 2003, string.Format("Секция {2}, Поле имеет неверный формат. Значение = {0}, имя поля: {1}", value, FieldName, section));
                            return;
                        }
                    } break;
            }
        }
    }
}
