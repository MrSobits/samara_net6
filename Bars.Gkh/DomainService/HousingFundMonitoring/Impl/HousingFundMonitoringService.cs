﻿namespace Bars.Gkh.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с Мониторинг жилищного фонда
    /// </summary>
    public class HousingFundMonitoringService : IHousingFundMonitoringService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для Период Мониторинга жилищного фонда
        /// </summary>
        public IDomainService<HousingFundMonitoringPeriod> HousingFundMonitoringPeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Запись Мониторинга жилищного фонда
        /// </summary>
        public IDomainService<HousingFundMonitoringInfo> HousingFundMonitoringInfoDomain { get; set; }

        /// <summary>
        /// Сервис "Муниципальный район и Муниципальный образование"
        /// </summary>
        public IMunicipalityService MunicipalityService { get; set; }

        ///<inheritdoc/>
        public IDataResult MassCreate(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<int>("year");
            var municipalityIds = baseParams.Params.GetAs<List<long>>("municipality");

            var municipalities = this.MunicipalityService.ListByParamAndOperatorQueryable();

            var periods = municipalities
                .WhereIf(municipalityIds != null && municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .Select(
                    x => new HousingFundMonitoringPeriod
                    {
                        Municipality = new Municipality {Id = x.Id},
                        Year = year
                    })
                .ToList();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                foreach (var period in periods)
                {
                    this.HousingFundMonitoringPeriodDomain.Save(period);
                    this.CreatePeriodInfo(period);
                }

                transaction.Commit();

                return new BaseDataResult();
            }
        }

        private void CreatePeriodInfo(HousingFundMonitoringPeriod period)
        {
            foreach (var record in HousingFundMonitoringService.Records)
            {
                var monitoringInfo = new HousingFundMonitoringInfo
                {
                    Period = period,
                    RowNumber = record.RowNumber,
                    Mark = record.Mark,
                    UnitMeasure = record.UnitMeasure,
                    DataProvider = record.DataProvider
                };

                this.HousingFundMonitoringInfoDomain.Save(monitoringInfo);
            }
        }

        private static readonly List<Record> Records = new List<Record>
        {
                new Record("1", "Основные характеристики жилищного фонда в субъекте Российской Федерации"),
                new Record("1.1", "Характеристики жилищного фонда по площади жилых помещений"),
                new Record("1.1.1", "Общая площадь жилых помещений на отчетную дату, всего (1.1.2 + 1.1.3 + 1.1.4)", "тыс. кв. м", "Орган местного самоуправления"),
                new Record("1.1.2", "Площадь жилых помещений в многоквартирных домах в зависимости от формы собственности, в том числе:", "тыс. кв. м"),
                new Record("1.1.2.1", "- частного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.2.2", "- государственного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.2.3", "- муниципального жилищного фонда", "тыс. кв. м"),
                new Record("1.1.3", "Площадь комнат в жилых домах блокированной застройки в зависимости от формы собственности, в том числе:", "тыс. кв. м"),
                new Record("1.1.3.1", "- частного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.3.2", "- государственного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.3.3", "- муниципального жилищного фонда", "тыс. кв. м"),
                new Record("1.1.4", "Площадь комнат в жилых домах в зависимости от формы собственности, в том числе:", "тыс. кв. м"),
                new Record("1.1.4.1", "- частного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.4.2", "- государственного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.4.3", "- муниципального жилищного фонда", "тыс. кв. м"),
                new Record("1.1.5", "Площадь жилых помещений в зависимости от целей использования, в том числе:", "тыс. кв. м"),
                new Record("1.1.5.1", "- жилищного фонда социального использования", "тыс. кв. м"),
                new Record("1.1.5.2", "- специализированного жилищного фонда", "тыс. кв. м"),
                new Record("1.1.5.3", "- индивидуального жилищного фонда", "тыс. кв. м"),
                new Record("1.1.5.4", "- жилищного фонда коммерческого использования", "тыс. кв. м"),
                new Record("1.1.6", "Общая площадь жилых помещений в многоквартирных домах, включенных в региональную программу капитального ремонта общего имущества в многоквартирном доме на отчетную дату", "тыс. кв. м"),
                new Record("1.1.7", "Общая площадь жилых помещений в многоквартирных домах, признанных аварийными на отчетную дату", "тыс. кв. м"),
                new Record("1.1.7.1", "- в том числе подлежащих сносу", "тыс. кв. м"),
                new Record("1.1.8", "Распределение общей площади жилых помещений многоквартирных домов по проценту износа:"),
                new Record("1.1.8.1", "- от 0% до 30%", "тыс. кв. м"),
                new Record("1.1.8.2", "- от 31% до 65%", "тыс. кв. м"),
                new Record("1.1.8.3", "- от 66% до 70%", "тыс. кв. м"),
                new Record("1.1.8.4", "- Свыше 70%", "тыс. кв. м"),
                new Record("1.2", "Характеристики жилищного фонда по количеству домов и помещений"),
                new Record("1.2.1", "Количество многоквартирных домов", "единиц", "Орган местного самоуправления"),
                new Record("1.2.2", "Количество жилых домов блокированной застройки", "единиц"),
                new Record("1.2.3", "Количество жилых домов", "единиц"),
                new Record("1.2.4", "Количество жилых помещений в многоквартирных домах", "единиц"),
                new Record("1.2.5", "Количество многоквартирных домов, включенных в региональную программу капитального ремонта общего имущества в многоквартирном доме на отчетную дату", "единиц"),
                new Record("1.2.6", "Количество многоквартирных домов, признанных аварийными на отчетную дату", "единиц"),
                new Record("1.2.6.1", "- в том числе подлежащих сносу", "единиц"),
                new Record("1.2.7", "Распределение многоквартирных домов по проценту износа:"),
                new Record("1.2.7.1", "- от 0% до 30%", "единиц"),
                new Record("1.2.7.2", "- от 31% до 65%", "единиц"),
                new Record("1.2.7.3", "- от 66% до 70%", "единиц"),
                new Record("1.2.7.4", "- свыше 70%", "единиц"),
                new Record("1.2.8", "Распределение многоквартирных домов в соответствии с выбранным способом управления многоквартирным домом:"),
                new Record("1.2.8.1", "- непосредственное управление собственниками помещений в многоквартирном доме", "единиц", "Орган государственного жилищного надзора"),
                new Record("1.2.8.2", "- управление товариществом собственников жилья", "единиц"),
                new Record("1.2.8.3", "- управление жилищным кооперативом или иным специализированным потребительским кооперативом", "единиц"),
                new Record("1.2.8.4", "- управление управляющей организацией", "единиц"),
                new Record("1.2.9", "Количество многоквартирных домов, в которых не выбран способ управления или принятое решение о выборе способа управления не реализовано", "единиц"),
                new Record("1.3", "Характеристики жилищного фонда по количеству проживающих в жилых помещениях"),
                new Record("1.3.1", "Общее количество проживающих в жилых помещениях, в том числе:", "человек", "Орган местного самоуправления"),
                new Record("1.3.1.1", "- в многоквартирных домах", "человек"),
                new Record("1.3.1.2", "- в домах блокированной застройки", "человек"),
                new Record("1.3.1.3", "- в жилых домах", "человек"),
                new Record("1.3.2", "Общее количество проживающих в жилых помещениях специализированного жилищного фонда:", "человек"),
                new Record("1.3.3", "Общее количество проживающих в жилых помещениях жилищного фонда социального использования", "человек"),
                new Record("1.3.4", "Общее количество проживающих в жилых помещениях в многоквартирных домах, признанных непригодными для проживания и подлежащих переселению на отчетную дату", "человек"),
                new Record("1.3.4.1", "- в том числе в многоквартирных домах, признанных аварийными и подлежащими сносу", "человек"),
                new Record("1.3.5", "Общее количество проживающих в жилых помещениях в многоквартирных домах, включенных в региональную программу капитального ремонта общего имущества в многоквартирном доме на отчетную дату", "человек"),
                new Record("1.3.6", "Сведения о количестве проживающих в многоквартирных домах по проценту износа этих домов:"),
                new Record("1.3.6.1", "- от 0% до 30%", "человек"),
                new Record("1.3.6.2", "- от 31% до 65%", "человек"),
                new Record("1.3.6.3", "- от 66% до 70%", "человек"),
                new Record("1.3.6.4", "- свыше 70%", "человек"),
                new Record("2", "Оборудование жилищного фонда и оснащение приборами учета коммунальных ресурсов"),
                new Record("2.1", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой холодного водоснабжения", "тыс. кв. м", "Орган местного самоуправления"),
                new Record("2.1.1", "- в том числе присоединенных к централизованной системе холодного водоснабжения", "тыс. кв. м"),
                new Record("2.2", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой горячего водоснабжения", "тыс. кв. м"),
                new Record("2.2.1", "- в том числе присоединенных к централизованной системе горячего водоснабжения", "тыс. кв. м"),
                new Record("2.3", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой водоотведения", "тыс. кв. м"),
                new Record("2.3.1", "- в том числе присоединенных к централизованной системе водоотведения", "тыс. кв. м"),
                new Record("2.4", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой теплоснабжения", "тыс. кв. м"),
                new Record("2.4.1", "- в том числе присоединенных к централизованной системе теплоснабжения", "тыс. кв. м"),
                new Record("2.5", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой газоснабжения", "тыс. кв. м"),
                new Record("2.5.1", "- в том числе присоединенных к централизованной системе газоснабжения", "тыс. кв. м"),
                new Record("2.6", "Площадь жилых помещений в многоквартирных домах, оборудованных инженерной системой электроснабжения", "тыс. кв. м"),
                new Record("2.6.1", "- в том числе присоединенных к централизованной системе электроснабжения", "тыс. кв. м"),
                new Record("2.7", "Количество многоквартирных домов, оснащенных коллективными (общедомовыми) приборами учета:"),
                new Record("2.7.1", "- холодной воды", "единиц"),
                new Record("2.7.2", "- горячей воды", "единиц"),
                new Record("2.7.3", "- тепловой энергии", "единиц"),
                new Record("2.7.4", "- газа", "единиц"),
                new Record("2.7.5", "- электрической энергии", "единиц"),
                new Record("2.8", "Количество жилых помещений в многоквартирных домах, оснащенных индивидуальными, общими (квартирными) приборами учета:"),
                new Record("2.8.1", "- холодной воды", "единиц"),
                new Record("2.8.2", "- горячей воды", "единиц"),
                new Record("2.8.3", "- тепловой энергии", "единиц"),
                new Record("2.8.4", "- газа", "единиц"),
                new Record("2.8.5", "- электрической энергии", "единиц"),
                new Record("3", "Движение жилищного фонда"),
                new Record("3.1", "Движение жилищного фонда по количеству домов"),
                new Record("3.1.1", "Количество многоквартирных домов, исключенных из жилищного фонда за отчетный период, в том числе:", "единиц"),
                new Record("3.1.1.1", "- снесены в связи с признанием аварийными и подлежащими сносу", "единиц"),
                new Record("3.1.1.2", "- все жилые помещения переведены в нежилые", "единиц"),
                new Record("3.1.1.3", "- прочие причины", "единиц"),
                new Record("3.1.2", "Количество жилых домов блокированной застройки, исключенных из жилищного фонда за отчетный период", "единиц"),
                new Record("3.1.3", "Количество жилых домов, исключенных из жилищного фонда за отчетный период", "единиц"),
                new Record("3.1.4", "Количество многоквартирных домов, включенных в жилищный фонд за отчетный период, в том числе:", "единиц"),
                new Record("3.1.4.1", "- новое строительство", "единиц"),
                new Record("3.1.4.2", "- все нежилые помещения переведены в жилые", "единиц"),
                new Record("3.1.4.3", "- прочие причины", "единиц"),
                new Record("3.1.5", "Количество жилых домов блокированной застройки, включенных в жилищный фонд за отчетный период", "единиц"),
                new Record("3.1.6", "Количество жилых домов, включенных в жилищный фонд за отчетный период", "единиц"),
                new Record("3.2", "Движение жилищного фонда по площади жилых помещений"),
                new Record("3.2.1", "Площадь жилых помещений в многоквартирных домах, исключенных из жилищного фонда за отчетный период, в том числе:", "тыс. кв. м"),
                new Record("3.2.1.1", "- в связи с признанием жилого помещения непригодным для проживания", "тыс. кв. м"),
                new Record("3.2.1.2", "- в связи со сносом аварийного многоквартирного дома", "тыс. кв. м"),
                new Record("3.2.1.3", "- жилые помещения переведены в нежилые", "тыс. кв. м"),
                new Record("3.2.1.4", "- прочие причины", "тыс. кв. м"),
                new Record("3.2.2", "Площадь комнат в жилых домах блокированной застройки, исключенных из жилищного фонда за отчетный период", "тыс. кв. м"),
                new Record("3.2.3", "Площадь комнат в жилых домах, исключенных из жилищного фонда за отчетный период", "тыс. кв. м"),
                new Record("3.2.4", "Площадь жилых помещений в многоквартирных домах, включенных в жилищный фонд за отчетный период, в том числе:", "тыс. кв. м"),
                new Record("3.2.4.1", "- новое строительство", "тыс. кв. м"),
                new Record("3.2.4.2", "- нежилые помещения переведены в жилые", "тыс. кв. м"),
                new Record("3.2.4.3", "- прочие причины", "тыс. кв. м"),
                new Record("3.2.5", "Площадь комнат в жилых домах блокированной застройки, включенных в жилищный фонд за отчетный период", "тыс. кв. м"),
                new Record("3.2.6", "Площадь комнат в жилых домах, включенных в жилищный фонд за отчетный период", "тыс. кв. м")
            };

        private class Record
        {
            public Record(string rowNumber, string mark, string unitMeasure = null, string dataProvider = null)
            {
                this.RowNumber = rowNumber;
                this.Mark = mark;
                this.UnitMeasure = unitMeasure;
                this.DataProvider = dataProvider;
            }

            public string RowNumber { get; }
            public string Mark { get; }
            public string UnitMeasure { get; }
            public string DataProvider { get; }
        }
    }
}