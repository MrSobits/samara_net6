namespace Bars.Gkh.Services.DataContracts.GetHousesInfoByEditDate
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts.HousesInfo;

    [DataContract]
    [XmlType(TypeName = "HouseInfoByEditDate")]
    public class HouseInfoByEditDate
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Адрес ФИАС
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AddressFias")]
        public string AddressFias { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MUnion")]
        public string MUnion { get; set; }

        /// <summary>
        /// Нас. пункт
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Settlement")]
        public string Settlement { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public string TotalArea { get; set; }

        /// <summary>
        /// Жилые и  нежилые площади
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiveUnliveArea")]
        public string LiveUnliveArea { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiveArea")]
        public string LiveArea { get; set; }


        /// <summary>
        /// Жилая площадь граждан
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PeopleLivingArea")]
        public string PeopleLivingArea { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Floor")]
        public int Floor { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BuildYear")]
        public int BuildYear { get; set; }

        /// <summary>
        /// Год сдачи в эксплуатацию
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExplYear")]
        public int ExplYear { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FlatNumber")]
        public int FlatNumber { get; set; }

        /// <summary>
        /// Количество жителей
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumberLiving")]
        public int NumberLiving { get; set; }

        /// <summary>
        /// Количество помещений: всего (ед.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FlatsCount")]
        public int FlatsCount { get; set; }

        /// <summary>
        /// Количество помещений: нежилых (ед.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingQuartersCount")]
        public int NotLivingQuartersCount { get; set; }

        /// <summary>
        /// Количество этажей: наибольшее (ед.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FloorCountMax")]
        public int FloorCountMax { get; set; }

        /// <summary>
        /// Количество этажей: наименьшее (ед.)	
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FloorCountMin")]
        public int FloorCountMin { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Residents")]
        public int Residents { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HouseType")]
        public string HouseType { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BuiltYear")]
        public int BuiltYear { get; set; }

        /// <summary>
        /// Процент износа дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OfWearPers")]
        public string OfWearPers { get; set; }

        /// <summary>
        /// Тип кровли
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofType")]
        public string RoofType { get; set; }

        /// <summary>
        /// Материал кровли
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofMaterial")]
        public string RoofMaterial { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "WallMaterial")]
        public string WallMaterial { get; set; }

        /// <summary>
        /// Страхование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Insurance")]
        public string Insurance { get; set; }

        /// <summary>
        /// Минимальная этажность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MinFloor")]
        public string MinFloor { get; set; }

        /// <summary>
        /// Наличие приватизированных квартир
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PrivatRoom")]
        public string PrivatRoom { get; set; }

        /// <summary>
        /// Дата приватизации первого жилого помещения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Data1PrivatRoom")]
        public string Data1PrivatRoom { get; set; }

        /// <summary>
        /// Требовалось проведение кап ремонта на дату приватизации первого помещения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NeedKapRem")]
        public string NeedKapRem { get; set; }

        /// <summary>
        /// Год последнего капремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LastKapRemDate")]
        public string LastKapRemDate { get; set; }

        /// <summary>
        /// Серия, тип проекта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProjectTip")]
        public string ProjectTip { get; set; }

        /// <summary>
        /// StaircaseNum
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StaircaseNum")]
        public string StaircaseNum { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участвка
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CadastrNumber")]
        public string CadastrNumber { get; set; }

        /// <summary>
        /// Кадастровый номер дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CadastralHouseNumber")]
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// Количество лифтов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiftNum")]
        public string LiftNum { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HeatingSystem")]
        public string HeatingSystem { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public string ManOrg { get; set; }

        /// <summary>
        /// Высота этажа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FloorHeight")]
        public string FloorHeight { get; set; }

        /// <summary>
        /// Сальдо на счету капремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CRSaldo")]
        public decimal CRSaldo { get; set; }

        /// <summary>
        /// Идентификатор управляющей организации
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdManOrg")]
        public string IdManOrg { get; set; }

        /// <summary>
        /// Фото до КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "BeforeKRPhoto")]
        public long[] BeforeKRPhoto { get; set; }

        /// <summary>
        /// Фото после КР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AfterKRPhoto")]
        public long[] AfterKRPhoto { get; set; }

        /// <summary>
        /// Сведения о конструктивных элементах: Фасады
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Facades")]
        public string Facades { get; set; }

        /// <summary>
        /// AreaCleaning
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AreaCleaning")]
        public string AreaCleaning { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений (кв.м.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PeopleNotLivingArea")]
        public decimal PeopleNotLivingArea { get; set; }

        /// <summary>
        /// Общая площадь помещений, входящих в состав общего имущества (кв.м.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GeneralArea")]
        public decimal GeneralArea { get; set; }

        /// <summary>
        /// Площадь нежилых помещений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingArea")]
        public string NotLivingArea { get; set; }

        /// <summary>
        /// Площадь мест общего пользования
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PublicArea")]
        public string PublicArea { get; set; }

        /// <summary>
        /// Количество жилых помещений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LivingRoomCount")]
        public int LivingRoomCount { get; set; }

        /// <summary>
        /// Количество жилых помещений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingRoomCount")]
        public int NotLivingRoomCount { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AreaNonResidental")]
        public decimal AreaNonResidental { get; set; }

        /// <summary>
        /// Количество подъездов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "EntranceCount")]
        public int EntranceCount { get; set; }

        /// <summary>
        /// Количество собственников
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumberOwners")]
        public int NumberOwners { get; set; }

        /// <summary>
        /// Способ формирования фонда капитального ремонта.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FormingOverhaulFund")]
        public int FormingOverhaulFund { get; set; }

        /// <summary>
        /// Количество жилых помещений
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TypesWorkData")]
        public TypesWork[] TypesWorkData { get; set; }

        /// <summary>
        /// Конструкция мусоропровода
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ConstructionGarbage")]
        public string ConstructionGarbage { get; set; }

        /// <summary>
        /// Вентиляция
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeVentilation")]
        public string TypeVentilation { get; set; }

        /// <summary>
        /// Количество вентиляционных каналов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumVentilationDuct")]
        public string NumVentilationDuct { get; set; }

        /// <summary>
        /// Количество мусоропроводов (шт.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumberOfRefuseChutes")]
        public int NumberOfRefuseChutes { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Теплоснабжение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HeatingType")]
        public string HeatingType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Горячего водоснабжение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HotWaterType")]
        public string HotWaterType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Холодного водоснабжение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ColdWaterType")]
        public string ColdWaterType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Электроснабжение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ElectricalType")]
        public string ElectricalType { get; set; }

        /// <summary>
        /// Сведения об инженерных системах: Водоотведение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SewerageType")]
        public string SewerageType { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HouseState")]
        public string HouseState { get; set; }

        /// <summary>
        /// Информация, участвует ли дом в программе КР
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OverhaulProgramShip")]
        public bool OverhaulProgramShip { get; set; }

        /// <summary>
        /// Дата включения дома в долгосрочную программу капитального ремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateOfInclusionInProgram")]
        public string DateOfInclusionInProgram { get; set; }

        /// <summary>
        /// Дата исключения дома из долгосрочной программы капитального ремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateOfExclusion")]
        public string DateOfExclusion { get; set; }

        /// <summary>
        /// Дата последнего тех. мониторинга МКД
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TechnicalSurveyDate")]
        public string TechnicalSurveyDate { get; set; }

        /// <summary>
        /// Площадь муниципальной собственности (кв. м.)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MunicipalArea")]
        public string MunicipalArea { get; set; }

        /// <summary>
        /// Площадь государственной собственности (кв.м)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PublicalArea")]
        public string PublicalArea { get; set; }

        /// <summary>
        /// Особые отметки дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SpecialMark")]
        public string SpecialMark { get; set; }

        /// <summary>
        /// Владелец счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AccountOwner")]
        public string AccountOwner { get; set; }

        /// <summary>
        /// Классификация дома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HouseClassification")]
        public string HouseClassification { get; set; }

        /// <summary>
        /// Дата сноса
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DemolitionDate")]
        public string DemolitionDate { get; set; }

        /// <summary>
        /// Отметка памятник архитектуры или нет
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Monument")]
        public bool Monument { get; set; }
    }
}