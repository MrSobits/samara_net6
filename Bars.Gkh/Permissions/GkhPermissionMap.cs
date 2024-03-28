namespace Bars.Gkh.Permissions
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Entities.Suggestion;

    /// <inheritdoc />
    public class GkhPermissionMap : PermissionMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GkhPermissionMap()
        {
            #region Администрирование
            this.Permission("B4.Security.LocalAdminRoleSettings", "Настройка локальных администраторов");
            this.Permission("B4.Security.FieldRequirement", "Обязательность полей");

            this.Namespace("Administration", "Администрирование");

            this.Namespace("Administration.Operator", "Операторы");
            this.CRUDandViewPermissions("Administration.Operator");

            this.Namespace("Administration.PrintCertHistory", "Контроль выдачи справок по ЛС");
            this.CRUDandViewPermissions("Administration.PrintCertHistory");

            this.Namespace("Administration.Operator.Fields", "Поля");
            this.Permission("Administration.Operator.Fields.UserPhoto", "Фото пользователя");
            this.Permission("Administration.Operator.Fields.MobileApplicationAccessEnabled", "Доступ к мобильному приложению");
            
            this.Namespace("Administration.Operator.Fields.RisToken", "Токен");
            this.Permission("Administration.Operator.Fields.RisToken.Edit", "Токен - Просмотр");
            this.Permission("Administration.Operator.Fields.RisToken.View", "Токен - Редактирование");

            this.Namespace("Administration.Profile", "Профиль");
            this.Permission("Administration.Profile.View", "Просмотр");
            this.Permission("Administration.Profile.Edit", "Изменение");

            this.Namespace("Administration.AddressDirectory.AddressMatching", "Сопоставление адресов");
            this.Permission("Administration.AddressDirectory.AddressMatching.View", "Сопоставление адресов");
            this.Permission("Administration.AddressDirectory.AddressMatching.Mismatch", "Удаление связей");

            this.Namespace("Administration.TemplateReplacement", "Шаблоны");
            this.Permission("Administration.TemplateReplacement.View", "Просмотр");
            this.Permission("Administration.TemplateReplacement.Edit", "Изменение");

            this.Namespace("Administration.ImportLog", "Логи загрузок");
            this.Permission("Administration.ImportLog.View", "Просмотр");
            this.Permission("Administration.ImportLog.ShowAll", "Показать все записи");

            this.Namespace("Administration.OperationLog", "Логи операций");
            this.Permission("Administration.OperationLog.View", "Просмотр");

            this.Namespace("Administration.InstructionGroups", "Категория документации");
            this.CRUDandViewPermissions("Administration.InstructionGroups");
            this.Namespace("Administration.InstructionGroups.Register", "Реестры");
            this.Namespace("Administration.InstructionGroups.Register.Instructions", "Документация");
            this.CRUDandViewPermissions("Administration.InstructionGroups.Register.Instructions");

            this.Namespace("Administration.LoadIdIs", "Загрузка идентификатора ИС");
            this.CRUDandViewPermissions("Administration.LoadIdIs");

            this.Namespace("Administration.ExecutionAction", "Выполнение действий");
            this.Permission("Administration.ExecutionAction.View", "Просмотр");

            this.Namespace("Administration.ImportExport", "Импорт/экспорт данных системы");
            this.Permission("Administration.ImportExport.View", "Просмотр раздела");
            this.Permission("Administration.ImportExport.Export", "Выгрузка данных");
            this.Permission("Administration.ImportExport.Import", "Загрузка данных");
            this.Permission("Administration.ImportExport.RisDataExport", "Экспорт данных системы в РИС ЖКХ");
            this.Permission("Administration.ImportExport.RisDataExportInfo", "Сведения об отправленных данных");

            this.Namespace("Administration.DataTransferIntegration", "Интеграция с внешней системой ЖКХ.Комплекс");
            this.Permission("Administration.DataTransferIntegration.View", "Просмотр раздела");
            this.Permission("Administration.DataTransferIntegration.Import", "Загрузка данных");

            this.Namespace("Administration.TableLock", "Реестр блокировок таблиц");
            this.Permission("Administration.TableLock.View", "Просмотр");
            this.Permission("Administration.TableLock.Edit", "Редактирование");

            this.Namespace("Administration.GkhParams", "Настройки приложения");
            this.Permission("Administration.GkhParams.View", "Просмотр");
            this.Permission("Administration.GkhParams.Edit", "Редактирование");

            this.Namespace("Administration.Oktmo", "ОКТМО");
            this.Namespace("Administration.Oktmo.fiasoktmo", "Привязка населенного пункта");

            this.Permission("Administration.Oktmo.fiasoktmo.View", "Просмотр раздела");
            this.Permission("Administration.Oktmo.fiasoktmo.Edit", "Редактирование");
            this.Permission("Administration.Oktmo.fiasoktmo.Delete", "Удаление");

            this.Namespace("Administration.Version", "Версия");
            this.Permission("Administration.Version.View", "Просмотр");

            #endregion

            #region Задачи
            this.Namespace("Tasks", "Задачи");
            this.Permission("Tasks.Delete_View", "Удалить задачу - Просмотр");
            this.Permission("Tasks.ClearRabbitMQ_View", "Очистить очередь - Просмотр");

            #endregion

            this.Namespace("Import", "Импорты");

            this.Namespace("Reports", "Отчеты");

            #region Виджеты

            this.Namespace("Widget", "Виджеты");
            this.Permission("Widget.News", "Новости");
            this.Permission("Widget.Faq", "Документация");
            this.Permission("Widget.ActiveOperator", "Панель оператора");
            #endregion

            this.Namespace("Gkh", "Модуль ЖКХ");

            this.Permission("Gkh.UpdateRetPreview_View", "Перечень обновляемых домов");

            #region Жилой дом
            this.Namespace("Gkh.RealityObject", "Жилые дома");
            this.CRUDandViewPermissions("Gkh.RealityObject");
            
            #region Жилой дом - поля

            this.Namespace<RealityObject>("Gkh.RealityObject.Field", "Поля");

            this.Namespace("Gkh.RealityObject.Field.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Field.View.FiasAddress_View", "Адрес дома");
            this.Permission("Gkh.RealityObject.Field.View.FiasHauseGuid_View", "Код ФИАС");
            this.Permission("Gkh.RealityObject.Field.View.TypeHouse_View", "Тип дома");
            this.Permission("Gkh.RealityObject.Field.View.BuildYear_View","Год постройки");
            this.Permission("Gkh.RealityObject.Field.View.PublishDate_View","Дата включения в ДПКР");
            this.Permission("Gkh.RealityObject.Field.View.DateLastOverhaul_View","Дата последнего кап. ремонта");
            this.Permission("Gkh.RealityObject.Field.View.HasPrivatizedFlats_View","Наличие приватизированных квартир");
            this.Permission("Gkh.RealityObject.Field.View.DateTechInspection_View","Дата тех. обследования");
            this.Permission("Gkh.RealityObject.Field.View.ConditionHouse_View","Состояние дома");
            this.Permission("Gkh.RealityObject.Field.View.ConditionHouse_EditButton", "Кнопка смены состояния дома");
            this.Permission("Gkh.RealityObject.Field.View.DateCommissioning_View","Дата сдачи в эксплуатацию");
            this.Permission("Gkh.RealityObject.Field.View.UnpublishDate_View","Дата исключения из ДПКР");
            this.Permission("Gkh.RealityObject.Field.View.DateCommissioningLastSection_View","Дата сдачи в эксплуатацию последней секции дома");
            this.Permission("Gkh.RealityObject.Field.View.PrivatizationDateFirstApartment_View","Дата приватизации первого жилого помещения");
            this.Permission("Gkh.RealityObject.Field.View.LatestTechnicalMonitoring_View","Дата последнего тех. мониторинга");
            this.Permission("Gkh.RealityObject.Field.View.ResidentsEvicted_View","Жильцы выселены");
            this.Permission("Gkh.RealityObject.Field.View.IsCulturalHeritage_View","Дом имеет статус ОКН");
            this.Permission("Gkh.RealityObject.Field.View.IsInsuredObject_View","Объект застрахован");
            this.Permission("Gkh.RealityObject.Field.View.IsNotInvolvedCr_View","Дом не участвует в КР");
            this.Permission("Gkh.RealityObject.Field.View.IsRepairInadvisable_View","Ремонт нецелесообразен");
            this.Permission("Gkh.RealityObject.Field.View.IsInvolvedCrTo2_View","Участвует в программе КР ТО №2");
            this.Permission("Gkh.RealityObject.Field.View.DateDemolition_View","Дата сноса");
            this.Permission("Gkh.RealityObject.Field.View.CulturalHeritageAssignmentDate_View","Дата присвоения");
            this.Permission("Gkh.RealityObject.Field.View.FederalNum_View","Федеральный номер");
            this.Permission("Gkh.RealityObject.Field.View.GkhCode_View","Код дома");
            this.Permission("Gkh.RealityObject.Field.View.AddressCode_View","Код адреса");
            this.Permission("Gkh.RealityObject.Field.View.CodeErc_View","Код ЕРЦ");
            this.Permission("Gkh.RealityObject.Field.View.PhysicalWear_View","Физический износ (%)");
            this.Permission("Gkh.RealityObject.Field.View.VtscpCode_View","Код дома для ВЦКП");
            this.Permission("Gkh.RealityObject.Field.View.BuildingFeature_View","Особые отметки дома");
            this.Permission("Gkh.RealityObject.Field.View.IsBuildSocialMortgage_View","Построен по соц. ипотеке");
            this.Permission("Gkh.RealityObject.Field.View.TypeOwnership_View","Форма собственности");
            this.Permission("Gkh.RealityObject.Field.View.TypeProject_View","Серия, тип проекта");
            this.Permission("Gkh.RealityObject.Field.View.RealEstateType_View","Классификация дома");
            this.Permission("Gkh.RealityObject.Field.View.RealEstateTypeNames_View","Классификация дома для расчета начислений");
            this.Permission("Gkh.RealityObject.Field.View.CapitalGroup_View","Группа капитальности");
            this.Permission("Gkh.RealityObject.Field.View.WebCameraUrl_View","Адрес веб-камеры");
            this.Permission("Gkh.RealityObject.Field.View.CadastreNumber_View","Кадастровый номер земельного участка");
            this.Permission("Gkh.RealityObject.Field.View.CadastralHouseNumber_View","Кадастровый номер дома");
            this.Permission("Gkh.RealityObject.Field.View.TotalBuildingVolume_View","Общий строительный объем (куб.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaMkd_View","Общая площадь (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaOwned_View","Площадь частной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaMunicipalOwned_View","Площадь муниципальной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaGovernmentOwned_View","Площадь государственной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaLivingNotLivingMkd_View","Общая площадь жилых и нежилых помещений (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaLiving_View","В т.ч. жилых всего (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaNotLivingPremises_View","В т. ч. нежилых всего (кв. м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaLivingOwned_View","В т.ч. жилых, находящихся в собственности граждан (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaNotLivingFunctional_View","Общая площадь помещений, входящих в состав общего имущества (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaCommonUsage_View","Площадь помещений общего пользования");
            this.Permission("Gkh.RealityObject.Field.View.NecessaryConductCr_View","Требовалось проведение КР на дату приватизации первого жилого помещения");
            this.Permission("Gkh.RealityObject.Field.View.MaximumFloors_View","Максимальная этажность");
            this.Permission("Gkh.RealityObject.Field.View.Floors_View","Минимальная этажность");
            this.Permission("Gkh.RealityObject.Field.View.FloorHeight_View","Высота этажа (м)");
            this.Permission("Gkh.RealityObject.Field.View.NumberEntrances_View","Количество подъездов");
            this.Permission("Gkh.RealityObject.Field.View.NumberApartments_View","Количество квартир");
            this.Permission("Gkh.RealityObject.Field.View.NumberNonResidentialPremises_View","Количество нежилых помещений");
            this.Permission("Gkh.RealityObject.Field.View.NumberLiving_View","Количество проживающих");
            this.Permission("Gkh.RealityObject.Field.View.NumberLifts_View","Количество лифтов");
            this.Permission("Gkh.RealityObject.Field.View.RoofingMaterial_View","Материал кровли");
            this.Permission("Gkh.RealityObject.Field.View.WallMaterial_View","Материал стен");
            this.Permission("Gkh.RealityObject.Field.View.TypeRoof_View","Тип кровли");
            this.Permission("Gkh.RealityObject.Field.View.PercentDebt_View","Собираемость платежей %");
            this.Permission("Gkh.RealityObject.Field.View.HeatingSystem_View","Система отопления");
            this.Permission("Gkh.RealityObject.Field.View.HasJudgmentCommonProp_View","Наличие судебного решения по проведению КР общего имущества");
            this.Permission("Gkh.RealityObject.Field.View.Notation_View","Примечание");
            this.Permission("Gkh.RealityObject.Field.Points_View", "Всего баллов - Просмотр");
            this.Permission("Gkh.RealityObject.Field.Points_Edit", "Всего баллов - Изменение");
            this.Permission("Gkh.RealityObject.Field.View.MonumentDocumentNumber_View", "Номер документа");
            this.Permission("Gkh.RealityObject.Field.View.MonumentFile_View", "Файл");
            this.Permission("Gkh.RealityObject.Field.View.MonumentDepartmentName_View",
                "Наименование органа, выдавшего документ о признании дома памятником архитектуры");
            this.Permission("Gkh.RealityObject.Field.View.IsIncludedRegisterCHO_View", "Включён в реестр ОКН");
            this.Permission("Gkh.RealityObject.Field.View.IsIncludedListIdentifiedCHO_View", "Включён в перечень выявленных ОКН");
            this.Permission("Gkh.RealityObject.Field.View.IsDeterminedSubjectProtectionCHO_View", "Предмет охраны ОКН определен");

            this.Namespace("Gkh.RealityObject.Field.Edit", "Редактирование");
            this.Permission("Gkh.RealityObject.Field.Edit.FiasAddress_Edit","Адрес дома");
            this.Permission("Gkh.RealityObject.Field.Edit.TypeHouse_Edit","Тип дома");
            this.Permission("Gkh.RealityObject.Field.Edit.BuildYear_Edit","Год постройки");
            this.Permission("Gkh.RealityObject.Field.Edit.PublishDate_Edit","Дата включения в ДПКР");
            this.Permission("Gkh.RealityObject.Field.Edit.DateLastOverhaul_Edit","Дата последнего кап. ремонта");
            this.Permission("Gkh.RealityObject.Field.Edit.HasPrivatizedFlats_Edit","Наличие приватизированных квартир");
            this.Permission("Gkh.RealityObject.Field.Edit.DateTechInspection_Edit","Дата тех. обследования");
            this.Permission("Gkh.RealityObject.Field.Edit.ConditionHouse_Edit","Состояние дома");
            this.Permission("Gkh.RealityObject.Field.Edit.DateCommissioning_Edit","Дата сдачи в эксплуатацию");
            this.Permission("Gkh.RealityObject.Field.Edit.UnpublishDate_Edit","Дата исключения из ДПКР");
            this.Permission("Gkh.RealityObject.Field.Edit.DateCommissioningLastSection_Edit","Дата сдачи в эксплуатацию последней секции дома");
            this.Permission("Gkh.RealityObject.Field.Edit.PrivatizationDateFirstApartment_Edit","Дата приватизации первого жилого помещения");
            this.Permission("Gkh.RealityObject.Field.Edit.LatestTechnicalMonitoring_Edit","Дата последнего тех. мониторинга");
            this.Permission("Gkh.RealityObject.Field.Edit.ResidentsEvicted_Edit","Жильцы выселены");
            this.Permission("Gkh.RealityObject.Field.Edit.IsCulturalHeritage_Edit","Дом имеет статус ОКН");
            this.Permission("Gkh.RealityObject.Field.Edit.IsInsuredObject_Edit","Объект застрахован");
            this.Permission("Gkh.RealityObject.Field.Edit.IsNotInvolvedCr_Edit","Дом не участвует в КР");
            this.Permission("Gkh.RealityObject.Field.Edit.IsRepairInadvisable_Edit","Ремонт нецелесообразен");
            this.Permission("Gkh.RealityObject.Field.Edit.IsInvolvedCrTo2_Edit","Участвует в программе КР ТО №2");
            this.Permission("Gkh.RealityObject.Field.Edit.DateDemolition_Edit","Дата сноса");
            this.Permission("Gkh.RealityObject.Field.Edit.CulturalHeritageAssignmentDate_Edit","Дата присвоения");
            this.Permission("Gkh.RealityObject.Field.Edit.FederalNum_Edit","Федеральный номер");
            this.Permission("Gkh.RealityObject.Field.Edit.GkhCode_Edit","Код дома");
            this.Permission("Gkh.RealityObject.Field.Edit.AddressCode_Edit","Код адреса");
            this.Permission("Gkh.RealityObject.Field.Edit.CodeErc_Edit","Код ЕРЦ");
            this.Permission("Gkh.RealityObject.Field.Edit.PhysicalWear_Edit","Физический износ (%)");
            this.Permission("Gkh.RealityObject.Field.Edit.VtscpCode_Edit","Код дома для ВЦКП");
            this.Permission("Gkh.RealityObject.Field.Edit.BuildingFeature_Edit","Особые отметки дома");
            this.Permission("Gkh.RealityObject.Field.Edit.IsBuildSocialMortgage_Edit","Построен по соц. ипотеке");
            this.Permission("Gkh.RealityObject.Field.Edit.TypeOwnership_Edit","Форма собственности");
            this.Permission("Gkh.RealityObject.Field.Edit.TypeProject_Edit","Серия, тип проекта");
            this.Permission("Gkh.RealityObject.Field.Edit.RealEstateType_Edit","Классификация дома");
            this.Permission("Gkh.RealityObject.Field.Edit.RealEstateTypeNames_Edit","Классификация дома для расчета начислений");
            this.Permission("Gkh.RealityObject.Field.Edit.CapitalGroup_Edit","Группа капитальности");
            this.Permission("Gkh.RealityObject.Field.Edit.WebCameraUrl_Edit","Адрес веб-камеры");
            this.Permission("Gkh.RealityObject.Field.Edit.CadastreNumber_Edit","Кадастровый номер земельного участка");
            this.Permission("Gkh.RealityObject.Field.Edit.CadastralHouseNumber_Edit","Кадастровый номер дома");
            this.Permission("Gkh.RealityObject.Field.Edit.TotalBuildingVolume_Edit","Общий строительный объем (куб.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaMkd_Edit","Общая площадь (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaOwned_Edit","Площадь частной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaMunicipalOwned_Edit","Площадь муниципальной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaGovernmentOwned_Edit","Площадь государственной собственности (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaLivingNotLivingMkd_Edit","Общая площадь жилых и нежилых помещений (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaLiving_Edit","В т.ч. жилых всего (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaNotLivingPremises_Edit","В т. ч. нежилых всего (кв. м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaLivingOwned_Edit","В т.ч. жилых, находящихся в собственности граждан (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaNotLivingFunctional_Edit","Общая площадь помещений, входящих в состав общего имущества (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.Edit.AreaCommonUsage_Edit","Площадь помещений общего пользования");
            this.Permission("Gkh.RealityObject.Field.Edit.NecessaryConductCr_Edit","Требовалось проведение КР на дату приватизации первого жилого помещения");
            this.Permission("Gkh.RealityObject.Field.Edit.MaximumFloors_Edit","Максимальная этажность");
            this.Permission("Gkh.RealityObject.Field.Edit.Floors_Edit","Минимальная этажность");
            this.Permission("Gkh.RealityObject.Field.Edit.FloorHeight_Edit","Высота этажа (м)");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberEntrances_Edit","Количество подъездов");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberApartments_Edit","Количество квартир");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberNonResidentialPremises_Edit","Количество нежилых помещений");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberLiving_Edit","Количество проживающих");
            this.Permission("Gkh.RealityObject.Field.Edit.NumberLifts_Edit","Количество лифтов");
            this.Permission("Gkh.RealityObject.Field.Edit.RoofingMaterial_Edit","Материал кровли");
            this.Permission("Gkh.RealityObject.Field.Edit.WallMaterial_Edit","Материал стен");
            this.Permission("Gkh.RealityObject.Field.Edit.TypeRoof_Edit","Тип кровли");
            this.Permission("Gkh.RealityObject.Field.Edit.PercentDebt_Edit","Собираемость платежей %");
            this.Permission("Gkh.RealityObject.Field.Edit.HeatingSystem_Edit","Система отопления");
            this.Permission("Gkh.RealityObject.Field.Edit.HasJudgmentCommonProp_Edit","Наличие судебного решения по проведению КР общего имущества");
            this.Permission("Gkh.RealityObject.Field.Edit.Notation_Edit","Примечание");
            this.Permission("Gkh.RealityObject.Field.Edit.MonumentDocumentNumber_Edit", "Номер документа");
            this.Permission("Gkh.RealityObject.Field.Edit.MonumentFile_Edit", "Файл");
            this.Permission("Gkh.RealityObject.Field.Edit.MonumentDepartmentName_Edit",
                "Наименование органа, выдавшего документ о признании дома памятником архитектуры");
            this.Permission("Gkh.RealityObject.Field.Edit.IsIncludedRegisterCHO_Edit", "Включён в реестр ОКН");
            this.Permission("Gkh.RealityObject.Field.Edit.IsIncludedListIdentifiedCHO_Edit", "Включён в перечень выявленных ОКН");
            this.Permission("Gkh.RealityObject.Field.Edit.IsDeterminedSubjectProtectionCHO_Edit", "Предмет охраны ОКН определен");

            this.Namespace("Gkh.RealityObject.MeteringDevicesChecks", "Проверки приборов учёта");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.View", "Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Delete", "Удаление записей");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Create", "Создание записей");
            this.Namespace("Gkh.RealityObject.MeteringDevicesChecks.Fields", "Поля");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.MeteringDevice_View", "Прибор учёта - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.ControlReading_View", "Контрольное показание - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.RemovalControlReadingDate_View", "Дата снятия контрольного показания - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.StartDateCheck_View", "Дата начала проверки - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.StartValue_View", "Значение показаний прибора учета на момент начала проверки - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.EndDateCheck_View", "Дата окончания проверки - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.EndValue_View", "Значение показаний на момент окончания проверки - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.IntervalVerification_View", "Межпроверочный интервал (лет) - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.MarkMeteringDevice_View", "Марка прибора учёта - Просмотр");
            this.Permission("Gkh.RealityObject.MeteringDevicesChecks.Fields.NextDateCheck_View", "Плановая дата следующей проверки - Просмотр");

            #endregion Жилой дом - поля

            #region Модуль ЖКХ/Жилые дома/Поля

            this.Permission("Gkh.RealityObject.Field.FillAreaOwned", "Заполнить площадь частной собственности - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaMunicipalOwned", "Заполнить площадь муниципальной площади - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaGovernmentOwned", "Заполнить площадь государственной площади - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaLivingNotLivingMkd", "Заполнить общую  площадь жилых и нежилых помещений - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaLiving", "Заполнить площадь жилых помещений - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaNotLivingPremises", "Заполнить площадь нежилых помещений - Просмотр");

            #endregion

            this.Namespace<RealityObject>("Gkh.RealityObject.Buttons", "Кнопки");
            this.Permission("Gkh.RealityObject.Buttons.SendToGZHI", "Уведомить ГЖИ");
            
            #region Жилой дом - реестры

            this.Namespace<RealityObject>("Gkh.RealityObject.Register", "Реестры");

            /*this.Namespace("Gkh.RealityObject.Register.MeteringDevice", "Приборы учета и узлы регулирования");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.MeteringDevice");
            this.Permission("Gkh.RealityObject.Register.MeteringDevice.SerialNumber_Edit", "Редактирование поля - Заводской номер прибора учёта");
            this.Permission("Gkh.RealityObject.Register.MeteringDevice.AddingReadingsManually_Edit", "Редактирование поля - Внесение показаний в ручном режиме");
            this.Permission("Gkh.RealityObject.Register.MeteringDevice.NecessityOfVerificationWhileExpluatation_Edit", "Редактирование поля - Обязательности поверки в рамках эксплуатации прибора учета");
            this.Permission("Gkh.RealityObject.Register.MeteringDevice.PersonalAccountNum_Edit", "Редактирование поля - Номер лицевого счёта");
            this.Permission("Gkh.RealityObject.Register.MeteringDevice.DateFirstVerification_Edit", "Редактирование поля - Дата первичной поверки");
            */

            this.Namespace("Gkh.RealityObject.Register.SKPT", "Сведения о СКПТ");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.SKPT");

            this.Namespace("Gkh.RealityObject.Register.SKPT", "Камеры видеонаблюдения");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Videcam");

            this.Namespace("Gkh.RealityObject.Register.Housekeeper", "Старший по дому");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Housekeeper");

            this.Namespace("Gkh.RealityObject.Register.CategoryCSMKD", "Категории МКД");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.CategoryCSMKD");

            this.Namespace("Gkh.RealityObject.Register.Image", "Фото-архив жилого дома");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Image");

            this.Namespace("Gkh.RealityObject.Register.ApartInfo", "Сведения о квартирах");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.ApartInfo");

            this.Namespace("Gkh.RealityObject.Register.HouseInfo", "Сведения о помещениях");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HouseInfo");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.ChamberNum.View", "Номер комнаты");

            this.Namespace("Gkh.RealityObject.Register.HouseInfo.Fields", "Поля");
            this.Namespace("Gkh.RealityObject.Register.HouseInfo.Fields.CadastralNumber", "Кадастровый номер");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.CadastralNumber.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.CadastralNumber.Edit", "Редактирование");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.IsCommunal_View", "Коммунальная квартира");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.IsRoomCommonPropertyInMcd_View", "Помещение составляет общее имущество в МКД");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.CommunalArea_View", "Площадь общего имущества в коммунальной квартире");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.PrevAssignedRegNumber_View", "Ранее присвоенный гос. учетный номер");

            this.Namespace("Gkh.RealityObject.Register.HouseInfo.Tabs", "Вкладки");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Tabs.InformationUnfitness_View", "Сведения о непригодности помещения");

            this.Namespace("Gkh.RealityObject.Register.HouseInfo.Fields.ChamberNum", "Номер комнаты");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.ChamberNum.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Register.HouseInfo.Fields.ChamberNum.Edit", "Редактирование");

            this.Namespace("Gkh.RealityObject.Register.Entrance", "Сведения о подъездах");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Entrance");

            this.Namespace("Gkh.RealityObject.Register.Block", "Сведения о блоках");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Block");
            this.Namespace("Gkh.RealityObject.Register.Block.Fields", "Поля");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.Number_View", "Номер блока - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.Number_Edit", "Номер блока - Изменение");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.AreaLiving_View", "Жилая площадь - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.AreaLiving_Edit", "Жилая площадь - Изменение");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.AreaTotal_View", "Общая площадь - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.AreaTotal_Edit", "Общая площадь - Изменение");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.CadastralNumber_View", "Кадастровый номер - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Block.Fields.CadastralNumber_Edit", "Кадастровый номер - Изменение");

            this.Namespace("Gkh.RealityObject.Register.TechnicalMonitoring", "Технический мониторинг");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.TechnicalMonitoring");


            this.Namespace("Gkh.RealityObject.Register.HousingComminalService", "Сведения по ЖКУ");
            this.Permission("Gkh.RealityObject.Register.HousingComminalService.View", "Просмотр");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.InfoOverview", "Общие сведения по дому");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.InfoOverview");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Account", "Лицевые счета дома");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.Account.AccountMeteringDeviceValues", "Показания приборов учета");
            this.Permission("Gkh.RealityObject.Register.HousingComminalService.Account.AccountMeteringDeviceValues.View", "Просмотр");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.Account");
            this.Namespace("Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue", "Показания общедомовых приборов учета");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue");

            this.Namespace("Gkh.RealityObject.Register.Land", "Земельные участки");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Land");
            
            /*this.Namespace("Gkh.RealityObject.Register.Belay", "Страхование объекта");
            this.Permission("Gkh.RealityObject.Register.Belay.View", "Просмотр");*/

            this.Namespace("Gkh.RealityObject.Register.ManagOrg", "Управление домом");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.ManagOrg");

            //this.Namespace("Gkh.RealityObject.Register.CashPaymentCenter", "Расчетно-кассовые центры");
            //this.Permission("Gkh.RealityObject.Register.CashPaymentCenter.View", "Просмотр");

            this.Namespace("Gkh.RealityObject.Register.ServiceOrg", "Поставщики жилищных услуг");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.ServiceOrg");
            
            this.Namespace("Gkh.RealityObject.Register.Councillors", "Совет МКД");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Councillors");

            this.Namespace("Gkh.RealityObject.Register.ConstructiveElement", "Конструктивные элементы");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.ConstructiveElement");

            this.Namespace("Gkh.RealityObject.Register.CurentRepair", "Текущий ремонт");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.CurentRepair");

            this.Namespace("Gkh.RealityObject.Register.ResOrg", "Поставщики коммунальных услуг");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.ResOrg");

            this.Namespace("Gkh.RealityObject.Register.ManagOrg.ServiceContract", "Договор оказания услуги с управляющей организацией");
            this.Permission("Gkh.RealityObject.Register.ManagOrg.ServiceContract.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Register.ManagOrg.ServiceContract.Edit", "Редактирование");

            #endregion

            #endregion Жилой дом
            
            #region Дворы
            this.Namespace("Gkh.RealityObjectOutdoor", "Дворы");
            this.CRUDandViewPermissions("Gkh.RealityObjectOutdoor");
            this.Namespace<RealityObjectOutdoor>("Gkh.RealityObjectOutdoor.Field", "Поля");

            this.Namespace("Gkh.RealityObjectOutdoor.Field.View", "Просмотр");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.Municipality_View", "Населенный пункт");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.Name_View", "Наименование двора");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.Code_View", "Код двора");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.Area_View", "Площадь двора (кв.м)");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.AsphaltArea_View", "Площадь асфальта (кв.м.)");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.Description_View", "Примечание");
            this.Permission("Gkh.RealityObjectOutdoor.Field.View.RepairPlanYear_View", "Плановый год ремонта");

            this.Namespace("Gkh.RealityObjectOutdoor.Field.Edit", "Редактирование");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.Municipality_Edit", "Населенный пункт");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.Name_Edit", "Наименование двора");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.Code_Edit", "Код двора");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.Area_Edit", "Площадь двора (кв.м)");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.AsphaltArea_Edit", "Площадь асфальта (кв.м.)");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.Description_Edit", "Примечание");
            this.Permission("Gkh.RealityObjectOutdoor.Field.Edit.RepairPlanYear_Edit", "Плановый год ремонта");
            #endregion

            #region Энергопаспорт Пустой

            #endregion Энергопаспорт Пустой

            #region Аварийный дом

            this.Namespace("Gkh.EmergencyObject", "Аварийные дома");
            this.CRUDandViewPermissions("Gkh.EmergencyObject");

            #region Аварийный дом - поля

            this.Namespace<EmergencyObject>("Gkh.EmergencyObject.Field", "Поля");
            this.Permission("Gkh.EmergencyObject.Field.RealityObject_Edit", "Жилой дом");
            this.Permission("Gkh.EmergencyObject.Field.ResettlementProgram_Edit", "Программа переселения");
            this.Permission("Gkh.EmergencyObject.Field.FurtherUse_Edit", "Дальнейшее использование");
            this.Permission("Gkh.EmergencyObject.Field.ReasonInexpedient_Edit", "Основание нецелесообразности");
            this.Permission("Gkh.EmergencyObject.Field.ActualInfoDate_Edit", "Дата актуальности информации");
            this.Permission("Gkh.EmergencyObject.Field.CadastralNumber_Edit", "Кадастровый номер");
            this.Permission("Gkh.EmergencyObject.Field.DemolitionDate_Edit", "Планируемая дата сноса/реконструкции МКД");
            this.Permission("Gkh.EmergencyObject.Field.ResettlementDate_Edit", "Планируемая дата завершения переселения");
            this.Permission("Gkh.EmergencyObject.Field.FactDemolitionDate_Edit", "Фактическая дата сноса/реконструкции МКД");
            this.Permission("Gkh.EmergencyObject.Field.FactResettlementDate_Edit", "Фактическая дата завершения переселения");
            this.Permission("Gkh.EmergencyObject.Field.LandArea_Edit", "Площадь земельного участка");
            this.Permission("Gkh.EmergencyObject.Field.ResettlementFlatArea_Edit", "Площадь расселяемых жилых помещений");
            this.Permission("Gkh.EmergencyObject.Field.ResettlementFlatAmount_Edit", "Количество расселяемых жилых помещений");
            this.Permission("Gkh.EmergencyObject.Field.InhabitantNumber_Edit", "Число жителей планируемых к переселению");
            this.Permission("Gkh.EmergencyObject.Field.IsRepairExpedient_Edit", "Ремонт целесообразен");
            this.Permission("Gkh.EmergencyObject.Field.ConditionHouse_Edit", "Состояние дома");
            this.Permission("Gkh.EmergencyObject.Field.DocumentName_Edit", "Наименование документа");
            this.Permission("Gkh.EmergencyObject.Field.DocumentNumber_Edit", "Номер документа");
            this.Permission("Gkh.EmergencyObject.Field.DocumentDate_Edit", "Дата документа");
            this.Permission("Gkh.EmergencyObject.Field.Description_Edit", "Описание");
            this.Permission("Gkh.EmergencyObject.Field.File_Edit", "Файл");
            this.Permission("Gkh.EmergencyObject.Field.State_Edit", "Статус");
            this.Permission("Gkh.EmergencyObject.Field.EmergencyDocumentName_Edit", "Наименование документа, подтверждающего аварийность МКД");
            this.Permission("Gkh.EmergencyObject.Field.EmergencyDocumentNumber_Edit", "Номер документа, подтверждающего аварийность МКД");
            this.Permission("Gkh.EmergencyObject.Field.EmergencyDocumentDate_Edit", "Дата документа, подтверждающего аварийность МКД");
            this.Permission("Gkh.EmergencyObject.Field.EmergencyFileInfo_Edit", "Файл документа, подтверждающего аварийность МКД");

            #endregion Аварийный дом - поля

            #region Аварийный дом - реестры
            this.Namespace("Gkh.EmergencyObject.Register", "Реестры");
            
            this.Namespace("Gkh.EmergencyObject.Register.ResettlementProgram", "Программы переселения");
            this.CRUDandViewPermissions("Gkh.EmergencyObject.Register.ResettlementProgram");

            this.Namespace("Gkh.EmergencyObject.Register.EmerObjResettlementProgram", "Разрезы финансирования");
            this.Permission("Gkh.EmergencyObject.Register.EmerObjResettlementProgram.Cost_Edit", "Плановая стоимость - Редактирование");
            this.Permission("Gkh.EmergencyObject.Register.EmerObjResettlementProgram.Cost_View", "Плановая стоимость - Просмотр");
            this.Permission("Gkh.EmergencyObject.Register.EmerObjResettlementProgram.ActualCost_Edit", "Фактическая стоимость - Редактирование");
            this.Permission("Gkh.EmergencyObject.Register.EmerObjResettlementProgram.ActualCost_View", "Фактическая стоимость - Просмотр");
            #endregion Аварийный дом - реестры

            #endregion Аварийный дом

            #region Импорт
            this.Namespace("Import.Gku", "Импорт из ЖКУ");
            this.Permission("Import.Gku.View", "Просмотр");

            this.Namespace("Import.RoImport", "Импорт жилых домов из Реформы ЖКХ");
            this.Permission("Import.RoImport.View", "Просмотр");

            this.Namespace("Import.RoImportFromFund", "Импорт жилых домов из фонда");
            this.Permission("Import.RoImportFromFund.View", "Просмотр");

            this.Namespace("Import.Billing", "Импорт из Биллинга");
            this.Permission("Import.Billing.View", "Просмотр");

            this.Namespace("Import.Operator", "Импорт операторов");
            this.Permission("Import.Operator.View", "Просмотр");

            this.Namespace("Import.ManagingOrganization", "Импорт лицензий");
            this.Permission("Import.ManagingOrganization.View", "Просмотр");

            this.Permission("Import.OrganizationImport", "Импорт организаций (с созданием договоров с домами)");
            this.Permission("Import.Oktmo", "Импорт данных ОКТМО");
            this.Permission("Import.MunicipalityFiasOktmo", "Импорт ОКТМО для населенных пунктов");

            #endregion

            #region Страхование деятельности УО
            this.Namespace("Gkh.BelayManOrgActivity", "Страхование деятельности УО");
            this.CRUDandViewPermissions("Gkh.BelayManOrgActivity");
            #endregion Страхование деятельности УО

            this.RegisterOrgsPermission();

            #region Должностное лицо
            this.Namespace<Person>("Gkh.Person", "Реестр должностных лиц");
            this.CRUDandViewPermissions("Gkh.Person");
            this.Permission("Gkh.Person.AddInContragent", "Кнопка 'Выбрать из контактов контрагентов' - Просмотр");

            // Делаю именно так потому что в данной ситуации и ДЛ и Заявки имею Статусы (и соответсвенн онастройки по статусам)
            // Но если мы добавить это ограничение для PersonRequestToExam , то по статусы ДЛ это нельзя будет настрйоить
            // А скрывать или показывать необходимо таблицу заявок именно по статусу ДЛ а не по статусу Заявки.
            this.Permission("Gkh.Person.RequestToExamView", "Вкладка 'Заявки на допуск к экзамену' - Просмотр");
            this.Permission("Gkh.Person.RequestToExamCreate", "Вкладка 'Заявки на допуск к экзамену' - Добавление"); 

            this.Namespace("Gkh.Person.Field", "Поля");
            this.Permission("Gkh.Person.Field.Surname_View", "Фамилия - Просмотр");
            this.Permission("Gkh.Person.Field.Surname_Edit", "Фамилия - Редактирование");
            this.Permission("Gkh.Person.Field.Name_View", "Имя - Просмотр");
            this.Permission("Gkh.Person.Field.Name_Edit", "Имя - Редактирование");
            this.Permission("Gkh.Person.Field.Patronymic_View", "Отчество - Просмотр");
            this.Permission("Gkh.Person.Field.Patronymic_Edit", "Отчество - Редактирование");
            this.Permission("Gkh.Person.Field.Email_View", "E-mail - Просмотр");
            this.Permission("Gkh.Person.Field.Email_Edit", "E-mail - Редактирование");
            this.Permission("Gkh.Person.Field.Phone_View", "Телефон - Просмотр");
            this.Permission("Gkh.Person.Field.Phone_Edit", "Телефон - Редактирование");
            this.Permission("Gkh.Person.Field.AddressReg_View", "Адрес регистрации - Просмотр");
            this.Permission("Gkh.Person.Field.AddressReg_Edit", "Адрес регистрации - Редактирование");
            this.Permission("Gkh.Person.Field.AddressLive_View", "Адрес места жительства - Просмотр");
            this.Permission("Gkh.Person.Field.AddressLive_Edit", "Адрес места жительства - Редактирование");
            this.Permission("Gkh.Person.Field.AddressBirth_View", "Адрес места рождения - Просмотр");
            this.Permission("Gkh.Person.Field.AddressBirth_Edit", "Адрес места рождения - Редактирование");
            this.Permission("Gkh.Person.Field.Birthdate_View", "Дата рождения - Просмотр");
            this.Permission("Gkh.Person.Field.Birthdate_Edit", "Дата рождения - Редактирование");
            this.Permission("Gkh.Person.Field.Inn_View", "ИНН - Просмотр");
            this.Permission("Gkh.Person.Field.Inn_Edit", "ИНН - Редактирование");
            this.Permission("Gkh.Person.Field.TypeIdentityDocument_View", "Тип документа - Просмотр");
            this.Permission("Gkh.Person.Field.TypeIdentityDocument_Edit", "Тип документа - Редактирование");
            this.Permission("Gkh.Person.Field.IdIssuedDate_View", "Дата выдачи - Просмотр");
            this.Permission("Gkh.Person.Field.IdIssuedDate_Edit", "Дата выдачи - Редактирование");
            this.Permission("Gkh.Person.Field.IdSerial_View", "Серия - Просмотр");
            this.Permission("Gkh.Person.Field.IdSerial_Edit", "Серия - Редактирование");
            this.Permission("Gkh.Person.Field.IdNumber_View", "Номер - Просмотр");
            this.Permission("Gkh.Person.Field.IdNumber_Edit", "Номер - Редактирование");
            this.Permission("Gkh.Person.Field.IdIssuedBy_View", "Кем выдан - Просмотр");
            this.Permission("Gkh.Person.Field.IdIssuedBy_Edit", "Кем выдан - Редактирование");
            
            this.Namespace<PersonRequestToExam>("Gkh.Person.RequestToExam", "Заявки на допуск к экзамену");
            this.Permission("Gkh.Person.RequestToExam.Edit", "Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Delete", "Удаление");
            //this.Permission("Gkh.Person.RequestToExam.View", "Просмотр"); Этого правила здесь быть недолжно поскольку у ДЛ и ЗАявки естьстатусы и настройки по статусам, Просмотр данного грида должен быт ьтолько по статусу Person 
            //this.Permission("Gkh.Person.RequestToExam.Create", "Добавление"); - Данное правило тут также быт ьнедолжно по темже причинам. см. выше

            this.Namespace("Gkh.Person.RequestToExam.Field", "Поля");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestSupplyMethod_View", "Способ подачи заявления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestSupplyMethod_Edit", "Способ подачи заявления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestNum_View", "Номер заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestNum_Edit", "Номер заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestDate_View", "Дата заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestDate_Edit", "Дата заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestTime_View", "Время заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestTime_Edit", "Время заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestFile_View", "Файл заявления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestFile_Edit", "Файл заявления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_View", "Файл согласия на обработку перс.данных - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_Edit", "Файл согласия на обработку перс.данных - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationNum_View", "Номер уведомления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationNum_Edit", "Номер уведомления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationDate_View", "Дата уведомления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationDate_Edit", "Дата уведомления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.IsDenied_View", "Отказ - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.IsDenied_Edit", "Отказ - Редактирование");

            this.Permission("Gkh.Person.RequestToExam.Field.ExamDate_View", "Дата экзамена - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamDate_Edit", "Дата экзамена - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamTime_View", "Время экзамена - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamTime_Edit", "Время экзамена - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_View", "Количество набранных баллов - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_Edit", "Количество набранных баллов - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolNum_View", "Номер протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolNum_Edit", "Номер протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolDate_View", "Дата протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolDate_Edit", "Дата протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolFile_View", "Файл протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolFile_Edit", "Файл протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationNum_View", "Номер уведомления (из блока 'Результаты экзамена') - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationNum_Edit", "Номер уведомления (из блока 'Результаты экзамена') - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationDate_View", "Дата уведомления (из блока 'Результаты экзамена') - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationDate_Edit", "Дата уведомления (из блока 'Результаты экзамена') - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.MailingDate_View", "Дата отправки почтой - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.MailingDate_Edit", "Дата отправки почтой - Редактирование");

            this.Namespace("Gkh.Person.Qualification", "Квалификационные аттестаты");
            this.Permission("Gkh.Person.Qualification.Create", "Добавление");
            this.Permission("Gkh.Person.Qualification.Edit", "Редактирование");
            this.Permission("Gkh.Person.Qualification.Delete", "Удаление");

            this.Namespace("Gkh.Person.Qualification.Field", "Поля");
            this.Permission("Gkh.Person.Qualification.Field.RequestToExam_View", "Заявка на доступ к экзамену - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RecieveDate_View", "Дата получения - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RecieveDate_Edit", "Дата получения - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.QualificationDocument_View", "Документ аттестата - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.QualificationDocument_Edit", "Документ аттестата - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.QualificationNotification_View", "Уведомление лицензионной комиссии о результатах экзамена - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.QualificationNotification_Edit", "Уведомление лицензионной комиссии о результатах экзамена - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.IsFromAnotherRegion_View", "КА другого региона - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.IsFromAnotherRegion_Edit", "КА другого региона - Редактирование");
            this.Permission("Gkh.Person.Qualification.Field.RegionCode_View", "Наименование региона - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RegionCode_Edit", "Наименование региона - Редактирование");

            this.Namespace("Gkh.Person.Qualification.Duplicate", "Дубликаты");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.Duplicate");

            this.Namespace("Gkh.Person.Qualification.Duplicate", "Переоформление");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.Renew");

            this.Namespace("Gkh.Person.Qualification.TechnicalMistake", "Информация о тех. ошибках");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.TechnicalMistake");

            this.Namespace("Gkh.RequestToExamRegister", "Реестр заявок на допуск к экзамену");
            this.Permission("Gkh.RequestToExamRegister.View", "Просмотр");

            this.Namespace("Gkh.Person.PersonDisqualificationInfo", "Сведения о дисквалификации");
            this.CRUDandViewPermissions("Gkh.Person.PersonDisqualificationInfo");

            this.Namespace("Gkh.Person.PersonPlaceWork", "Место работы");
            this.CRUDandViewPermissions("Gkh.Person.PersonPlaceWork");
            #endregion Должностное лицо

            #region Лицензия УО
            this.Namespace("Gkh.ManOrgLicense", "Лицензирование УО");

            this.Namespace<ManOrgLicense>("Gkh.ManOrgLicense.License", "Реестр лицензий");
            this.Permission("Gkh.ManOrgLicense.License.View", "Просмотр");
            this.Permission("Gkh.ManOrgLicense.License.Edit", "Редактирование");
            this.Permission("Gkh.ManOrgLicense.License.Delete", "Удаление");

            this.Namespace<ManOrgLicense>("Gkh.ManOrgLicense.License.Field", "Поля");

            this.Permission("Gkh.ManOrgLicense.License.Field.LicNum_Edit", "Номер");


            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request", "Заявки на выдачу лицензий");
            this.CRUDandViewPermissions("Gkh.ManOrgLicense.Request");

            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request.SubmittedDocs", "Предоставленные документы");
            this.CRUDandViewPermissions("Gkh.ManOrgLicense.Request.SubmittedDocs");

            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request.Field", "Поля");

            this.Permission("Gkh.ManOrgLicense.Request.Field.RegisterNum_Edit", "Регистрационный номер - Редактирование");

            this.Namespace("Gkh.ManOrgLicense.Request.Reports", "Отчеты");
            this.Permission("Gkh.ManOrgLicense.Request.Reports.Print", "Печать");
            #endregion

            #region Отчеты

            this.Namespace("Reports.GKH", "Модуль ЖКХ");
            this.Permission("Reports.GKH.AdviceMKDReport", "Отчет по Совету МКД");
            this.Permission("Reports.GKH.InformationByFloors", "Справка по этажности (Приложение 7)");
            this.Permission("Reports.GKH.NoteByReestrHouses", "Выписка из реестра домов (Приложение 2)");
            this.Permission("Reports.GKH.ReferenceOnGroundsAccident", "Справка по основаниям аварийности (Приложение 4)");
            this.Permission("Reports.GKH.InformationOnApartments", "Сведения по квартирам");
            this.Permission("Reports.GKH.InformationOnUseBuildings", "Справка по использованию домов (Приложение 3)");
            this.Permission("Reports.GKH.InformationByEmergencyObjects", "Сведения по аварийным домам");
            this.Permission("Reports.GKH.ReferenceByYearConstruction", "Справка по годам возведения (Приложение 5)");
            this.Permission("Reports.GKH.ControlActivityDatOfContractByUK", "Контроль заполнения сроков действия договора с УК");
            this.Permission("Reports.GKH.ReferenceWallMaterial", "Справка по материалу стен (Приложение 6)");
            this.Permission("Reports.GKH.MakingProtocolsOwnersControl", "Контроль внесения прокотолов собраний собственников о выборе УО");
            this.Permission("Reports.GKH.OwnersProtocolsControlManOrg", "Контроль внесения прокотолов собраний собственников о выборе УО -2");
            this.Permission("Reports.GKH.ContragentReport", "Отчет по контрагентам");
            this.Permission("Reports.GKH.ManOrgLicenseRequestLicenseApplicationPrimary", "Заявление о предоставлении лицензии (первичное обращение)");
            this.Permission("Reports.GKH.ManOrgLicenseRequestLicenseApplicationJurPerson", "Заявление о предоставлении лицензии (реорганизация юр. лица)");
            this.Permission("Reports.GKH.ManOrgLicenseRequestLicenseRenewalApplication", "Заявление о переоформлении лицензии");
            this.Permission("Reports.GKH.ManOrgLicenseRequestLicenseDuplicateApplication", "Заявление о выдаче дубликата лицензии");
            this.Permission("Reports.GKH.ManOrgLicenseRequestManOrgLicenseTaxRequest", "Запрос в налоговую");
            this.Permission("Reports.GKH.ManOrgLicenseRequestManOrgLicenseRequestMvdReport", "Запрос в ОМВД");
            this.Permission("Reports.GKH.ManOrgLicenseRequestManOrgLicenseRequestTreasuryReport", "Запрос в федеральное казначейство");
            this.Permission("Reports.GKH.ManOrgLicenseRequestManOrgLicenseRequestOrderReport", "Приказ");

            this.Permission("Reports.GKH.MotivatedProposalForLicensingReport", "Мотивированное предложение о выдаче лицензии");
            this.Permission("Reports.GKH.NotificationRefusalToIssueLicenseReport", "Уведомление об отказе в выдаче лицензии");
            this.Permission("Reports.GKH.ProtocolMeetingLicensingCommissionReport", "Протокол заседания лицензионной комиссии");
            this.Permission("Reports.GKH.NotificationAboutResultsQualificationExaminationReport", "Уведомление о результатах квалификационного экзамена");
            this.Permission("Reports.GKH.NotificationOfAdmissionExamReport", "Уведомление о допуске к экзамену");
            this.Permission("Reports.GKH.QualificationExaminationResultsReport", "Протокол результатов квалификационного экзамена");

            #endregion

            this.RegisterDictionaryPermission();
        }

        /// <summary>
        /// Участники процесса
        /// </summary>
        private void RegisterOrgsPermission()
        {
            this.Namespace("Gkh.Orgs", "Участники процесса");

            this.Namespace("Gkh.Orgs.Contragent", "Контрагенты");
            this.CRUDandViewPermissions("Gkh.Orgs.Contragent");

            #region Общие сведения
            this.Namespace("Gkh.Orgs.Contragent.Field", "Поля");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguRegNumber_View", "Реестровый номер функции в ФРГУ - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguRegNumber_Edit", "Реестровый номер функции в ФРГУ - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguOrgNumber_View", "Номер организации в ФРГУ - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguOrgNumber_Edit", "Номер организации в ФРГУ - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguServiceNumber_View", "Номер услуги в ФРГУ - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.FrguServiceNumber_Edit", "Номер услуги в ФРГУ - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.AddressCoords_View", "Адреса - Координаты - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.AddressCoords_Edit", "Адреса - Координаты - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.ActivityStage_View", "Cтадии деятельности - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.ActivityStage_Edit", "Cтадии деятельности - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.MainRole_View", "Основная роль - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.MainRole_Edit", "Основная роль - Изменение");
            this.Permission("Gkh.Orgs.Contragent.ChangeLog_View", "История изменений");
            this.Permission("Gkh.Orgs.Contragent.AdditionRole_View", "Дополнительная роль");

            this.Permission("Gkh.Orgs.Contragent.Field.TimeZoneType_View", "Часовая зона - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.TimeZoneType_Edit", "Часовая зона - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.Okogu_View", "ОКОГУ - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.Okogu_Edit", "ОКОГУ - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.Okfs_View", "ОКФС - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.Okfs_Edit", "ОКФС - Изменение");
            this.Permission("Gkh.Orgs.Contragent.Field.ReceiveNotifications_View", "Получать оповещения на E-mail - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Field.ReceiveNotifications_Edit", "Получать оповещения на E-mail - Изменение");

            #endregion

            #region Контрагенты реестры
            this.Namespace("Gkh.Orgs.Contragent.Register", "Реестры");

            this.Namespace("Gkh.Orgs.Contragent.Register.Contact", "Контакты");
            this.CRUDandViewPermissions("Gkh.Orgs.Contragent.Register.Contact");
            this.Permission("Gkh.Orgs.Contragent.Register.Contact.ChangeLog_View", "История изменений");

            this.Namespace("Gkh.Orgs.Contragent.Register.Bank", "Обслуживающие банки");
            this.CRUDandViewPermissions("Gkh.Orgs.Contragent.Register.Bank");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.ChangeLog_View", "История изменений");

            this.Namespace("Gkh.Orgs.Contragent.Register.CasesEdit", "Падежи");
            this.CRUDandViewPermissions("Gkh.Orgs.Contragent.Register.CasesEdit");

            this.Namespace("Gkh.Orgs.Contragent.Register.Risk", "Категории риска");
            this.Permission("Gkh.Orgs.Contragent.Register.Risk.View", "Просмотр");

            this.Namespace("Gkh.Orgs.Contragent.Register.Municipality", "Муниципальные образования");
            this.Permission("Gkh.Orgs.Contragent.Register.Municipality.View", "Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.Municipality.Create", "Создание");
            this.Permission("Gkh.Orgs.Contragent.Register.Municipality.Delete", "Удаление");
            #endregion

            this.Namespace("Gkh.Orgs.Managing", "Управляющие организации");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing");

            this.Namespace("Gkh.Orgs.Managing.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.Managing.GoToContragent.View", "Просмотр");

            #region Управляющие орг реестры
            this.Namespace("Gkh.Orgs.Managing.Register", "Реестры");

            this.Namespace("Gkh.Orgs.Managing.Register.RealityObject", "Жилые дома");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.RealityObject");

            this.Namespace("Gkh.Orgs.Managing.Register.Contract", "Управление домами");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Contract");

            #region Управляющие орг реестры
            this.Namespace("Gkh.Orgs.Managing.Register.Contract.Payment", "Сведения о плате");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.View", "Просмотр раздела");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_Edit", "Дата начала периода - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_View", "Дата начала периода - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_Edit", "Дата окончания периода - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_View", "Дата окончания периода - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_Edit", "Размер платы (УК/Прочее) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_View", "Размер платы (УК/Прочее) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_Edit", "Протокол (УК/Прочее) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_View", "Протокол (УК/Прочее) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_Edit", "Описание протокола (УК/Прочее) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_View", "Описание протокола (УК/Прочее) - Просмотр");


            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyReqiredPaymentAmount_Edit", "Размер обязательных платежей/взносов (ТСЖ/ЖСК - члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyReqiredPaymentAmount_View", "Размер обязательных платежей/взносов (ТСЖ/ЖСК - члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolFile_Edit", "Протокол собрания членов товарищества, кооператива (ТСЖ/ЖСК - члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolFile_View", "Протокол собрания членов товарищества, кооператива (ТСЖ/ЖСК - члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolDescription_Edit", "Описание протокола (ТСЖ/ЖСК - члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolDescription_View", "Описание протокола (ТСЖ/ЖСК - члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentAmount_Edit", "Размер платежей/взносов (ТСЖ/ЖСК - не члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentAmount_View", "Размер платежей/взносов (ТСЖ/ЖСК - не члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolFile_Edit", "Протокол собрания членов товарищества, кооператива (ТСЖ/ЖСК - не члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolFile_View", "Протокол собрания членов товарищества, кооператива (ТСЖ/ЖСК - не члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolDescription_Edit", "Описание протокола (ТСЖ/ЖСК - не члены товарищества) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolDescription_View", "Описание протокола (ТСЖ/ЖСК - не члены товарищества) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_Edit", "Основание установления размера платы за содержание жилого помещения (УК/Прочее) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_View", "Основание установления размера платы за содержание жилого помещения (УК/Прочее) - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_Edit", "Причина аннулирования (УК/Прочее) - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_View", "Причина аннулирования (УК/Прочее) - Просмотр");

            #endregion Управляющие орг реестры

            this.Namespace("Gkh.Orgs.Managing.Register.Contract.ControlTransfer", "Передача управления");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Contract.ControlTransfer");

            this.Namespace("Gkh.Orgs.Managing.Register.Contract.Field", "Поля");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_Edit", "Основание завершения обслуживания - Редактирование");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_View", "Основание завершения обслуживания - Просмотр");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View", "День месяца начала ввода показаний по приборам учета - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_Edit", "День месяца начала ввода показаний по приборам учета - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_View", "День месяца окончания ввода показаний по приборам учета - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_Edit", "День месяца окончания ввода показаний по приборам учета - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_View", "День выставления платежных документов - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_Edit", "День выставления платежных документов - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_View", "Текущего/следующего месяца - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_Edit", "Текущего/следующего месяца - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View", "Номер протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit", "Номер протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View", "Дата протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit", "Дата протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View", "Файл протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit", "Файл протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View", "Основание расторжения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit", "Основание расторжения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View", "Номер протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit", "Номер протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View", "Дата протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit", "Дата протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View", "Файл протокола - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit", "Файл протокола - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View", "Дата документа - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit", "Дата документа - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View", "Дата начала управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit", "Дата начала управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View", "Плановая дата окончания - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit", "Плановая дата окончания - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View", "Дата окончания управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit", "Дата окончания управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View", "Договор управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit", "Договор управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.Note_View", "Примечание - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit", "Примечание - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceRegister_View", "Дата внесения в реестр лицензий - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceRegister_Edit", "Дата внесения в реестр лицензий - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceDelete_View", "Дата исключения из реестра лицензий - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceDelete_Edit", "Дата исключения из реестра лицензий - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.RegisterReason_View", "Основание включения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.RegisterReason_Edit", "Основание включения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DeleteReason_View", "Основание исключения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DeleteReason_Edit", "Основание исключения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View", "Основание расторжения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit", "Основание расторжения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View", "Дата документа - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit", "Дата документа - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View", "Дата начала управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit", "Дата начала управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View", "Плановая дата окончания - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit", "Плановая дата окончания - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View", "Дата окончания управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit", "Дата окончания управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View", "Договор управления - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit", "Договор управления - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.Note_View", "Примечание - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit", "Примечание - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_View", "Основание - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_Edit", "Основание - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_View", "Дата расторжения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_Edit", "Дата расторжения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_View", "Файл расторжения - Просмотр");
            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_Edit", "Файл расторжения - Редактирование");

            this.Permission("Gkh.Orgs.Managing.Register.Contract.Field.RealityObjectId_Edit", "Жилой дом - Редактирование");

            this.Namespace("Gkh.Orgs.Managing.Register.Documentation", "Документы");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Documentation");

            this.Namespace("Gkh.Orgs.Managing.Register.WorkMode", "Режимы работы");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.WorkMode");

            this.Namespace("Gkh.Orgs.Managing.Register.Membership", "Членство в объединениях");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Membership");

            this.Namespace("Gkh.Orgs.Managing.Register.Activity", "Прекращение деятельности");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Activity");

            this.Namespace("Gkh.Orgs.Managing.Register.Registry", "Реестры");
            this.CRUDandViewPermissions("Gkh.Orgs.Managing.Register.Registry");
            #endregion

            this.Namespace("Gkh.Orgs.SupplyResource", "Поставщики коммунальных услуг");
            this.CRUDandViewPermissions("Gkh.Orgs.SupplyResource");

            this.Namespace("Gkh.Orgs.SupplyResource.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.SupplyResource.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.SupplyResource.RealtyObject", "Жилые дома");
            this.Permission("Gkh.Orgs.SupplyResource.RealtyObject.View", "Просмотр");
            this.Permission("Gkh.Orgs.SupplyResource.RealtyObject.Edit", "Редактирование (добавление/удаление)");

            this.Namespace("Gkh.Orgs.SupplyResource.ContractsWithRealObj", "Договора с жилыми домами");
            this.Permission("Gkh.Orgs.SupplyResource.ContractsWithRealObj.View", "Просмотр");
            this.Permission("Gkh.Orgs.SupplyResource.ContractsWithRealObj.Edit", "Редактирование (добавление/удаление)");

            this.Namespace("Gkh.Orgs.Serv", "Поставщики жилищных услуг");
            this.CRUDandViewPermissions("Gkh.Orgs.Serv");

            this.Namespace("Gkh.Orgs.Serv.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.Serv.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.Serv.RealtyObject", "Жилые дома");
            this.Permission("Gkh.Orgs.Serv.RealtyObject.View", "Просмотр");
            this.Permission("Gkh.Orgs.Serv.RealtyObject.Edit", "Редактирование (добавление/удаление)");

            this.Namespace("Gkh.Orgs.Serv.RealityObjectContract", "Договора с жилыми домами");
            this.Permission("Gkh.Orgs.Serv.RealityObjectContract.View", "Просмотр");
            this.Permission("Gkh.Orgs.Serv.RealityObjectContract.Edit", "Редактирование (добавление/удаление)");

            this.Namespace("Gkh.Orgs.Builder", "Подрядные организации");
            this.CRUDandViewPermissions("Gkh.Orgs.Builder");

            this.Namespace("Gkh.Orgs.Builder.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.Builder.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.Builder.Field", "Поля");
            this.Permission("Gkh.Orgs.Builder.Field.AdvancedTechnologies_View", "Применение прогрессивных технологий - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Field.AdvancedTechnologies_Edit", "Применение прогрессивных технологий - Изменение");

            this.Permission("Gkh.Orgs.Builder.Field.ConsentInfo_View", "Согласие на предоставление информации - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Field.ConsentInfo_Edit", "Согласие на предоставление информации - Изменение");

            this.Permission("Gkh.Orgs.Builder.Field.FileLearningPlan_View", "План обучения (переподготовки) кадров - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Field.FileLearningPlan_Edit", "План обучения (переподготовки) кадров - Изменение");

            this.Permission("Gkh.Orgs.Builder.Field.FileManningShedulle_View", "Штатное расписание кадров - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Field.FileManningShedulle_Edit", "Штатное расписание кадров - Изменение");

            this.Namespace("Gkh.Orgs.Builder.Register", "Реестры");
            this.Namespace("Gkh.Orgs.Builder.Register.Feedback", "Отзывы заказчиков");
            this.Permission("Gkh.Orgs.Builder.Register.Feedback.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.Loan", "Займы");
            this.Permission("Gkh.Orgs.Builder.Register.Loan.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.ProductionBase", "Производственные базы");
            this.Permission("Gkh.Orgs.Builder.Register.ProductionBase.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.SroInfo", "Сведения об участии в СРО");
            this.Permission("Gkh.Orgs.Builder.Register.SroInfo.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.Technique", "Техника, инструменты");
            this.Permission("Gkh.Orgs.Builder.Register.Technique.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.Workforce", "Cостав трудовых ресурсов");
            this.Permission("Gkh.Orgs.Builder.Register.Workforce.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.Activity", "Деятельность");
            this.Permission("Gkh.Orgs.Builder.Register.Activity.View", "Просмотр");
            this.Namespace("Gkh.Orgs.Builder.Register.Document", "Документы");
            this.Namespace("Gkh.Orgs.Builder.Register.Document.Column", "Колонки");
            this.Permission("Gkh.Orgs.Builder.Register.Document.Column.Period", "Период");
            this.Namespace("Gkh.Orgs.Builder.Register.Document.Field", "Поля");
            this.Permission("Gkh.Orgs.Builder.Register.Document.Field.DocumentExist_View", "Наличие документа - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Register.Document.Field.DocumentExist_Edit", "Наличие документа - Изменение");
            this.Permission("Gkh.Orgs.Builder.Register.Document.Field.Period_View", "Период - Просмотр");
            this.Permission("Gkh.Orgs.Builder.Register.Document.Field.Period_Edit", "Период - Изменение");


            this.Namespace("Gkh.Orgs.Belay", "Страховые организации");
            this.CRUDandViewPermissions("Gkh.Orgs.Belay");

            this.Namespace("Gkh.Orgs.Belay.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.Belay.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.LocalGov", "Органы местного самоуправления");
            this.CRUDandViewPermissions("Gkh.Orgs.LocalGov");

            this.Namespace("Gkh.Orgs.LocalGov.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.LocalGov.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.PaymentAgent", "Платежные агенты");
            this.CRUDandViewPermissions("Gkh.Orgs.PaymentAgent");

            this.Namespace("Gkh.Orgs.PaymentAgent.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.PaymentAgent.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.PoliticAuth", "Органы государственной власти");
            this.CRUDandViewPermissions("Gkh.Orgs.PoliticAuth");

            this.Namespace("Gkh.Orgs.PoliticAuth.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.PoliticAuth.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.TechnicalCustomer", "Технические заказчики");
            this.CRUDandViewPermissions("Gkh.Orgs.TechnicalCustomer");

            this.Namespace("Gkh.Orgs.HousingInspection", "Жилищные инспекции");
            this.CRUDandViewPermissions("Gkh.Orgs.HousingInspection");
            this.Namespace("Gkh.Orgs.TechnicalCustomer.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.TechnicalCustomer.GoToContragent.View", "Просмотр");

            #region Контрагенты ПИР
            this.Namespace("Gkh.Orgs.ContragentClw", "Контрагенты ПИР");
            this.CRUDandViewPermissions("Gkh.Orgs.ContragentClw");

            this.Namespace("Gkh.Orgs.ContragentClw.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.ContragentClw.GoToContragent.View", "Просмотр");
            
            // Делаю именно так потому что в данной ситуации и ДЛ и Заявки имею Статусы (и соответсвенн онастройки по статусам)
            // Но если мы добавить это ограничение для PersonRequestToExam , то по статусы ДЛ это нельзя будет настрйоить
            // А скрывать или показывать необходимо таблицу заявок именно по статусу ДЛ а не по статусу Заявки.
            this.Permission("Gkh.Person.RequestToExamView", "Вкладка 'Заявки на допуск к экзамену' - Просмотр");
            this.Permission("Gkh.Person.RequestToExamCreate", "Вкладка 'Заявки на допуск к экзамену' - Добавление"); 

            this.Namespace("Gkh.Person.Field", "Поля");
            this.Permission("Gkh.Person.Field.Surname_View", "Фамилия - Просмотр");
            this.Permission("Gkh.Person.Field.Surname_Edit", "Фамилия - Редактирование");
            this.Permission("Gkh.Person.Field.Name_View", "Имя - Просмотр");
            this.Permission("Gkh.Person.Field.Name_Edit", "Имя - Редактирование");
            this.Permission("Gkh.Person.Field.Patronymic_View", "Отчество - Просмотр");
            this.Permission("Gkh.Person.Field.Patronymic_Edit", "Отчество - Редактирование");
            this.Permission("Gkh.Person.Field.Email_View", "E-mail - Просмотр");
            this.Permission("Gkh.Person.Field.Email_Edit", "E-mail - Редактирование");
            this.Permission("Gkh.Person.Field.Phone_View", "Телефон - Просмотр");
            this.Permission("Gkh.Person.Field.Phone_Edit", "Телефон - Редактирование");
            this.Permission("Gkh.Person.Field.AddressReg_View", "Адрес регистрации - Просмотр");
            this.Permission("Gkh.Person.Field.AddressReg_Edit", "Адрес регистрации - Редактирование");
            this.Permission("Gkh.Person.Field.AddressLive_View", "Адрес места жительства - Просмотр");
            this.Permission("Gkh.Person.Field.AddressLive_Edit", "Адрес места жительства - Редактирование");
            this.Permission("Gkh.Person.Field.AddressBirth_View", "Адрес места рождения - Просмотр");
            this.Permission("Gkh.Person.Field.AddressBirth_Edit", "Адрес места рождения - Редактирование");
            this.Permission("Gkh.Person.Field.Birthdate_View", "Дата рождения - Просмотр");
            this.Permission("Gkh.Person.Field.Birthdate_Edit", "Дата рождения - Редактирование");
            this.Permission("Gkh.Person.Field.Inn_View", "ИНН - Просмотр");
            this.Permission("Gkh.Person.Field.Inn_Edit", "ИНН - Редактирование");
            this.Permission("Gkh.Person.Field.TypeIdentityDocument_View", "Тип документа - Просмотр");
            this.Permission("Gkh.Person.Field.TypeIdentityDocument_Edit", "Тип документа - Редактирование");
            this.Permission("Gkh.Person.Field.IdIssuedDate_View", "Дата выдачи - Просмотр");
            this.Permission("Gkh.Person.Field.IdIssuedDate_Edit", "Дата выдачи - Редактирование");
            this.Permission("Gkh.Person.Field.IdSerial_View", "Серия - Просмотр");
            this.Permission("Gkh.Person.Field.IdSerial_Edit", "Серия - Редактирование");
            this.Permission("Gkh.Person.Field.IdNumber_View", "Номер - Просмотр");
            this.Permission("Gkh.Person.Field.IdNumber_Edit", "Номер - Редактирование");
            this.Permission("Gkh.Person.Field.IdIssuedBy_View", "Кем выдан - Просмотр");
            this.Permission("Gkh.Person.Field.IdIssuedBy_Edit", "Кем выдан - Редактирование");
            
            this.Namespace<PersonRequestToExam>("Gkh.Person.RequestToExam", "Заявки на допуск к экзамену");
            this.Permission("Gkh.Person.RequestToExam.Edit", "Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Delete", "Удаление");
            //this.Permission("Gkh.Person.RequestToExam.View", "Просмотр"); Этого правила здесь быть недолжно поскольку у ДЛ и ЗАявки естьстатусы и настройки по статусам, Просмотр данного грида должен быт ьтолько по статусу Person 
            //this.Permission("Gkh.Person.RequestToExam.Create", "Добавление"); - Данное правило тут также быт ьнедолжно по темже причинам. см. выше

            this.Namespace("Gkh.Person.RequestToExam.Field", "Поля");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestSupplyMethod_View", "Способ подачи заявления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestSupplyMethod_Edit", "Способ подачи заявления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestNum_View", "Номер заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestNum_Edit", "Номер заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestDate_View", "Дата заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestDate_Edit", "Дата заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestTime_View", "Время заявки - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestTime_Edit", "Время заявки - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestFile_View", "Файл заявления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.RequestFile_Edit", "Файл заявления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_View", "Файл согласия на обработку перс.данных - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_Edit", "Файл согласия на обработку перс.данных - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationNum_View", "Номер уведомления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationNum_Edit", "Номер уведомления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationDate_View", "Дата уведомления - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.NotificationDate_Edit", "Дата уведомления - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.IsDenied_View", "Отказ - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.IsDenied_Edit", "Отказ - Редактирование");

            this.Permission("Gkh.Person.RequestToExam.Field.ExamDate_View", "Дата экзамена - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamDate_Edit", "Дата экзамена - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamTime_View", "Время экзамена - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ExamTime_Edit", "Время экзамена - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_View", "Количество набранных баллов - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_Edit", "Количество набранных баллов - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolNum_View", "Номер протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolNum_Edit", "Номер протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolDate_View", "Дата протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolDate_Edit", "Дата протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolFile_View", "Файл протокола - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ProtocolFile_Edit", "Файл протокола - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationNum_View", "Номер уведомления (из блока 'Результаты экзамена') - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationNum_Edit", "Номер уведомления (из блока 'Результаты экзамена') - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationDate_View", "Дата уведомления (из блока 'Результаты экзамена') - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.ResultNotificationDate_Edit", "Дата уведомления (из блока 'Результаты экзамена') - Редактирование");
            this.Permission("Gkh.Person.RequestToExam.Field.MailingDate_View", "Дата отправки почтой - Просмотр");
            this.Permission("Gkh.Person.RequestToExam.Field.MailingDate_Edit", "Дата отправки почтой - Редактирование");

            this.Namespace("Gkh.Person.Qualification", "Квалификационные аттестаты");
            this.Permission("Gkh.Person.Qualification.Create", "Добавление");
            this.Permission("Gkh.Person.Qualification.Edit", "Редактирование");
            this.Permission("Gkh.Person.Qualification.Delete", "Удаление");

            this.Namespace("Gkh.Person.Qualification.Field", "Поля");
            this.Permission("Gkh.Person.Qualification.Field.RequestToExam_View", "Заявка на доступ к экзамену - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RecieveDate_View", "Дата получения - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RecieveDate_Edit", "Дата получения - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.QualificationDocument_View", "Документ аттестата - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.QualificationDocument_Edit", "Документ аттестата - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.QualificationNotification_View", "Уведомление лицензионной комиссии о результатах экзамена - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.QualificationNotification_Edit", "Уведомление лицензионной комиссии о результатах экзамена - Редактирование");

            this.Permission("Gkh.Person.Qualification.Field.IsFromAnotherRegion_View", "КА другого региона - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.IsFromAnotherRegion_Edit", "КА другого региона - Редактирование");
            this.Permission("Gkh.Person.Qualification.Field.RegionCode_View", "Наименование региона - Просмотр");
            this.Permission("Gkh.Person.Qualification.Field.RegionCode_Edit", "Наименование региона - Редактирование");

            this.Namespace("Gkh.Person.Qualification.Duplicate", "Дубликаты");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.Duplicate");

            this.Namespace("Gkh.Person.Qualification.Duplicate", "Переоформление");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.Renew");

            this.Namespace("Gkh.Person.Qualification.TechnicalMistake", "Информация о тех. ошибках");
            this.CRUDandViewPermissions("Gkh.Person.Qualification.TechnicalMistake");

            this.Namespace("Gkh.RequestToExamRegister", "Реестр заявок на допуск к экзамену");
            this.Permission("Gkh.RequestToExamRegister.View", "Просмотр");

            this.Namespace("Gkh.Person.PersonDisqualificationInfo", "Сведения о дисквалификации");
            this.CRUDandViewPermissions("Gkh.Person.PersonDisqualificationInfo");

            this.Namespace("Gkh.Person.PersonPlaceWork", "Место работы");
            this.CRUDandViewPermissions("Gkh.Person.PersonPlaceWork");
            #endregion Должностное лицо


            this.Namespace("Gkh.CSCalculation", "Расчет платы за ЖКУ");

            this.Namespace("Gkh.CSCalculation.Calculate", "Форма расчета");
            this.CRUDandViewPermissions("Gkh.CSCalculation.Calculate");

            this.Namespace("Gkh.CSCalculation.CSFormula", "Формулы расчета");
            this.CRUDandViewPermissions("Gkh.CSCalculation.CSFormula");


            #region Лицензия УО
            this.Namespace("Gkh.ManOrgLicense", "Лицензирование УО");

            this.Namespace<ManOrgLicense>("Gkh.ManOrgLicense.License", "Реестр лицензий");
            this.Permission("Gkh.ManOrgLicense.License.View", "Просмотр");
            this.Permission("Gkh.ManOrgLicense.License.Edit", "Редактирование");
            this.Permission("Gkh.ManOrgLicense.License.Delete", "Удаление");

            this.Namespace<ManOrgLicense>("Gkh.ManOrgLicense.License.Field", "Поля");

            this.Permission("Gkh.ManOrgLicense.License.Field.LicNum_Edit", "Номер");


            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request", "Заявки на выдачу лицензий");
            this.CRUDandViewPermissions("Gkh.ManOrgLicense.Request");

            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request.SubmittedDocs", "Предоставленные документы");
            this.CRUDandViewPermissions("Gkh.ManOrgLicense.Request.SubmittedDocs");

            this.Namespace<ManOrgLicenseRequest>("Gkh.ManOrgLicense.Request.Field", "Поля");

            this.Permission("Gkh.ManOrgLicense.Request.Field.RegisterNum_Edit", "Регистрационный номер - Редактирование");

            this.Namespace("Gkh.ManOrgLicense.Request.Reports", "Отчеты");
            this.Permission("Gkh.ManOrgLicense.Request.Reports.Print", "Печать");
            this.Namespace("Gkh.Orgs.ContragentClw.Municipality", "Муниципальные образования");
            this.Permission("Gkh.Orgs.ContragentClw.Municipality.View", "Просмотр");
            this.Permission("Gkh.Orgs.ContragentClw.Municipality.Create", "Добавить");
            this.Permission("Gkh.Orgs.ContragentClw.Municipality.Delete", "Удалить");
            #endregion
        }

        /// <summary>
        /// Справочники
        /// </summary>
        private void RegisterDictionaryPermission()
        {
            this.Namespace("Gkh.Dictionaries", "Справочники");

            this.Namespace("Gkh.Dictionaries.QualifyTestQuestions", "Вопросы квалификационного экзамена");
            this.CRUDandViewPermissions("Gkh.Dictionaries.QualifyTestQuestions");           

            this.Namespace("Gkh.Dictionaries.LicenseProvidedDoc", "Документы для выдачи лицензии");
            this.CRUDandViewPermissions("Gkh.Dictionaries.LicenseProvidedDoc");

            this.Namespace("Gkh.Dictionaries.BelayOrgKindActivity", "Виды деятельности страховой организации");
            this.CRUDandViewPermissions("Gkh.Dictionaries.BelayOrgKindActivity");

            this.Namespace("Gkh.Dictionaries.CapitalGroup", "Группы капитальности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.CapitalGroup");
            
            this.Namespace("Gkh.Dictionaries.MonitoringTypeDict", "Тип технического мониторинга");
            this.CRUDandViewPermissions("Gkh.Dictionaries.MonitoringTypeDict");
            
            this.Namespace("Gkh.Dictionaries.ConstructiveElement", "Конструктивные элементы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ConstructiveElement");

            this.Namespace("Gkh.Dictionaries.BuildingFeature", "Особые признаки строения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.BuildingFeature");

            this.Namespace("Gkh.Dictionaries.ConstructiveElementGroup", "Группы конструктивных элементов");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ConstructiveElementGroup");

            this.Namespace("Gkh.Dictionaries.FurtherUse", "Дальнейшее использование");
            this.CRUDandViewPermissions("Gkh.Dictionaries.FurtherUse");

            this.Namespace("Gkh.Dictionaries.Inspector", "Инспекторы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Inspector");

            this.Namespace("Gkh.Dictionaries.Institutions", "Учебные заведения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Institutions");

            this.Namespace("Gkh.Dictionaries.KindEquipment", "Виды оснащения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.KindEquipment");

            this.Namespace("Gkh.Dictionaries.KindRisk", "Виды рисков");
            this.CRUDandViewPermissions("Gkh.Dictionaries.KindRisk");

            this.Namespace("Gkh.Dictionaries.ProtocolMKDState", "Статусы протоколов МКД");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ProtocolMKDState");

            this.Namespace("Gkh.Dictionaries.ProtocolMKDSource", "Источники протоколов МКД");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ProtocolMKDSource");

            this.Namespace("Gkh.Dictionaries.ProtocolMKDIniciator", "Инициаторы протоколов МКД");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ProtocolMKDIniciator");

            this.Namespace("Gkh.Dictionaries.MeteringDevice", "Приборы учета");
            this.CRUDandViewPermissions("Gkh.Dictionaries.MeteringDevice");

            this.Namespace("Gkh.Dictionaries.Municipality", "Муниципальные образования");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Municipality");

            this.Namespace("Gkh.Dictionaries.MunicipalityTree", "Дерево муниципальных образований");
            this.Permission("Gkh.Dictionaries.MunicipalityTree.View", "Просмотр");
            this.Permission("Gkh.Dictionaries.MunicipalityTree.Edit", "Изменение");
            this.Permission("Gkh.Dictionaries.MunicipalityTree.UnionMoBtn", "Объединение МО");

            this.Namespace("Gkh.Dictionaries.OrganizationForm", "Организационно-правовые формы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.OrganizationForm");

            this.Namespace("Gkh.Dictionaries.Period", "Периоды кап. ремонта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Period");

            this.Namespace("Gkh.Dictionaries.Position", "Должности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Position");

            this.Namespace("Gkh.Dictionaries.ReasonInexpedient", "Основания нецелесообразности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ReasonInexpedient");

            this.Namespace("Gkh.Dictionaries.ResettlementProgramSource", "Источники по программам переселения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ResettlementProgramSource");

            this.Namespace("Gkh.Dictionaries.ResettlementProgram", "Программы переселения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ResettlementProgram");

            this.Namespace("Gkh.Dictionaries.RoofingMaterial", "Материалы кровли");
            this.CRUDandViewPermissions("Gkh.Dictionaries.RoofingMaterial");
            
            this.Namespace("Gkh.Dictionaries.VideoOverwatchType", "Виды видеонаблюдения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.VideoOverwatchType");
            
            this.Namespace("Gkh.Dictionaries.Specialty", "Специальности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Specialty");

            this.Namespace("Gkh.Dictionaries.TypeOwnership", "Формы собственности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeOwnership");

            this.Namespace("Gkh.Dictionaries.TypeProject", "Типы проектов");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeProject");

            this.Namespace("Gkh.Dictionaries.TypeService", "Типы обслуживания");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeService");

            this.Namespace("Gkh.Dictionaries.UnitMeasure", "Единицы измерения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.UnitMeasure");

            this.Namespace("Gkh.Dictionaries.WallMaterial", "Материалы стены");
            this.CRUDandViewPermissions("Gkh.Dictionaries.WallMaterial");

            this.Namespace("Gkh.Dictionaries.TypeCategoryCS", "Типы категорий МКД");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeCategoryCS");

            this.Namespace("Gkh.Dictionaries.Work", "Работы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Work");

            this.Namespace("Gkh.Dictionaries.CommunalResource", "Коммунальный ресурс");
            this.CRUDandViewPermissions("Gkh.Dictionaries.CommunalResource");

            this.Namespace("Gkh.Dictionaries.StopReason", "Причина расторжения договора");
            this.CRUDandViewPermissions("Gkh.Dictionaries.StopReason");

            this.Namespace("Gkh.Dictionaries.ManagementContractService", "Услуги по договорам управления");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ManagementContractService");

            this.Namespace("Gkh.Dictionaries.OrganizationWork", "Работы и услуги организации");
            this.CRUDandViewPermissions("Gkh.Dictionaries.OrganizationWork");

            this.Namespace("Gkh.Dictionaries.ContentRepairMkdWork", "Работы по содержанию и ремонту МКД");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ContentRepairMkdWork");

            this.Namespace("Gkh.Dictionaries.WorkKindCurrentRepair", "Вид работы текущего ремонта");
            this.CRUDandViewPermissions("Gkh.Dictionaries.WorkKindCurrentRepair");

            this.Namespace("Gkh.Dictionaries.ZonalInspection", "Зональные жилищные инспекции");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ZonalInspection");

            this.Namespace("Gkh.Dictionaries.ZonalInspection.Fields", "Поля");
            this.Permission("Gkh.Dictionaries.ZonalInspection.Fields.IndexOfGji", "Индекс");


            this.Namespace("Gkh.Dictionaries.NormativeDoc", "Нормативные документы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.NormativeDoc");

            this.Namespace("Gkh.Dictionaries.NormativeDoc.Field", "Поля");
            this.Permission("Gkh.Dictionaries.NormativeDoc.Field.FullName", "Полное наименование");
            this.Permission("Gkh.Dictionaries.NormativeDoc.Field.Validity", "Период действия");

            this.Namespace("Gkh.Dictionaries.Suggestion", "Обращения");

            this.Namespace("Gkh.Dictionaries.Suggestion.CitizenSuggestionViewCreate", "Обращения граждан: Просмотр, создание");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestionViewCreate.View", "Просмотр");

            this.Namespace<CitizenSuggestion>("Gkh.Dictionaries.Suggestion.CitizenSuggestion", "Обращения граждан: Изменение, удаление");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Suggestion.CitizenSuggestion");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.CloseExpired", "Закрытие просроченных обращений");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.ExportToGji", "Экспорт обращения в реестр ГЖИ");

            this.Namespace("Gkh.Dictionaries.CitizenSuggestion.Comment", "Вопросы");
            this.Permission("Gkh.Dictionaries.CitizenSuggestion.Comment.Create", "Создание");

            this.Namespace("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field", "Поля");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ProblemPlace", "Место проблемы");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.Rubric", "Рубрика");

            this.Namespace("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ExecutionDeadline", "Крайний срок");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ExecutionDeadline.View", "Просмотр");
            this.Permission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ExecutionDeadline.Edit", "Редактирование");

            this.Namespace("Gkh.Dictionaries.Suggestion.Rubric", "Рубрики");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Suggestion.Rubric");

            this.Namespace("Gkh.Dictionaries.Suggestion.Rubric.Field", "Поля");
            this.Permission("Gkh.Dictionaries.Suggestion.Rubric.Field.RunBP", "Запуск процесса");


            this.Namespace("Gkh.Dictionaries.ProblemPlace", "Места проблемы");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ProblemPlace");

            this.Namespace("Gkh.Dictionaries.RealEstateType", "Типы домов");
            this.Permission("Gkh.Dictionaries.RealEstateType.View", "Просмотр");            
            this.Permission("Gkh.Dictionaries.RealEstateType.View", "Просмотр");
            this.Permission("Gkh.Dictionaries.RealEstateType.Edit", "Редактирование");

            this.Namespace("Gkh.Dictionaries.Multipurpose", "Универсальные справочники");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Multipurpose");

            this.Namespace("Gkh.Dictionaries.CategoryPosts", "Категории сообщений");
            this.Permission("Gkh.Dictionaries.CategoryPosts.View", "Просмотр");
            this.Permission("Gkh.Dictionaries.CategoryPosts.Create", "Создание");
            this.Permission("Gkh.Dictionaries.CategoryPosts.Edit", "Редактирование");

            this.Namespace("Gkh.Dictionaries.BuilderDocumentType", "Документы подрядных организаций");
            this.CRUDandViewPermissions("Gkh.Dictionaries.BuilderDocumentType");

            this.Namespace("Gkh.Dictionaries.CentralHeatingStation", "ЦТП");
            this.Permission("Gkh.Dictionaries.CentralHeatingStation.View", "Просмотр");

            this.Namespace("Gkh.Dictionaries.LivingSquareCost", "Средняя стоимость квадратного метра");
            this.CRUDandViewPermissions("Gkh.Dictionaries.LivingSquareCost");

            this.Namespace("Gkh.Dictionaries.BaseHouseEmergency", "Основание признания дома аварийным");
            this.CRUDandViewPermissions("Gkh.Dictionaries.BaseHouseEmergency");

            this.Namespace("Gkh.Dictionaries.TypesHeatSource", "Типы теплоисточника или теплоносителя");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesHeatSource");

            this.Namespace("Gkh.Dictionaries.TypeInterHouseHeatingSystem", "Тип поквартирной разводки внутридомовой системы отопления");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeInterHouseHeatingSystem");

            this.Namespace("Gkh.RealityObject.Register.Lift", "Лифты");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.Lift");
            this.Namespace("Gkh.Dictionaries.TypesHeatedAppliances", "Типы отопительных приборов");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesHeatedAppliances");

            this.Namespace("Gkh.Dictionaries.NetworkAndRiserMaterials", "Материалы сети и стояков");
            this.CRUDandViewPermissions("Gkh.Dictionaries.NetworkAndRiserMaterials");

            this.Namespace("Gkh.Dictionaries.NetworkInsulationMaterials", "Материалы теплоизоляции сети");
            this.CRUDandViewPermissions("Gkh.Dictionaries.NetworkInsulationMaterials");

            this.Namespace("Gkh.Dictionaries.TypesWaterDisposalMaterial", "Типы материала водотведения");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesWaterDisposalMaterial");

            this.Namespace("Gkh.Dictionaries.FoundationMaterials", "Материалы фундамента");
            this.CRUDandViewPermissions("Gkh.Dictionaries.FoundationMaterials");

            this.Namespace("Gkh.Dictionaries.TypesWindowMaterials", "Типы материалов окон");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesWindowMaterials");

            this.Namespace("Gkh.Dictionaries.TypesBearingPartRoof", "Виды несущей части крыши");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesBearingPartRoof");

            this.Namespace("Gkh.Dictionaries.WarmingLayersAttics", "Утепляющие слои чердачных перекрытий");
            this.CRUDandViewPermissions("Gkh.Dictionaries.WarmingLayersAttics");

            this.Namespace("Gkh.Dictionaries.MaterialRoof", "Материал кровли");
            this.CRUDandViewPermissions("Gkh.Dictionaries.MaterialRoof");

            this.Namespace("Gkh.Dictionaries.FacadeDecorationMaterials", "Материалы отделки фасада");
            this.CRUDandViewPermissions("Gkh.Dictionaries.FacadeDecorationMaterials");

            this.Namespace("Gkh.Dictionaries.TypesExternalFacadeInsulation", "Типы наружного утепления фасада");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesExternalFacadeInsulation");

            this.Namespace("Gkh.Dictionaries.TypesExteriorWalls", "Типы наружных стен");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypesExteriorWalls");

            this.Namespace("Gkh.Dictionaries.WaterDispensers", "Водоразборные устройства");
            this.CRUDandViewPermissions("Gkh.Dictionaries.WaterDispensers");

            this.Namespace("Gkh.Dictionaries.CategoryConsumersEqualPopulation", "Категория потребителей, приравненных к населению");
            this.CRUDandViewPermissions("Gkh.Dictionaries.CategoryConsumersEqualPopulation");

            this.Namespace("Gkh.Dictionaries.TypeInformationNpa", "Типы информации в НПА");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeInformationNpa");
            this.Namespace("Gkh.Dictionaries.EnergyEfficiencyClasses", "Классы энергетической эффективности");
            this.CRUDandViewPermissions("Gkh.Dictionaries.EnergyEfficiencyClasses");

            this.Namespace("Gkh.Dictionaries.TypeNpa", "Типы НПА");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeNpa");

            this.Namespace("Gkh.Dictionaries.TypeNormativeAct", "Виды нормативных актов");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeNormativeAct");

            this.Namespace("Gkh.Dictionaries.ContragentRole", "Роли контрагента");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ContragentRole");

            this.Namespace("Gkh.Dictionaries.RiskCategory", "Категории риска");
            this.DictionaryPermissions("Gkh.Dictionaries.RiskCategory");

            this.Namespace("Gkh.Dictionaries.TypeFloor", "Тип перекрытия");
            this.CRUDandViewPermissions("Gkh.Dictionaries.TypeFloor");

            this.Namespace("Gkh.Dictionaries.IdentityDocumentType", "Типы документов");
            this.CRUDandViewPermissions("Gkh.Dictionaries.IdentityDocumentType");
        }
    }
}