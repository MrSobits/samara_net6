namespace Bars.Gkh.Gis.DomainService.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using B4;
    using Entities.Register.HouseServiceRegister;
    using Entities.Register.MultipleAnalysis;
    using Enum;

    /// <summary>
    /// Интерфейс для сервисов множественного анализа
    /// </summary>
    public interface IMultipleAnalysisService
    {
        /// <summary>
        /// Анализ на диапазон значений
        /// </summary>
        /// <param name="houseService">услуги дома</param>
        /// <param name="indicator">индикатор</param>
        /// <param name="min">минимальное значение</param>
        /// <param name="max">максимальное значение</param>
        /// <returns></returns>
        IList<MultipleAnalysisIndicatorProxy> AnalyzeMinMaxValue(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal? min, decimal? max);

        /// <summary>
        /// Анализ на процент отклонения
        /// </summary>
        /// <param name="houseService">услуги дома</param>
        /// <param name="indicator">индикатора</param>
        /// <param name="percent">процент отклонения</param>
        /// <returns></returns>
        IList<MultipleAnalysisIndicatorProxy> AnalyzeDeviationPercent(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal percent);

        /// <summary>
        /// Анализ на соответсвие значению
        /// </summary>
        /// <param name="houseService">услуги дома</param>
        /// <param name="indicator">индикатор</param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        IList<MultipleAnalysisIndicatorProxy> AnalyzeExactValue(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal value);

        /// <summary>
        /// Возвращает дерево индикатор с заполненными значениями для шаблона
        /// </summary>
        /// <returns></returns>
        IList<MultipleAnalysisGroupProxy> GetTemplateIndicatorTree(BaseParams baseParams);

        /// <summary>
        /// Возвращает названия колонок для индикаторов
        /// </summary>
        /// <param name="indicators">список индикаторов</param>
        /// <param name="separator">разделитель в названиях клонок</param>
        /// <returns></returns>
        Dictionary<string, string> GetReportColumns(List<MultipleAnalysisProxy> indicators, string separator);

        /// <summary>
        /// Возвращает список идентификаторов домов
        /// </summary>
        /// <param name="typeHouse">тип дома</param>
        /// <param name="municipalArea">муниципальный район</param>
        /// <param name="settlement">город/поселок</param>
        /// <param name="street">улица</param>
        /// <returns></returns>
        IList<long> GetHouseIdByType(long typeHouse, string municipalArea, string settlement, string street);

        /// <summary>
        /// Возвращает отчет в виде таблицы
        /// </summary>
        /// <param name="indicators">список индикаторов</param>
        /// <param name="housesIdByType">список идентификаторов домов</param>
        /// <param name="date">дата</param>
        /// <param name="typeCondition">условие</param>
        /// <returns></returns>
        DataTable GetReportData(List<MultipleAnalysisProxy> indicators, IList<long> housesIdByType, DateTime date, GisTypeCondition typeCondition);

        /// <summary>
        /// Возвращает отчет в виде excel файла
        /// </summary>
        /// <param name="template">шаблон</param>
        /// <param name="date">дата</param>
        /// <param name="municipalArea">гуид муниципального района</param>
        /// <param name="settlement">гуид нас пункта</param>
        /// <param name="street">гуид улицы</param>
        /// <returns></returns>
        Stream GetExcelReport(MultipleAnalysisTemplate template, DateTime date);

        /// <summary>
        /// Метод отправки сообщения
        /// </summary>
        /// <param name="template">шаблон</param>
        /// <param name="report">отчет</param>
        /// <param name="date">дата</param>
        /// <returns></returns>
        bool SendEmail(MultipleAnalysisTemplate template, Stream report, DateTime date);

        /// <summary>
        /// Запускает фоновый процесс отправки писем с отчетами
        /// </summary>
        /// <returns></returns>
        bool StartBackgroundProcess();

        /// <summary>
        /// Возвращает список фиас гуидов муниципальных районов
        /// </summary>
        /// <returns></returns>
        IDataResult ListFiasArea(BaseParams baseParams);
    }
}