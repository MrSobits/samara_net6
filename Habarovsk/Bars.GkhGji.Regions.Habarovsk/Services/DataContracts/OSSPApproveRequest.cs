namespace Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCit
{
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "OSSPApproveRequest")]
    public class OSSPApproveRequest
    {

        /// <summary>
        /// Заголовок РКК ОГ.
        /// </summary>        
        [DataMember]
        [XmlElement(ElementName = "EsiaId")]
        public string EsiaId { get; set; }

        /// <summary>
        /// Содержание обращения
        /// </summary>        
        [DataMember]
        [XmlElement(ElementName = "FiasGuid")]
        public string FiasGuid { get; set; }

        /// <summary>
        /// Особая отметка
        /// </summary>        
        [DataMember]
        [XmlElement(ElementName = "HouseId")]
        public string HouseId { get; set; }

        /// <summary>
        /// DateFrom
        /// </summary>        
        [DataMember]
        [XmlElement(ElementName = "DateFrom")]
        public string DateFrom { get; set; }

        /// <summary>
        /// DateTo
        /// </summary>        
        [DataMember]
        [XmlElement(ElementName = "DateTo")]
        public string DateTo { get; set; }

        /// <summary>
        /// Рег.номер РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Дата регистрации РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Fio")]
        public string Fio { get; set; }

        /// <summary>
        /// CadastralNumber
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CadastralNumber")]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Адресаты
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AttorneyFio")]
        public string AttorneyFio { get; set; }

        /// <summary>
        /// Предмет ведения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AttorneyNumber")]
        public string AttorneyNumber { get; set; }

        /// <summary>
        /// Предмет ведения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AttorneyDate")]
        public string AttorneyDate { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProtocolNum")]
        public string ProtocolNum { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProtocolDate")]
        public string ProtocolDate { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocNumber")]
        public string DocNumber { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocDate")]
        public string DocDate { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Note")]
        public string Note { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Room")]
        public string Room { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "OwnerProtocolType")]
        public string OwnerProtocolType { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "RequestNumber")]
        public string RequestNumber { get; set; }

        [DataMember]
        [XmlElement(ElementName = "File")]
        public File File { get; set; }

    }

    [DataContract]
    [XmlType(TypeName = "OSSRequestHistoryResponce")]
    public class OSSRequestHistoryResponce
    {
        [DataMember]
        [XmlElement(ElementName = "AppealCitResult")]
        public AppealCitResult AppealCitResult { get; set; }


        [DataMember]
        [XmlArray(ElementName = "OSSRequestHistory")]
        public OSSRequestHistory[] OSSRequestHistory { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "OSSRequestStateResponce")]
    public class OSSRequestStateResponce
    {

        [DataMember]
        [XmlElement(ElementName = "AppealCitResult")]
        public AppealCitResult AppealCitResult { get; set; }


        [DataMember]
        [XmlArray(ElementName = "OSSRequestsState")]
        public OSSRequestState[] OSSRequestsState { get; set; }

    }

    [DataContract]
    [XmlType(TypeName = "SendOSSPApproveResult")]
    public class SendOSSPApproveResult
    {
        [DataMember]
        [XmlElement(ElementName = "AppealCitResult")]
        public AppealCitResult AppealCitResult { get; set; }
        

         [DataMember]
        [XmlArray(ElementName = "OSSProtocols")]
        public OSSProtocol[] OSSProtocols { get; set; }
    }

    public class OSSProtocol
    {
        [DataMember]
        [XmlElement(ElementName = "RegNumber")]
        public string RegNumber { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RegDate")]
        public string RegDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ProtocolDate")]
        public string ProtocolDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "File")]
        public File File { get; set; }
    }

    public class OSSRequestState
    {
        [DataMember]
        [XmlElement(ElementName = "RequestId")]
        public string RequestId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "State")]
        public string State { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Comment")]
        public string Comment { get; set; }

    }

    public class OSSRequestHistory
    {
        [DataMember]
        [XmlElement(ElementName = "RequestId")]
        public string RequestId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "NoticeDate")]
        public string NoticeDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "NoticeTime")]
        public string NoticeTime { get; set; }

        [DataMember]
        [XmlElement(ElementName = "NoticeText")]
        public string NoticeText { get; set; }

    }

    [DataContract]
    [XmlType(TypeName = "GetProtocolTypesResult")]
    public class GetProtocolTypesResult
    {
        [DataMember]
        [XmlElement(ElementName = "AppealCitResult")]
        public AppealCitResult AppealCitResult { get; set; }


        [DataMember]
        [XmlArray(ElementName = "ProtocolTypes")]
        public ProtocolType[] ProtocolTypes { get; set; }
    }

    public class ProtocolType
    {
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

    }

}