namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RealityObjectItem")]
    public class RealityObjectItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор муниципального района
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MoId")]
        public string MoId { get; set; }

        /// <summary>
        /// МуниципальноеОбразование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Municipality")]
        public string Municipality { get; set; }

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
        /// Периоды
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Periods")]
        public PeriodItem[] Periods { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BuiltYear")]
        public string BuiltYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HouseType")]
        public string HouseType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FloorCountMax")]
        public int FloorCountMax { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FloorCountMin")]
        public int FloorCountMin { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FlatsCount")]
        public long FlatsCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingQuartersCount")]
        public long NotLivingQuartersCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaResidential")]
        public decimal AreaResidential { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaNonResidential")]
        public decimal AreaNonResidential { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaCommonProperty")]
        public decimal AreaCommonProperty { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CadastralNumbers")]
        public string CadastralNumbers { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaLand")]
        public decimal AreaLand { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ParkingSquare")]
        public decimal ParkingSquare { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "EnergyEfficiency")]
        public string EnergyEfficiency { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HasPlayground")]
        public bool HasPlayground { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HasSportsground")]
        public bool HasSportsground { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MethodOfFormingOverhaulFund")]
        public string MethodOfFormingOverhaulFund { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CommonMeetingProtocolDate")]
        public string CommonMeetingProtocolDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CommonMeetingProtocolNumber")]
        public string CommonMeetingProtocolNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FoundationType")]
        public string FoundationType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaBasement")]
        public string AreaBasement { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FloorType")]
        public string FloorType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Facades")]
        public string Facades { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Roofs")]
        public string Roofs { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "СhuteЕype")]
        public string СhuteЕype { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "СhuteСount")]
        public string СhuteСount { get; set; }

        [DataMember]
        [XmlArray(ElementName = "HouseCommunalService")]
        public HouseCommunalService[] HouseCommunalService { get; set; }

    }
}