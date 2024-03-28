﻿namespace Bars.Gkh.Entities
{
    using System;
    using System.Text;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Base;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Жилой дом
    /// </summary>
    public class RealityObject : BaseGkhEntity, IStatefulEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Площадь жилая, всего (кв. м.)
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Площадь жилая находящаяся в собственности граждан, всего (кв. м.)
        /// </summary>
        public virtual decimal? AreaLivingOwned { get; set; }

        /// <summary>
        /// Площадь частной собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaOwned { get; set; }

        /// <summary>
        /// Площадь муниципальной собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaMunicipalOwned { get; set; }

        /// <summary>
        /// Площадь государственной собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaGovernmentOwned { get; set; }

        /// <summary>
        /// Площадь федеральной собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaFederalOwned { get; set; }

        /// <summary>
        /// Площадь коммерческой собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaCommercialOwned { get; set; }

        /// <summary>
        /// В т.ч. нежилых, функционального назначения (кв.м.)
        /// </summary>
        public virtual decimal? AreaNotLivingFunctional { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений в МКД (кв.м.)
        /// </summary>
        public virtual decimal? AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// Общая площадь МКД (кв.м.)
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Площадь подвала (кв.м.) (СКРЫЛИ)
        /// </summary>
        public virtual decimal? AreaBasement { get; set; }

        /// <summary>
        /// Площадь помещений общего пользования
        /// </summary>
        public virtual decimal? AreaCommonUsage { get; set; }

        /// <summary>
        /// Уборочная площадь
        /// </summary>
        public virtual decimal? AreaCleaning { get; set; }

        /// <summary>
        /// Дата последнего кап. ремонта (СКРЫЛИ)
        /// </summary>
        public virtual DateTime? DateLastOverhaul { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        public virtual DateTime? DateCommissioning { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию последней секции дома
        /// </summary>
        public virtual DateTime? DateCommissioningLastSection { get; set; }

        /// <summary>
        /// Группа капитальности
        /// </summary>
        public virtual CapitalGroup CapitalGroup { get; set; }

        /// <summary>
        /// Код ЕРЦ
        /// </summary>
        public virtual string CodeErc { get; set; }

        /// <summary>
        /// Дата сноса
        /// </summary>
        public virtual DateTime? DateDemolition { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public virtual int? MaximumFloors { get; set; }

        /// <summary>
        /// Материал кровли
        /// </summary>
        public virtual RoofingMaterial RoofingMaterial { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public virtual WallMaterial WallMaterial { get; set; }

        /// <summary>
        /// Наличие подвала (СКРЫЛИ)
        /// </summary>
        public virtual YesNoNotSet HavingBasement { get; set; }

        /// <summary>
        /// Объект застрахован
        /// </summary>
        public virtual bool IsInsuredObject { get; set; }

        /// <summary>
        /// Наличие камеры видеонаблюдения
        /// </summary>
        public virtual bool HasVidecam { get; set; }

        /// <summary>
        /// Объект застрахован
        /// </summary>
        public virtual bool Iscluttered { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Notation { get; set; }

        /// <summary>
        /// Серия дома (СКРЫЛИ)
        /// </summary>
        public virtual string SeriesHome { get; set; }

        /// <summary>
        /// Серия, тип проекта
        /// </summary>
        public virtual TypeProject TypeProject { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        [JsonIgnore]
        public virtual bool IsEmergency
        {
            get => this.ConditionHouse == ConditionHouse.Emergency; 
            set => this.ConditionHouse = value ? ConditionHouse.Emergency : this.ConditionHouse;
        }

        /// <summary>
        /// Вид дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Тип кровли
        /// </summary>
        public virtual TypeRoof TypeRoof { get; set; }

        /// <summary>
        /// Федеральный номер
        /// </summary>
        public virtual string FederalNum { get; set; }

        /// <summary>
        /// Физический износ (%)
        /// </summary>
        public virtual decimal? PhysicalWear { get; set; }

        /// <summary>
        /// Форма собственности
        /// </summary>
        public virtual TypeOwnership TypeOwnership { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        public virtual int? Floors { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int? NumberApartments { get; set; }

        /// <summary>
        /// Количество сопоставленных с ГИС ЖКХ жилых помещений
        /// </summary>
        public virtual int? NumberGisGkhMatchedApartments { get; set; }

        /// <summary>
        /// Количество сопоставленных с ГИС ЖКХ нежилых помещений
        /// </summary>
        public virtual int? NumberGisGkhMatchedNonResidental { get; set; }

        /// <summary>
        /// Дата сопоставления с ГИС ЖКХ
        /// </summary>
        public virtual DateTime? GisGkhMatchDate { get; set; }

        /// <summary>
        /// Дата сопоставления ЛС по дому с ГИС ЖКХ
        /// </summary>
        public virtual DateTime? GisGkhAccMatchDate { get; set; }

        /// <summary>
        /// Количество нежилых помещений
        /// </summary>
        public virtual int? NumberNonResidentialPremises { get; set; }

        /// <summary>
        /// Площадь нежилых помещений
        /// </summary>
        public virtual decimal? AreaNotLivingPremises { get; set; }

        /// <summary>
        /// Количество входов (подъездов)
        /// </summary>
        public virtual int? NumberEntrances { get; set; }

        /// <summary>
        /// Количество лифтов
        /// </summary>
        public virtual int? NumberLifts { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public virtual int? NumberLiving { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Внутрнний Код дома
        /// </summary>
        public virtual string GkhCode { get; set; }

        /// <summary>
        /// Адрес веб камеры
        /// </summary>
        public virtual string WebCameraUrl { get; set; }

        /// <summary>
        /// дата проведения тех. обследования
        /// </summary>
        public virtual DateTime? DateTechInspection { get; set; }

        /// <summary>
        /// Жильцы выселены
        /// </summary>
        public virtual bool ResidentsEvicted { get; set; }

        /// <summary>
        /// Не хранимое поле для того чтобы удалить адрес если его уже нет
        /// </summary>
        [JsonIgnore]
        public virtual int DeleteAddressId { get; set; }

        /// <summary>
        /// Дом построен по соц.ипотеке(да/нет)
        /// </summary>
        public virtual YesNo IsBuildSocialMortgage { get; set; }


        /// <summary>
        /// Общий строительный объем
        /// </summary>
        public virtual decimal? TotalBuildingVolume { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка
        /// </summary>
        public virtual string CadastreNumber { get; set; }

        /// <summary>
        /// Кадастровый номер дома
        /// </summary>
        public virtual string CadastralHouseNumber { get; set; }

        /// <summary>
        /// Требовалось проведение КР на дату приватизации первого жилого помещения
        /// </summary>
        public virtual YesNoNotSet NecessaryConductCr { get; set; }

        /// <summary>
        /// Высота этажа
        /// </summary>
        public virtual decimal? FloorHeight { get; set; }

        /// <summary>
        /// Собираемость платежей на кап.ремонт, %
        /// </summary>
        public virtual decimal? PercentDebt { get; set; }

        /// <summary>
        /// Дата приватизации первого жилого помещения
        /// </summary>
        public virtual DateTime? PrivatizationDateFirstApartment { get; set; }

        /// <summary>
        /// В доме присутствуют приватизированные квартиры
        /// </summary>
        public virtual bool? HasPrivatizedFlats { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public virtual int? BuildYear { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Способ формирования фонда КР
        /// </summary>
        public virtual MethodFormFundCr MethodFormFundCr { get; set; }

        /// <summary>
        /// Наличие судебного решения по проведению Кр общего имущества
        /// </summary>
        public virtual YesNo HasJudgmentCommonProp { get; set; }

        /// <summary>
        /// Ремонт нецелесообразен
        /// </summary>
        public virtual bool IsRepairInadvisable { get; set; }

        /// <summary>
        /// Дом не участвует в КР (в программе кап. ремонта)
        /// </summary>
        public virtual bool IsNotInvolvedCr { get; set; }

        /// <summary>
        /// Наличие проектной дркументации
        /// </summary>
        public virtual TypePresence ProjectDocs { get; set; }

        /// <summary>
        /// Наличие энергетического паспорта
        /// </summary>
        public virtual TypePresence EnergyPassport { get; set; }

        /// <summary>
        /// Документы по гос.кадастровому учету зем.участка
        /// </summary>
        public virtual TypePresence ConfirmWorkDocs { get; set; }

        /// <summary>
        /// МО - поселение
        /// </summary>
        public virtual Municipality MoSettlement { get; set; }

        /// <summary>
        /// Наименования организаций, управляющих домом
        /// </summary>
        public virtual string ManOrgs { get; set; }

        /// <summary>
        /// Типы договоров управления
        /// </summary>
        public virtual string TypesContract { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual Dicts.RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Административный округ (для МСК) 
        /// </summary>
        public virtual string District { get; set; }

        /// <summary>
        /// Код справочника "UNOM" (для МСК)
        /// </summary>
        public virtual string UnomCode { get; set; }

        /// <summary>
        /// Код дома для ВЦКП
        /// </summary>
        public virtual string VtscpCode { get; set; }

        /// <summary>
        /// Баллы
        /// </summary>
        public virtual int? Points { get; set; }

        /// <summary>
        /// способ формирования фонда кр на текущий момент (обновляется через FillAccountFormationVariantAction)
        /// </summary>
        public virtual CrFundFormationType? AccountFormationVariant { get; set; }

        /// <summary>
        /// Код адреса (используется в импорте льготников)
        /// </summary>
        public virtual string AddressCode { get; set; }

        /// <summary>
        /// Гуид дома в ФИАСе
        /// </summary>
        public virtual string HouseGuid { get; set; }

        /// <summary>
        /// Объект строительства" в раздел
        /// </summary>
        public virtual YesNoNotSet ObjectConstruction { get; set; }

        /// <summary>
        /// Построен по программе переселения
        /// </summary>
        public virtual YesNoNotSet BuiltOnResettlementProgram { get; set; }

        /// <summary>
        /// Дом имеет статус объекта культурного наследия
        /// </summary>
        public virtual bool IsCulturalHeritage { get; set; }

        /// <summary>
        /// Участвует в программе КР ТО №2
        /// </summary>
        public virtual bool? IsInvolvedCrTo2 { get; set; }

        /// <summary>
        /// Дата последнего технического мониторинга
        /// </summary>
        public virtual DateTime? LatestTechnicalMonitoring { get; set; }

        /// <summary>
        /// Причина по которой проставился признак Дом не участвует в программе КР
        /// </summary>
        public virtual IsNotInvolvedCrReason IsNotInvolvedCrReason { get; set; }

        /// <summary>
        /// Дата исключения из ДПКР
        /// </summary>
        public virtual DateTime? UnpublishDate { get; set; }

        /// <summary>
        /// Дата присвоения для поля "Памятник архитектуры"
        /// </summary>
        public virtual DateTime? CulturalHeritageAssignmentDate { get; set; }

        /// <summary>
        /// Наименование ЦТП
        /// </summary>
        public virtual CentralHeatingStation CentralHeatingStation { get; set; }

        /// <summary>
        /// Порядковый номер объекта в ЦТП
        /// </summary>
        public virtual int? NumberInCtp { get; set; }

        /// <summary>
        /// Приоритет вывода из эксплуатации ЦТП
        /// </summary>
        public virtual int? PriorityCtp { get; set; }

        /// <summary>
        /// Номер документа	
        /// </summary>
        public virtual string MonumentDocumentNumber { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo MonumentFile { get; set; }

        /// <summary>
        /// Наименование органа, выдавшего документ о признании дома памятником архитектуры
        /// </summary>
        public virtual string MonumentDepartmentName { get; set; }

        /// <summary>
        /// Электронный паспорт дома
        /// </summary>
        public virtual FileInfo TechPassportFile { get; set; }

        /// <summary>
        /// Дом в подпрограмме
        /// </summary>
        public virtual bool IsSubProgram { get; set; }

        /// <summary>
        /// Включить в подпрограмму
        /// </summary>
        public virtual bool IncludeToSubProgram { get; set; }

        /// <summary>
        /// Настройки для выгрузки в Клиент-СБербанк
        /// </summary>
        public virtual ASSberbankClient ASSberbankClient { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGUID { get; set; }

        /// <summary>
        /// выводить на портал
        /// </summary>
        public virtual bool ExportedToPortal { get; set; }

        /// <summary>
        /// Имеет начисления по 185 ФЗ
        /// </summary>
        public virtual bool HasCharges185FZ { get; set; }

        /// <summary>
        /// Справка БТИ
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// широта
        /// </summary>
        public virtual decimal? Latitude { get; set; }

        /// <summary>
        /// долгота
        /// </summary>
        public virtual decimal? Longitude { get; set; }
        
        /// <summary>
        /// Двор.
        /// </summary>
        public virtual RealityObjectOutdoor Outdoor { get; set; }

        /// <summary>
        /// Инн управляющих компаний
        /// </summary>
        public virtual string InnManOrgs { get; set; }

        /// <summary>
        /// Дата начала управления
        /// </summary>
        public virtual string StartControlDate { get; set; }

        /// <summary>
        /// Включён в реестр ОКН
        /// </summary>
        public virtual bool? IsIncludedRegisterCHO { get; set; }

        /// <summary>
        /// Включён в перечень выявленных ОКН
        /// </summary>
        public virtual bool? IsIncludedListIdentifiedCHO { get; set; }

        /// <summary>
        /// Предмет охраны ОКН определен
        /// </summary>
        public virtual bool? IsDeterminedSubjectProtectionCHO { get; set; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (this.Id > 0)
            {
                return this.Id.GetHashCode();
            }

            return base.GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var ro = obj as RealityObject;
            if (this.Id != 0 && ro != null && ro.Id != 0)
            {
                return this.Id == ro.Id;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Возвращает адрес в пределах населенного пункта (начиная с улицы)
        /// </summary>
        /// <returns></returns>
        public virtual string GetLocalAddress()
        {
            if (this.FiasAddress == null)
            {
                return string.Empty;
            }

            var localAddress = new StringBuilder(this.FiasAddress.StreetName);

            this.AddAddressPart(localAddress, "д.", this.FiasAddress.House);
            this.AddAddressPart(localAddress, "лит.", this.FiasAddress.Letter);
            this.AddAddressPart(localAddress, "корп.", this.FiasAddress.Housing);
            this.AddAddressPart(localAddress, "секц.", this.FiasAddress.Building);

            return localAddress.ToString();
        }


        private void AddAddressPart(StringBuilder aggregator, string prefix, string part)
        {
            if (string.IsNullOrEmpty(part))
            {
                return;
            }

            if (aggregator.Length > 0)
            {
                aggregator.Append(", ");
            }

            aggregator.Append(prefix);
            aggregator.Append(" ");
            aggregator.Append(part);
        }

        /// <summary>
        /// Уникальный идентификатор ТОР.
        /// </summary>
        public virtual Guid? TorId { get; set; }
    }
}
