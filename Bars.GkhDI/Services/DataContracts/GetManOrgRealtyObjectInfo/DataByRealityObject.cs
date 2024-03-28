namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DataByRealityObject")]
    public class DataByRealityObject
    {
        /// <summary>
        /// СведенияОбУслугах 
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Services")]
        public ServiceItem[] Services { get; set; }

        /// <summary>
        /// ИспользованиеНежилыхПомещений 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NonResidentialPlace")]
        public NonResidentialPlace NonResidentialPlace { get; set; }

        /// <summary>
        /// ИспользованиеМестОбщегоПользования  
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "UseCommonFacil")]
        public UseCommonFacil UseCommonFacil { get; set; }

        /// <summary>InfoAboutUseCommonFacil
        /// СведенияОбОплатах
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "InfoAboutPay")]
        public InfoAboutPayItem[] InfoAboutPay { get; set; }

        /// <summary>
        /// ПрочиеУслуги 
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "OtherService")]
        public OtherServiceItem[] OtherService { get; set; }

        /// <summary>
        /// ПорядокИУсловияОказанияУслуг
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "OrderAndConditionService")]
        public OrderAndConditionService OrderAndConditionService { get; set; }

        /// <summary>
        /// Информация о наличии претензий по качеству выполненных работ (оказанных услуг)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Claims")]
        public Claims[] Claims { get; set; }

        /// <summary>
        /// Приборы учёта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HouseMeteringDevice")]
        public HouseMeteringDevice[] HouseMeteringDevice { get; set; }


        /// <summary>
        /// Инженерные системы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "EngineeringSystem")]
        public EngineeringSystem[] EngineeringSystem { get; set; }
    }
}