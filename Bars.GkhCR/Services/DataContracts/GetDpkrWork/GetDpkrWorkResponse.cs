namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhCr.Services.DataContracts;

    /// <summary>
    /// Получение ДПКР работы
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetDpkrWorkResponse")]
    public class GetDpkrWorkResponse
    {
        /// <summary>
        /// ДПКР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DPKR")]
        public DPKR[] DPKR { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}