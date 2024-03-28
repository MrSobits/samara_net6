namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RealityObject")]
    public class RealityObject
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// МуниципальноеОбразование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Municipality")]
        public string Municipality { get; set; }

        /// <summary>
        /// ДатаИзменения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateChange")]
        public string DateChange { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// АдресКЛАДР 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AddressKladr")]
        public string AddressKladr { get; set; }

        /// <summary>
        /// ГодСдачиВЭксплуатацию
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExpluatationYear")]
        public string ExpluatationYear { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Floor")]
        public int Floor { get; set; }

        /// <summary>
        /// КоличествоКвартир
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ApartamentCount")]
        public int ApartamentCount { get; set; }

        /// <summary>
        /// КоличествоПроживающих
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LivingPeople")]
        public int LivingPeople { get; set; }

        /// <summary>
        /// ОбщаяПлощадь
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GeneralArea")]
        public string GeneralArea { get; set; }

        /// <summary>
        /// ФизическийИзнос
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Deterioration")]
        public string Deterioration { get; set; }

        /// <summary>
        /// ГодПоследнегоКапремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "YearCapRepair")]
        public string YearCapRepair { get; set; }

        /// <summary>
        /// СерияДома
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SeriaHouse")]
        public string SeriaHouse { get; set; }

        /// <summary>
        /// НаличиеПодвала
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Fasad")]
        public string Fasad { get; set; }

        /// <summary>
        /// КоличествоПодъездов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GatesCount")]
        public int GatesCount { get; set; }

        /// <summary>
        /// ЖилыеИНежилыеПлощади
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LivingNotLivingArea")]
        public string LivingNotLivingArea { get; set; }

        /// <summary>
        /// ЖилаяПлощадь
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LivingArea")]
        public string LivingArea { get; set; }

        /// <summary>
        /// ЖилаяПлощадьГраждан
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LivingAreaPeople")]
        public string LivingAreaPeople { get; set; }

        /// <summary>
        /// КоличествоЛифтов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiftCount")]
        public int LiftCount { get; set; }

        /// <summary>
        /// ПлощадьПодвала
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FasadArea")]
        public string FasadArea { get; set; }

        /// <summary>
        /// ТипКровли
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofType")]
        public string RoofType { get; set; }

        /// <summary>
        /// МатериалКровли
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofMaterial")]
        public string RoofMaterial { get; set; }

        /// <summary>
        /// МатериалСтен
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
        /// ДатаПроведенияТехническогоОбследования
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        /// <summary>
        /// УправляющиеОрганизации
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ManagingOrg")]
        public ManagingOrgItem[] ManagingOrg { get; set; }

        /// <summary>
        /// ПрограммыКР
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ProgrammCr")]
        public ProgrammCrItem[] ProgrammCr { get; set; }

        /// <summary>
        /// ДвиженияДенежныхСредств
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "MovementMoney")]
        public MovementMoneyItem[] MovementMoney { get; set; }

        /// <summary>
        /// ДенежныеСредстваРегФонд
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ResourceRf")]
        public ResourceRf ResourceRf { get; set; }

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
        /// Количество нежилых помещений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingRoomCount")]
        public int NotLivingRoomCount { get; set; }
    }
}