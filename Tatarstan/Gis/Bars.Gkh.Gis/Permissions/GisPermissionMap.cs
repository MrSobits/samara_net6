namespace Bars.Gkh.Gis.Permissions
{
    using B4;

    public class GisPermissionMap : PermissionMap
    {
        public GisPermissionMap()
        {
            this.Namespace("Gis", "ГИС ЖКХ");

            this.Namespace("Gis.Indicators", "Индикаторы");
            this.Permission("Gis.Indicators.View", "Просмотр");

            this.Namespace("Gis.RealEstateType", "Типы домов");
            this.Permission("Gis.RealEstateType.View", "Просмотр");

            this.Namespace("Gis.IndicatorsAnalysis", "Анализ индикаторов");
            this.Permission("Gis.IndicatorsAnalysis.View", "Просмотр");

            this.Namespace("Gis.RegressionAnalysis", "Регрессионный анализ");
            this.Permission("Gis.RegressionAnalysis.View", "Просмотр");

            this.Namespace("Gis.MultipleAnalysis", "Множественный анализ");
            this.Permission("Gis.MultipleAnalysis.View", "Просмотр");

            this.Namespace("Gis.Skap", "СКАП");
            this.Permission("Gis.Skap.View", "Просмотр");

            this.Namespace("Gis.VolumeDiscrepancy", "Сравнение данных УО и РСО");
            this.Permission("Gis.VolumeDiscrepancy.View", "Просмотр");

            this.Namespace("Gis.Dict", "Справочники");
            this.Namespace("Gis.Dict.Service", "Услуги");
            this.Permission("Gis.Dict.Service.View", "Просмотр");
            this.Namespace("Gis.Dict.Normativ", "Нормативные параметры");
            this.Permission("Gis.Dict.Normativ.View", "Просмотр");

            this.Namespace("Gis.Dict.Tariff", "Тарифы");
            this.CRUDandViewPermissions("Gis.Dict.Tariff");
            this.Namespace("Gis.Dict.Tariff.Field", "Поля");
            this.Permission("Gis.Dict.Tariff.Field.Contragent_Edit", " Поставщик - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.ActivityKind_Edit", "Вид деятельности - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.ContragentName_Edit", "Вид деятельности - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.EaisUploadDate_Edit", "Дата загрузки в ЕАИС - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.EaisUploadDate_Edit", "Дата последнего изменения в ЕАИ - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.Municipality_Edit", "Муниципальный район - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.Service_Edit", "Услуга - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.UnitMeasure_Edit", "Единица измерения - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.StartDate_Edit", "Дата начала периода - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.EndDate_Edit", "Дата окончания периода - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.ZoneCount_Edit", "Количество зон - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.TariffKind_Edit", "Вид тарифа - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.TariffValue_Edit", "Значение тарифа - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.TariffValue1_Edit", "Значение тарифа 1 - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.TariffValue2_Edit", "Значение тарифа 2 - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.TariffValue3_Edit", "Значение тарифа 3 - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.IsNdsInclude_Edit", "Включая НДС - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.IsSocialNorm_Edit", "В пределах социальной нормы - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.IsMeterExists_Edit", "Наличие прибора учета - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.IsElectricStoveExists_Edit", "Наличие электрической плиты - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.Floor_Edit", "Этаж - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.ConsumerType_Edit", "Тип потребителя - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.SettelmentType_Edit", "Вид населенного пункта - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.ConsumerByElectricEnergyType_Edit", "Тип потребителя по электроэнергии - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.RegulatedPeriodAttribute_Edit", "Дополнительный признак организации в регулируемом периоде - Редактирование");
            this.Permission("Gis.Dict.Tariff.Field.BasePeriodAttribute_Edit", "Дополнительный признак организации в базовом периоде - Редактирование");

            this.Namespace("Gis.ImportExportData", "Импорт/экспорт данных");
            this.Permission("Gis.ImportExportData.ImportDataOt", "Импорт данных в систему \"Открытый Татарстан\"");
            this.Permission("Gis.ImportExportData.UnloadCounterValues", "Выгрузка показаний ПУ");

            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Params", "Параметры дома");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Params");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Service", "Услуги по дому");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Service");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Accruals", "Начисления по домам");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Accruals");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Counters", "Показания ОДПУ");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Counters");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Claims", "Претензии граждан");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Claims");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Account.PublicControl", "Претензии граждан (НК)");
            this.Permission("Gkh.RealityObject.Register.HousingComminalService.Account.PublicControl.View", "Просмотр");

            this.Namespace("Gis.KpSettings", "Настройка структуры данных");
            this.Namespace("Gis.KpSettings.GisDataBank", "Банки данных");
            this.CRUDandViewPermissions("Gis.KpSettings.GisDataBank");
            this.Permission("Administration.ImportExport.Rso", "Импорт от РСО");
            this.Permission("Administration.ImportExport.IncrementalDataLoading", "Загрузка инкрементальных данных");
            this.Permission("Administration.ImportExport.IncrementalImport", "Импорт инкрементальных данных");

           // this.Namespace("Administration.OutsideSystemIntegrations", "Интеграция с внешними системами");
          //  this.Permission("Administration.OutsideSystemIntegrations.Gis", "Интеграция с ГИС ЖКХ");

            this.Namespace("Gkh.WasteCollectionPlaces", "Площадки сбора ТБО и ЖБО");
            this.CRUDandViewPermissions("Gkh.WasteCollectionPlaces");

            this.Namespace("Gkh.Orgs.Managing.Register.Contract.Services", "Услуги");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Services.Addition_Edit", "Дополнительные - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Services.Addition_View", "Дополнительные - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Services.Communal_Edit", "Коммунальные - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Services.Communal_View", "Коммунальные - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_Edit", "Работы/услуги - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_View", "Работы/услуги - Просмотр");
        }
    }
}