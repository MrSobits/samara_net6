namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Интерфейс приведения данных к формату экспорта
    /// </summary>
    public interface IExportFormatConverter
    {
        /// <summary>
        /// Да (1)
        /// </summary>
        string Yes { get; }

        /// <summary>
        /// Нет (2)
        /// </summary>
        string No { get; }

        /// <summary>
        /// Не задано (0)
        /// </summary>
        string NotSet { get; }

        /// <summary>
        /// Получить строку даты согласно формату
        /// </summary>
        string GetDate(DateTime? date);

        /// <summary>
        /// Получить строку даты согласно формату
        /// </summary>
        string GetDate(string date);

        /// <summary>
        /// Найти первое вхождение даты в тексте согласно формату
        /// </summary>
        string FindDate(string stringWithDate);

        /// <summary>
        /// Получить строку даты и времени согласно формату
        /// </summary>
        string GetDateTime(DateTime? date);

        /// <summary>
        /// Получить строку времени согласно формату
        /// </summary>
        string GetTime(DateTime? date);

        /// <summary>
        /// Получить первое число в году
        /// </summary>
        /// <param name="year">Числовое значение года</param>
        string GetFirstDateYear(int? year);

        /// <summary>
        /// Получить первое число в году
        /// </summary>
        /// <param name="year">Строковое значение года</param>
        string GetFirstDateYear(string year);

        /// <summary>
        /// Получить дробное значение
        /// </summary>
        /// <param name="decimalValue">Строковое значение числа</param>
        string GetDecimal(string decimalValue);

        /// <summary>
        /// Получить дробное значение
        /// </summary>
        /// <param name="decimalValue">Строковое значение числа</param>
        /// <param name="decimalPlaces">Число знаков после запятой</param>
        string GetDecimal(string decimalValue, int decimalPlaces);

        /// <summary>
        /// Получить дробное значение
        /// </summary>
        /// <param name="decimalValue">Nullable значение числа</param>
        string GetDecimal(decimal? decimalValue);

        /// <summary>
        /// Получить дробное значение
        /// </summary>
        /// <param name="decimalValue">Nullable значение числа</param>
        /// <param name="decimalPlaces">Число знаков после запятой</param>
        string GetDecimal(decimal? decimalValue, int decimalPlaces);

        /// <summary>
        /// Получить номер документа
        /// </summary>
        /// <param name="number">Строка с номером</param>
        string GetDocumentNumber(string number);

        /// <summary>
        /// Получить номер документа
        /// </summary>
        /// <param name="number">Строка с номером</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        string GetDocumentNumber(string number, string defaultValue);

        /// <summary>
        /// Получить номер документа
        /// </summary>
        /// <param name="number">Номер</param>
        string GetDocumentNumber(int? number);

        /// <summary>
        /// Получить номер документа
        /// </summary>
        /// <param name="number">Номер</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        string GetDocumentNumber(int? number, string defaultValue);

        /// <summary>
        /// Получить тип размера платы
        /// </summary>
        /// <param name="paymentsOwnersFoundation">Основание договора ук с собственниками</param>
        string GetPaymentFoundation(ManOrgSetPaymentsOwnersFoundation paymentsOwnersFoundation);

        /// <summary>
        /// Получить строковое представление идентификатора сущности
        /// </summary>
        string GetStrId(IHaveId entity);

        /// <summary>
        /// Получить строковое представление сквозного идентификатора сущности
        /// </summary>
        string GetStrId(IHaveExportId entity);

        /// <summary>
        /// Заменить пустую строку значением по умолчанию
        /// </summary>
        string GetValueOrDefault(string value, string defaultValue = null);

        /// <summary>
        /// Получить строковое значение больше нуля или пустую строку
        /// <param name="value">Nullable значение числа</param>
        /// </summary>
        string GetNotZeroValue(int? value);

        /// <summary>
        /// Получить строковое значение больше нуля или пустую строку
        /// </summary>
        /// <param name="decimalValue">Nullable значение числа</param>
        string GetNotZeroValue(decimal? decimalValue);

        /// <summary>
        /// Получить строковое значение больше нуля или пустую строку
        /// </summary>
        /// <param name="decimalValue">Nullable значение числа</param>
        /// <param name="decimalPlaces">Число знаков после запятой</param>
        string GetNotZeroValue(decimal? decimalValue, int decimalPlaces);

        /// <summary>
        /// Разбить Имя на ФИО
        /// </summary>
        /// <returns>(Фамилия, Имя, Отчество)</returns>
        Tuple<string, string, string> ParseFullName(string name);
    }
}