namespace Bars.Gkh.RegOperator.Services.DataContracts.Accounts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Лицевой счет для мобильного приложения
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "PersonalAccountDTO")]

    public class PersonalAccountMobileDTO
    {
        /// <summary>
        /// ID лицевого счета
        /// regop_pers_acc
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }
        
        /// <summary>
        ///Номер лицевого счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PersonalAccountNum")]
        public string PersonalAccountNum { get; set; }
        
        /// <summary>
        /// Площадь, относящаяся к лс
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Area")]
        public virtual decimal Area { get; set; }
        
        /// <summary>
        /// Адрес помещения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }
       
        /// <summary>
        /// Доля собственности
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Share")]
        public decimal Share { get; set; }
        
        /// <summary>
        /// Тариф
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Tariff")]
        public decimal Tariff { get; set; }
        
    }
}