namespace Bars.Gkh.Services.DataContracts.GetOperationTime
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Инфо нового дома
    /// </summary>
    [DataContract]
    [XmlRoot(ElementName = "HouseInfo")]
    public class GetCreateHouseResponse
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "House_type")]
        public string HouseType { get; set; }

        /// <summary>
        /// Guid улицы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Street_id")]
        public string StreetAoGuid { get; set; }

        //[DataMember]
        //[XmlElement(ElementName = "Streetname")]
        //public string StreetName { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Housenum")]
        public string HouseNum { get; set; }

        /// <summary>
        /// Корпус дома
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Korpus")]
        public string Korpus { get; set; }

        /// <summary>
        /// Строение
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Building")]
        public string Building { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Startupday")]
        public string StartupDay { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Builtyear")]
        public string BuiltYear { get; set; }

        /// <summary>
        /// Общая площадь дома
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Totalarea")]
        public string TotalArea { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Maxfloors")]
        public string Maxfloors { get; set; }

        /// <summary>
        /// Минимальная этажность
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Minfloors")]
        public string Minfloors { get; set; }

        /// <summary>
        /// Код муниципального учреждения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Municipal_code")]
        public string MunicipalCode { get; set; }
    }

    /// <summary>
    /// Выходные параметры сервиса создания нового дома
    /// </summary>
    [DataContract]
    [XmlType(Namespace = "GetCreateHouseResult", TypeName = "Result")]
    public class GetCreateHouseResult
    {
        /// <summary>
        /// Код результата запроса
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }
    }
}