namespace Bars.Gkh.Permissions
{
    using DomainService;

    class GkhFieldRequirementMap : FieldRequirementMap
    {
        public GkhFieldRequirementMap()
        {
            this.Namespace("Gkh", "Модуль ЖКХ");
            this.Namespace("Gkh.RealityObject", "Жилые дома");
            this.Namespace("Gkh.RealityObject.Field", "Поля");

            this.Requirement("Gkh.RealityObject.Field.Address_Rqrd", "Адрес");
            this.Requirement("Gkh.RealityObject.Field.HauseGuid_Rqrd", "Код ФИАС");
            this.Requirement("Gkh.RealityObject.Field.TypeHouse_Rqrd", "Тип дома");
            this.Requirement("Gkh.RealityObject.Field.ConditionHouse_Rqrd", "Состояние дома");
            this.Requirement("Gkh.RealityObject.Field.BuildYear_Rqrd", "Год постройки");
            this.Requirement("Gkh.RealityObject.Field.DateDemolition_Rqrd", "Дата сноса");
            this.Requirement("Gkh.RealityObject.Field.DateCommissioning_Rqrd", "Дата сдачи в эксплуатацию");
            this.Requirement("Gkh.RealityObject.Field.DateCommissioningLastSection_Rqrd", "Дата сдачи в эксплуатацию последней секции дома");
            this.Requirement("Gkh.RealityObject.Field.DateTechInspection_Rqrd", "Дата тех. обследования");
            this.Requirement("Gkh.RealityObject.Field.PrivatizationDateFirstApartment_Rqrd", "Наличие и дата приватизации первого жилого помещения");
            this.Requirement("Gkh.RealityObject.Field.MethodFormFundCr_Rqrd", "Предполагаемый способ формирования фонда КР");

            this.Requirement("Gkh.RealityObject.Field.FederalNum_Rqrd", "Федеральный номер");
            this.Requirement("Gkh.RealityObject.Field.GkhCode_Rqrd", "Код дома");
            this.Requirement("Gkh.RealityObject.Field.TypeProject_Rqrd", "Серия, тип проекта");
            this.Requirement("Gkh.RealityObject.Field.CapitalGroup_Rqrd", "Группа капитальности");
            this.Requirement("Gkh.RealityObject.Field.WebCameraUrl_Rqrd", "Адрес веб-камеры");

            this.Requirement("Gkh.RealityObject.Field.TypeOwnership_Rqrd", "Форма собственности");
            this.Requirement("Gkh.RealityObject.Field.PhysicalWear_Rqrd", "Физический износ");
            this.Requirement("Gkh.RealityObject.Field.BuildSocialMortgage_Rqrd", "Построен по соц. ипотеке");
            this.Requirement("Gkh.RealityObject.Field.CodeErc_Rqrd", "Код ЕРЦ");

            this.Requirement("Gkh.RealityObject.Field.CadastreNumber_Rqrd", "Кадастровый номер земельного участка");
            this.Requirement("Gkh.RealityObject.Field.CadastralHouseNumber_Rqrd", "Кадастровый номер дома");
            this.Requirement("Gkh.RealityObject.Field.TotalBuildingVolume_Rqrd", "Общий строительный объем");
            this.Requirement("Gkh.RealityObject.Field.AreaMkd_Rqrd", "Общая площадь МКД");
            this.Requirement("Gkh.RealityObject.Field.AreaOwned_Rqrd", "Площадь частной собственности");
            this.Requirement("Gkh.RealityObject.Field.AreaMunicipalOwned_Rqrd", "Площадь муниципальной собственности");
            this.Requirement("Gkh.RealityObject.Field.AreaGovernmentOwned_Rqrd", "Площадь государственной собственности");
            this.Requirement("Gkh.RealityObject.Field.AreaLivingNotLivingMkd_Rqrd", "Общая площадь жилых и нежилых помещений");
            this.Requirement("Gkh.RealityObject.Field.AreaLiving_Rqrd", "В т.ч. жилых всего");
            this.Requirement("Gkh.RealityObject.Field.AreaLivingOwned_Rqrd", "В т.ч. жилых, находящихся в собственности граждан");
            this.Requirement("Gkh.RealityObject.Field.AreaNotLivingFunctional_Rqrd", "Общая площадь помещений, входящих в состав общего имущества");
            this.Requirement("Gkh.RealityObject.Field.NecessaryConductCr_Rqrd", "Требовалось проведение КР на дату приватизации первого жилого помещения");
            this.Requirement("Gkh.RealityObject.Field.NumberNonResidentialPremises_Rqrd", "Количество нежилых помещений");

            this.Requirement("Gkh.RealityObject.Field.MaximumFloors_Rqrd", "Максимальная этажность");
            this.Requirement("Gkh.RealityObject.Field.Floors_Rqrd", "Минимальная этажность");
            this.Requirement("Gkh.RealityObject.Field.FloorHeight_Rqrd", "Высота этажа");
            this.Requirement("Gkh.RealityObject.Field.NumberEntrances_Rqrd", "Количество подъездов");
            this.Requirement("Gkh.RealityObject.Field.NumberApartments_Rqrd", "Количество квартир");
            this.Requirement("Gkh.RealityObject.Field.NumberLiving_Rqrd", "Количество проживающих");
            this.Requirement("Gkh.RealityObject.Field.NumberLifts_Rqrd", "Количество лифтов");
            this.Requirement("Gkh.RealityObject.Field.RoofingMaterial_Rqrd", "Материал кровли");
            this.Requirement("Gkh.RealityObject.Field.WallMaterial_Rqrd", "Материал стен");
            this.Requirement("Gkh.RealityObject.Field.TypeRoof_Rqrd", "Тип кровли");
            this.Requirement("Gkh.RealityObject.Field.PercentDebt_Rqrd", "Собираемость платежей");
            this.Requirement("Gkh.RealityObject.Field.HeatingSystem_Rqrd", "Система отопления");
            this.Requirement("Gkh.RealityObject.Field.HasJudgmentCommonProp_Rqrd", "Наличие судебного решения по проведению КР общего имущества");
            this.Requirement("Gkh.RealityObject.Field.LatestTechnicalMonitoring_Rqrd", "Дата последнего технического мониторинга");

            this.Requirement("Gkh.RealityObject.Field.MonumentDocumentNumber_Rqrd", "Номер документа");
            this.Requirement("Gkh.RealityObject.Field.MonumentFile_Rqrd", "Файл");
            this.Requirement("Gkh.RealityObject.Field.MonumentDepartmentName_Rqrd",
                "Наименование органа, выдавшего документ о признании дома памятником архитектуры");

            this.Requirement("Gkh.RealityObject.Field.IsIncludedRegisterCHO_Rqrd", "Включён в реестр ОКН");
            this.Requirement("Gkh.RealityObject.Field.IsIncludedListIdentifiedCHO_Rqrd", "Включён в перечень выявленных ОКН");
            this.Requirement("Gkh.RealityObject.Field.IsDeterminedSubjectProtectionCHO_Rqrd", "Предмет охраны ОКН определен");

            this.Requirement("Gkh.RealityObject.Field.DateLastOverhaul_Rqrd", "Дата последнего кап. ремонта");
            this.Requirement("Gkh.RealityObject.Field.AreaNotLivingPremises_Rqrd", "в т.ч. нежилых всего");
            this.Requirement("Gkh.RealityObject.Field.AreaCommonUsage_Rqrd", "Площадь помещений общего пользования");

            this.Namespace("Gkh.RealityObject.Image", "Фото-архив");
            this.Namespace("Gkh.RealityObject.Image.Fields", "Поля");

            this.Requirement("Gkh.RealityObject.Image.Fields.File_Rqrd", "Файл");

            this.Namespace("Gkh.RealityObject.Block", "Сведения о блоках");
            this.Namespace("Gkh.RealityObject.Block.Fields", "Поля");
            this.Requirement("Gkh.RealityObject.Block.Fields.Number_Rqrd", "Номер блока");
            this.Requirement("Gkh.RealityObject.Block.Fields.AreaLiving_Rqrd", "Жилая площадь");
            this.Requirement("Gkh.RealityObject.Block.Fields.AreaTotal_Rqrd", "Общая площадь");
            this.Requirement("Gkh.RealityObject.Block.Fields.CadastralNumber_Rqrd", "Кадастровый номер");

            this.Namespace("Gkh.RealityObject.MeteringDevicesChecks", "Проверки приборов учёта");
            this.Namespace("Gkh.RealityObject.MeteringDevicesChecks.Fields", "Поля");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.MeteringDevice_Rqrd", "Прибор учёта");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.ControlReading_Rqrd", " Контрольное показание");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.RemovalControlReadingDate_Rqrd", "Дата снятия контрольного показания");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.StartDateCheck_Rqrd", "Дата начала проверки");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.StartValue_Rqrd", "начение показаний прибора учета на момент начала проверки");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.EndDateCheck_Rqrd", "Дата окончания проверки");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.EndValue_Rqrd", "Значение показаний на момент окончания проверки");
            this.Requirement("Gkh.RealityObject.MeteringDevicesChecks.Fields.IntervalVerification_Rqrd", "Межпроверочный интервал (лет)");
           

            this.Namespace("Gkh.Orgs", "Участники процесса");
            this.Namespace("Gkh.Orgs.Contragent", "Контрагенты");
            this.Namespace("Gkh.Orgs.Contragent.Field", "Поля");

            this.Requirement("Gkh.Orgs.Contragent.Field.Inn_Rqrd", "ИНН");
            this.Requirement("Gkh.Orgs.Contragent.Field.Kpp_Rqrd", "КПП");
            this.Requirement("Gkh.Orgs.Contragent.Field.FiasJuridicalAddress_Rqrd", "Юридический адрес");
            this.Requirement("Gkh.Orgs.Contragent.Field.FiasFactAddress_Rqrd", "Фактический адрес");
            this.Requirement("Gkh.Orgs.Contragent.Field.Ogrn_Rqrd", "ОГРН");
            this.Requirement("Gkh.Orgs.Contragent.Field.Oktmo_Rqrd", "ОКТМО");
            this.Requirement("Gkh.Orgs.Contragent.Field.FrguRegNumber_Rqrd", "Реестровый номер функции в ФРГУ");
            this.Requirement("Gkh.Orgs.Contragent.Field.FrguOrgNumber_Rqrd", "Номер организации в ФРГУ");
            this.Requirement("Gkh.Orgs.Contragent.Field.FrguServiceNumber_Rqrd", "Номер услуги в ФРГУ");

            this.Namespace("GkhCr.ObjectCR", "Объект КР");
            this.Namespace("GkhCr.ObjectCR.Protocol", "Протоколы");
            this.Namespace("GkhCr.ObjectCR.Protocol.Field", "Поля");

            this.Requirement("GkhCr.ObjectCR.Protocol.Field.OwnerName", "Собственник, участвующий в приемке работ");
            this.Requirement("GkhCr.ObjectCR.Protocol.Field.File", "Файл");


            // Управляющие организации
            this.Namespace("Gkh.Orgs.Contragent.Manorg", "Управляющие организации");
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging", "Управление домами");

            // УК
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK", "Тип управления = УК, Прочее");
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field", "Поля");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.StartDatePaymentPeriod_Rqrd", "Дата начала периода");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.EndDatePaymentPeriod_Rqrd", "Дата окончания периода");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.DateLicenceRegister_Rqrd", "Дата внесения в реестр лицензий");

            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolFile_Rqrd", "Сведения о плате - Протокол");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolDescription_Rqrd", "Сведения о плате - Описание");

            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.SetPaymentsFoundation_Rqrd", "Сведения о плате - Основание установления размера платы за содержание жилого помещения");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.RevocationReason_Rqrd", "Сведения о плате - Причина аннулирования");

            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract", "Договор управления");
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation", "Сведения о договоре");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolNumber_Rqrd", "Номер протокола");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolDate_Rqrd", "Дата протокола - от");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.ProtocolFileInfo_Rqrd", "Протокол");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.OwnersSignedContractFile_Rqrd", "Реестр собственников, подписавших договор");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.PlannedEndDate_Rqrd", "Плановая дата окончания");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ContractInformation.FileInfo_Rqrd", "Договор управления");

            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation", "Сведения об услугах");
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService", "Дополнительные услуги");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_Service_Rqrd", "Дополнительная услуга организации");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_StartDate_Rqrd", "Дата начала предоставления");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.AdditionService.Addition_EndDate_Rqrd", "Дата окончания предоставления");

            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService", "Коммунальные услуги");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_Service_Rqrd", "Коммунальная услуга организации");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_StartDate_Rqrd", "Дата начала предоставления");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.ServiceInformation.CommunalService.Communal_EndDate_Rqrd", "Дата окончания предоставления");

            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation", "Сведения о сроках");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.InputMeteringDeviceValuesBeginDate_Rqrd", "День месяца начала ввода показаний по ПУ (от 1 до 30)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.InputMeteringDeviceValuesEndDate_Rqrd", "День месяца окончания ввода показаний по ПУ (от 2 до 31)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.DrawingPaymentDocumentDate_Rqrd", "День месяца выставления платежных документов (от 1 до 30)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.DateInformation.PaymentServicePeriodDate_Rqrd", "День месяца внесения платы за ЖКУ (от 1 до 30)");

            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation", "Сведения о плате");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.PaymentAmount_Rqrd", "Размер платы (цена) за услуги, работы по управлению домом");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.StartDatePaymentPeriod_Rqrd", "Дата начала");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.ManOrgContract.PaymentInformation.EndDatePaymentPeriod_Rqrd", "Дата окончания");

            // ТСЖ/ЖСК
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk", "Тип управления = ТСЖ, ЖСК");
            this.Namespace("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field", "Поля");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.StartDatePaymentPeriod_Rqrd", "Дата начала периода");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.EndDatePaymentPeriod_Rqrd", "Дата окончания периода");

            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyReqiredPaymentAmount_Rqrd", "Размер обязательных платежей/взносов");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolFile_Rqrd", "Протокол (члены товарищества)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolDescription_Rqrd", "Описание протокола (члены товарищества)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ReqiredPaymentAmount_Rqrd", "Размер платежей/взносов");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolFile_Rqrd", "Протокол");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolDescription_Rqrd", "Описание протокола");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolNumber_Rqrd", "Номер протокола");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolFileInfo_Rqrd", "Протокол");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PlannedEndDate_Rqrd", "Плановая дата окончания");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.FileInfo_Rqrd", "Файл");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesBeginDate_Rqrd", "День месяца начала ввода показаний по ПУ (от 1 до 30)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesEndDate_Rqrd", "День месяца окончания ввода показаний по ПУ (от 2 до 31)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.DrawingPaymentDocumentDate_Rqrd", "День месяца выставления платежных документов (от 1 до 30)");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentServicePeriodDate_Rqrd", "День месяца внесения платы за ЖКУ (от 1 до 30)");

            //дворы
            this.Namespace("Gkh.RealityObjectOutdoor", "Дворы");
            this.Namespace("Gkh.RealityObjectOutdoor.Field", "Поля");
            this.Requirement("Gkh.RealityObjectOutdoor.Field.Area", "Площадь двора (кв.м)");
            this.Requirement("Gkh.RealityObjectOutdoor.Field.AsphaltArea", "Площадь асфальта (кв.м.)");
            this.Requirement("Gkh.RealityObjectOutdoor.Field.Description", "Примечание");
            this.Requirement("Gkh.RealityObjectOutdoor.Field.RepairPlanYear", "Плановый год ремонта");
            
            //Раскрытие информации
            this.Namespace("Gkh.DisclosureInfo", "Раскрытие информации");
            this.Namespace("Gkh.DisclosureInfo.RealityObject", "Объекты управления");
            this.Namespace("Gkh.DisclosureInfo.RealityObject.Documents", "Документы");
            this.Namespace("Gkh.DisclosureInfo.RealityObject.Documents.Fields", "Поля");
            this.Requirement("Gkh.DisclosureInfo.RealityObject.Documents.Fields.Year", "Год");

            this.Namespace("Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo", "Сведения об использовании нежилых помещений");
            this.Namespace("Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo.Fields", "Поля");
            this.Requirement("Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo.Fields.StartDate", "Дата начала");
            this.Requirement("Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo.Fields.EndDate", "Дата окончания");
        }
    }
}
