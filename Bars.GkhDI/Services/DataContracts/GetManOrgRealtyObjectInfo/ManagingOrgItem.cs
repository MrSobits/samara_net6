namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ManagingOrgItem")]
    public class ManagingOrgItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Информация о файле
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FileInfo")]
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ЮридическийАдрес
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "JuridicalAddress")]
        public string JuridicalAddress { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Ogrn")]
        public string Ogrn { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Inn")]
        public string Inn { get; set; }

        /// <summary>
        /// ГодРегистрации
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "YearRegistration")]
        public string YearRegistration { get; set; }

        /// <summary>
        /// ОрганПринявшийРешениеОРегистрации
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OgrnRegistration")]
        public string OgrnRegistration { get; set; }

        /// <summary>
        /// ПочтовыйАдрес
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PostAddress")]
        public string PostAddress { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Телефон дисп.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhoneDispatchService")]
        public string PhoneDispatchService { get; set; }
        
        /// <summary>
        /// ЭлектроннаяПочта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// ОфициальныйСайт
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Suite")]
        public string Suite { get; set; }

        /// <summary>
        /// ДатаНачалаДействияДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContractStart")]
        public string ContractStart { get; set; }

        /// <summary>
        /// ДатаОкончанияДействияДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContractEnd")]
        public string ContractEnd { get; set; }

        /// <summary>
        /// ПлощадьМКД
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MkdArea")]
        public string MkdArea { get; set; }

        /// <summary>
        /// ТипДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContractType")]
        public string ContractType { get; set; }

        /// <summary>
        /// ТипУправления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ManagingType")]
        public string ManagingType { get; set; }

        /// <summary>
        /// ДанныеПоДому  
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DataByRealityObject")]
        public DataByRealityObject DataByRealityObject { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentDate")]
        public string DocumentDate { get; set; }
 
        /// <summary>
        /// Основание расторжения 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TerminateReason")]
        public string TerminateReason { get; set; }

        /// <summary>
        /// Дата расторжения 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TerminationDate")]
        public string TerminationDate { get; set; }

        /// <summary>
        /// Документ, подтверждающий выбранный способ управления:Номер документа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Документ, подтверждающий выбранный способ управления:Наименование документа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentName")]
        public string DocumentName { get; set; }

        /// <summary>
        /// Основание управления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContractFoundation")]
        public string ContractFoundation { get; set; }

        /// <summary>
        /// Основание управления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DirectorFio")]
        public string DirectorFio { get; set; }
        
        /// Сведения о коммунальных услугах
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "HouseCommunalService")]
        public HouseCommunalService[] HouseCommunalService { get; set; }

        /// <summary>
        /// Сведения об использовании мест общего пользования
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "HouseCommonProperty")]
        public HouseCommonProperty[] HouseCommonProperty { get; set; }

        /// <summary>
        /// Финансовые показатели
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "HouseReportCommon")]
        public HouseReportCommon[] HouseReportCommon { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ManagServs")]
        public ManagServ[] ManagServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "HousingServs")]
        public HousingServ[] HousingServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ComServs")]
        public ComServ[] ComServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorksTo")]
        public ServiceWorkTo[] WorksTo { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorksPpr")]
        public ServiceWorkPpr[] WorksPpr { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Protocols")]
        public Protocol[] Protocols { get; set; }
    }
}