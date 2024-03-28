namespace Bars.Gkh.RegOperator.Services.DataContracts.GetOwnerInfo
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetOwnerInfo")]
    public class GetOwnerInfo
    {
        /// <summary>
        /// Номера лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AccNum")]
        public List<string> AccNum { get; set; }

        /// <summary>
        /// Мыло
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Чекер
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsSelected")]
        public bool IsSelected { get; set; }
    }
}