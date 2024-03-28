namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Жилой дом"</summary>
    public class RealityObjectMap : BaseImportableEntityMap<RealityObject>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RealityObjectMap() : 
                base("Жилой дом", "GKH_REALITY_OBJECT")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE").NotNull();
            this.Property(x => x.CodeErc, "Код ЕРЦ").Column("CODE_ERC").Length(250);
            this.Property(x => x.HavingBasement, "Наличие подвала (СКРЫЛИ)").Column("HAVING_BASEMENT").NotNull();
            this.Property(x => x.HeatingSystem, "Система отопления").Column("HEATING_SYSTEM").NotNull();
            this.Property(x => x.TypeHouse, "Вид дома").Column("TYPE_HOUSE").NotNull();
            this.Property(x => x.TypeRoof, "Тип кровли").Column("TYPE_ROOF").NotNull();
            this.Property(x => x.Address, "Адрес").Column("ADDRESS").Length(1000);
            this.Property(x => x.AreaBasement, "Площадь подвала (кв.м.) (СКРЫЛИ)").Column("AREA_BASEMENT");
            this.Property(x => x.AreaLiving, "Площадь жилая, всего (кв. м.)").Column("AREA_LIVING");
            this.Property(x => x.AreaLivingNotLivingMkd, "Общая площадь жилых и нежилых помещений в МКД (кв.м.)").Column("AREA_LIV_NOT_LIV_MKD");
            this.Property(x => x.AreaLivingOwned, "Площадь жилая находящаяся в собственности граждан, всего (кв. м.)").Column("AREA_LIVING_OWNED");
            this.Property(x => x.AreaMkd, "Общая площадь МКД (кв.м.)").Column("AREA_MKD");
            this.Property(x => x.AreaCommonUsage, "Площадь помещений общего пользования").Column("AREA_COM_USAGE");
            this.Property(x => x.DateDemolition, "Дата сноса").Column("DATE_DEMOLITION");
            this.Property(x => x.FederalNum, "Федеральный номер").Column("FEDERAL_NUMBER").Length(300);
            this.Property(x => x.Floors, "Этажность").Column("FLOORS");
            this.Property(x => x.MaximumFloors, "Максимальная этажность").Column("MAXIMUM_FLOORS");
            this.Property(x => x.IsInsuredObject, "Объект застрахован").Column("IS_INSURED_OBJECT");
            this.Property(x => x.Iscluttered, "Объект захламлен").Column("IS_CLUTTERED");
            this.Property(x => x.HasVidecam, "Наличие камеры видеонаблюдения").Column("HAS_VIDECAM");
            this.Property(x => x.Notation, "Примечание").Column("NOTATION").Length(1000);
            this.Property(x => x.NumberApartments, "Количество квартир").Column("NUMBER_APARTMENTS");
            this.Property(x => x.NumberGisGkhMatchedApartments, "Количество сопоставленных с ГИС ЖКХ жилых помещений").Column("NUMBER_GIS_GKH_MATCHED_APARTMENTS");
            this.Property(x => x.NumberGisGkhMatchedNonResidental, "Количество сопоставленных с ГИС ЖКХ нежилых помещений").Column("NUMBER_GIS_GKH_MATCHED_NON_RESIDENTAL");
            this.Property(x => x.GisGkhMatchDate, "Дата сопоставления с ГИС ЖКХ").Column("GIS_GKH_MATCH_DATE");
            this.Property(x => x.GisGkhAccMatchDate, "Дата сопоставления ЛС с ГИС ЖКХ").Column("GIS_GKH_ACC_MATCH_DATE");
            this.Property(x => x.NumberEntrances, "Количество входов (подъездов)").Column("NUMBER_ENTRANCES");
            this.Property(x => x.NumberLifts, "Количество лифтов").Column("NUMBER_LIFTS");
            this.Property(x => x.NumberLiving, "Количество проживающих").Column("NUMBER_LIVING");
            this.Property(x => x.PhysicalWear, "Физический износ (%)").Column("PHYSICAL_WEAR");
            this.Property(x => x.SeriesHome, "Серия дома (СКРЫЛИ)").Column("SERIES_HOME").Length(250);
            this.Property(x => x.DateCommissioning, "Дата сдачи в эксплуатацию").Column("DATE_COMMISSIONING");
            this.Property(x => x.DateCommissioningLastSection, "Дата сдачи в эксплуатацию последней секции дома").Column("DATE_COMMISSIONING_LAST_SECTION");
            this.Property(x => x.DateLastOverhaul, "Дата последнего кап. ремонта (СКРЫЛИ)").Column("DATE_LAST_RENOVATION");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(1000);
            this.Property(x => x.GkhCode, "Внутрнний Код дома").Column("GKH_CODE").Length(100);
            this.Property(x => x.WebCameraUrl, "Адрес веб камеры").Column("WEB_CAMERA_URL").Length(1000);
            this.Property(x => x.DateTechInspection, "дата проведения тех. обследования").Column("DATE_TECH_INS");
            this.Property(x => x.ResidentsEvicted, "Жильцы выселены").Column("RESIDENTS_EVICTED").NotNull();
            this.Property(x => x.IsBuildSocialMortgage, "Дом построен по соц.ипотеке(да/нет)").Column("IS_BUILD_SOC_MORTGAGE").NotNull();
            this.Property(x => x.TotalBuildingVolume, "Общий строительный объем (куб.м.)").Column("TOTAL_BUILD_VOL");
            this.Property(x => x.AreaOwned, "Площадь частной собственности (кв.м.)").Column("AREA_OWNED");
            this.Property(x => x.AreaMunicipalOwned, "Площадь муниципальной собственности (кв.м.)").Column("AREA_MUNICIPAL_OWNED");
            this.Property(x => x.AreaGovernmentOwned, "Площадь государственной собственности (кв.м.)").Column("AREA_GOVERNMENT_OWNED");
			this.Property(x => x.AreaFederalOwned, "Площадь федеральной собственности (кв.м.)").Column("AREA_FEDERAL_OWNED");
			this.Property(x => x.AreaCommercialOwned, "Площадь коммерческой собственности (кв.м.)").Column("AREA_COMMERCIAL_OWNED");
			this.Property(x => x.AreaNotLivingFunctional, "В т.ч. нежилых, функционального назначения (кв.м.)").Column("AREA_NOT_LIV_FUNCT");
            this.Property(x => x.CadastreNumber, "Кадастровый номер земельного участка").Column("CADASTRE_NUMBER");
            this.Property(x => x.CadastralHouseNumber, "Кадастровый номер дома").Column("CADASTRAL_HOUSE_NUMBER");
            this.Property(x => x.NecessaryConductCr, "Требовалось проведение КР на дату приватизации первого жилого помещения").Column("NECESSARY_CONDUCT_CR").NotNull();
            this.Property(x => x.FloorHeight, "Высота этажа").Column("FLOOR_HEIGHT");
            this.Property(x => x.PercentDebt, "Собираемость платежей на кап.ремонт, %").Column("PERCENT_DEBT");
            this.Property(x => x.PrivatizationDateFirstApartment, "Дата приватизации первого жилого помещения").Column("PRIV_DATE_FAPARTMENT");
            this.Property(x => x.BuildYear, "Год постройки").Column("BUILD_YEAR");
            this.Property(x => x.MethodFormFundCr, "Способ формирования фонда КР").Column("METHOD_FORM_FUND").NotNull();
            this.Property(x => x.HasJudgmentCommonProp, "Наличие судебного решения по проведению Кр общего имущества").Column("JUDGMENT_COMMON_PROP").NotNull();
            this.Property(x => x.IsRepairInadvisable, "Ремонт нецелесообразен").Column("IS_REPAIR_INADVISABLE");
            this.Property(x => x.HasPrivatizedFlats, "В доме присутствуют приватизированные квартиры").Column("HAS_PRIV_FLATS");
            this.Property(x => x.IsNotInvolvedCr, "Дом не участвует в КР (в программе кап. ремонта)").Column("IS_NOT_INVOLVED_CR");
            this.Property(x => x.District, "Административный округ (для МСК)").Column("DISTRICT").Length(300);
            this.Property(x => x.UnomCode, "Код справочника \"UNOM\" (для МСК)").Column("UNOM_CODE").Length(100);
            this.Property(x => x.VtscpCode, "Код дома для ВЦКП").Column("VTSCP_CODE").Length(50);
            this.Property(x => x.Points, "Баллы").Column("POINTS");
            this.Property(x => x.ProjectDocs, "Наличие проектной дркументации").Column("PROJECT_DOCS").NotNull();
            this.Property(x => x.EnergyPassport, "Наличие энергетического паспорта").Column("ENERGY_PASSPORT").NotNull();
            this.Property(x => x.ConfirmWorkDocs, "Документы по гос.кадастровому учету зем.участка").Column("CONFIRM_WORK_DOCS").NotNull();
            this.Property(x => x.ManOrgs, "Наименования организаций, управляющих домом").Column("MAN_ORGS");
            this.Property(x => x.TypesContract, "Типы договоров управления").Column("TYPES_CONTRACT");
            this.Property(x => x.AreaCleaning, "Уборочная площадь").Column("AREA_CLEANING");
            this.Property(x => x.AddressCode, "Код адреса (используется в импорте льгоников)").Column("ADDRESS_CODE").Length(50);
            this.Property(x => x.AccountFormationVariant, "способ формирования фонда кр на текущий момент (обновляется через FillAccountForm" +
                    "ationVariantAction)").Column("ACC_FORM_VARIANT");
            this.Property(x => x.NumberNonResidentialPremises, "Количество нежилых помещений").Column("NONRES_PREMISES");
            this.Property(x => x.AreaNotLivingPremises, "Площадь нежилых помещений").Column("AREA_NL_PREMISES");
            this.Property(x => x.HouseGuid, "Гуид дома в ФИАСе").Column("HOUSEGUID");
            this.Property(x => x.ObjectConstruction, "Объект строительства").Column("OBJECT_CONSTRUCTIJN");
            this.Property(x => x.BuiltOnResettlementProgram, "Построен по программе переселения").Column("BUILT_ON_RESETTLEMENT_PROGRAM");
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Reference(x => x.TypeOwnership, "Форма собственности").Column("TYPE_OWNERSHIP_ID");
            this.Reference(x => x.CapitalGroup, "Группа капитальности").Column("CAPITAL_GROUP_ID");
            this.Reference(x => x.RoofingMaterial, "Материал кровли").Column("ROOFING_MATERIAL_ID");
            this.Reference(x => x.WallMaterial, "Материал стен").Column("WALL_MATERIAL_ID");
            this.Reference(x => x.TypeProject, "Серия, тип проекта").Column("TYPE_PROJECT_ID");
            this.Reference(x => x.FiasAddress, "Адрес ФИАС").Column("FIAS_ADDRESS_ID");
            this.Reference(x => x.RealEstateType, "Тип дома").Column("REAL_EST_TYPE_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.MoSettlement, "МО - поселение").Column("STL_MUNICIPALITY_ID").Fetch();
            this.Property(x => x.IsCulturalHeritage, "Памятник архитектуры").Column("IS_CULTURAL_HERITAGE");
            this.Property(x => x.IsInvolvedCrTo2, "Участвует в программе КР ТО №2").Column("IS_INVOLVED_CR_TO_2");
            this.Property(x => x.LatestTechnicalMonitoring, "Дата последнего технического мониторинга").Column("LATEST_TECH_MONITORING");
            this.Property(x => x.IsNotInvolvedCrReason, "Причина по которой проставился признак Дом не участвует в программе КР").Column("IS_NOT_INVOLVED_CR_REASON");
            this.Property(x => x.UnpublishDate, "Дата исключения из ДПКР").Column("UNPUBLISH_DATE");
            this.Property(x => x.CulturalHeritageAssignmentDate, "Дата присвоения для поля \"Памятник архитектуры\"").Column("CULT_HER_ASSIGN_DATE");
            this.Reference(x => x.CentralHeatingStation, "Наименование ЦТП").Column("CTP_NAME_ID");
            this.Property(x => x.NumberInCtp, "Порядковый номер объекта в ЦТП").Column("NUMBER_IN_CTP");
            this.Property(x => x.PriorityCtp, "Приоритет вывода из эксплуатации ЦТП").Column("PRIORITY_CTP");
            this.Property(x => x.MonumentDocumentNumber, "Номер документа").Column("MONUMENT_DOCUMENT_NUMBER");
            this.Reference(x => x.MonumentFile, "Файл").Column("MONUMENT_FILE");
            this.Property(x => x.MonumentDepartmentName, 
                "Наименование органа, выдавшего документ о признании дома памятником архитектуры")
                .Column("MONUMENT_DEPARTMENT_NAME");
            this.Reference(x => x.TechPassportFile, "Электронный паспорт дома").Column("TECH_PASSPORT_FILE_ID");
            Property(x => x.IsSubProgram, "Дом в подпрограмме").Column("IS_SUB_PROGRAM").NotNull();
            Property(x => x.HasCharges185FZ, "Имеет начисления по 185 ФЗ").Column("HAS_CHARGES_185FZ");
            Property(x => x.IncludeToSubProgram, "Включить в подпрограмму").Column("INCLUDE_TO_SUB_PROGRAM").NotNull();
            Reference(x => x.ASSberbankClient, "Настройки для выгрузки в Клиент-Сбербанк").Column("ASSBERBANKCLIENT_ID");
            this.Property(x => x.GisGkhGUID, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID");
            this.Property(x => x.ExportedToPortal, "Выводить на портал").Column("EXPORT_TO_PORTAL");
            this.Reference(x => x.FileInfo, "Выписка из БТИ").Column("FILE_INFO_ID");
            Property(x => x.Latitude, "Широта").Column("LATITUDE");
            Property(x => x.Longitude, "Долгота").Column("LONGITUDE");
            //this.Property(x => x.TorId, "Идентификатор в ТОР").Column("TOR_ID");
            this.Reference(x => x.Outdoor, "Двор").Column("OUTDOOR_ID");
            this.Property(x => x.InnManOrgs, "Инн управляющих компаний").Column("INN_MAN_ORGS");
            this.Property(x => x.StartControlDate, "Дата начала управления").Column("START_CONTROL_DATE");
            this.Property(x => x.IsIncludedRegisterCHO, "Включён в реестр ОКН").Column("IS_INCLUDED_REG_CHO");
            this.Property(x => x.IsIncludedListIdentifiedCHO, "Включён в перечень выявленных ОКН").Column("IS_INCLUDED_LIST_IDENT_CHO");
            this.Property(x => x.IsDeterminedSubjectProtectionCHO, "Предмет охраны ОКН определен").Column("IS_DETERMINED_SUB_PROTECT_CHO");
        }
    }
}
