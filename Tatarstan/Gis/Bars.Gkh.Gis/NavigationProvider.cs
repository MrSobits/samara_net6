namespace Bars.Gkh.Gis
{
    using B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            // Администрирование
            var administration = root.Add("Администрирование");

            administration
                .Add("Справочник адресов")
                .Add("Сопоставление адресов", "billingaddressmatching")
                .AddRequiredPermission("Administration.AddressDirectory.AddressMatching.View");

            administration
                .Add("Импорт/экспорт данных системы")
                .Add("Импорт инкрементальных данных", "incrementalimport")
                .AddRequiredPermission("Administration.ImportExport.IncrementalImport");

            // ГИС ЖКХ
            var gis = root.Add("ГИС ЖКХ");

            var monitoring = gis.Add("Мониторинг индикаторов");
            monitoring.Add("Индикаторы", "indicator").AddRequiredPermission("Gis.Indicators.View");
            monitoring.Add("Типы домов", "gisrealestatetype").AddRequiredPermission("Gis.RealEstateType.View");
            monitoring.Add("Анализ индикаторов", "analysisofindicators").AddRequiredPermission("Gis.IndicatorsAnalysis.View");
            monitoring.Add("Регрессионный анализ", "regressionanalysis").AddRequiredPermission("Gis.RegressionAnalysis.View");
            monitoring.Add("Множественный анализ", "multipleanalysis").AddRequiredPermission("Gis.MultipleAnalysis.View");

            var analystIndicators = gis.Add("Аналитика по индикаторам");
            analystIndicators.Add("СКАП", "skap").AddRequiredPermission("Gis.Skap.View");

            var comparison = gis.Add("Сравнение данных");
            comparison.Add("Сравнение данных УО и РСО", "volumediscrepancy").AddRequiredPermission("Gis.VolumeDiscrepancy.View");

            var dicts = gis.Add("Справочники");
            dicts.Add("Услуги", "gisservicedict").AddRequiredPermission("Gis.Dict.Service.View");
            dicts.Add("Нормативные параметры", "gisnormativdict").AddRequiredPermission("Gis.Dict.Normativ.View");
            dicts.Add("Тарифы", "gistariffdict").AddRequiredPermission("Gis.Dict.Tariff.View");

            var importData = gis.Add("Импорт/экспорт данных");
            importData
                .Add("Импорт данных в систему \"Открытый Татарстан\"", "importdataot")
                .WithIcon("localGovernment")
                .AddRequiredPermission("Gis.ImportExportData.ImportDataOt");
            importData.Add("Выгрузка показаний ПУ", "unloadcountervalues").AddRequiredPermission("Gis.ImportExportData.UnloadCounterValues");

            var kpSettings = gis.Add("Настройка структуры данных");
            kpSettings.Add("Банки данных", "gisdatabank").AddRequiredPermission("Gis.KpSettings.GisDataBank.View");

            // Жилищный фонд
            root.Add("Жилищный фонд").Add("Сбор бытовых отходов").Add("Площадки сбора ТБО и ЖБО", "wastecollection").AddRequiredPermission("Gkh.WasteCollectionPlaces.View");
        }

        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }
        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
    }
}