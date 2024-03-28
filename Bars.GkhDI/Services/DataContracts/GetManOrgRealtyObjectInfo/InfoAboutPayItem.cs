namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "InfoAboutPayItem")]
    public class InfoAboutPayItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ТипУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeGroupServiceDi")]
        public string TypeGroupServiceDi { get; set; }

        /// <summary>
        /// ПоставщикУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Provider")]
        public string Provider { get; set; }

        /// <summary>
        /// ПоказаниеСчетчикаНаНачалоПер
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CounterValuePeriodStart")]
        public string CounterValuePeriodStart { get; set; }

        /// <summary>
        /// ПоказаниеСчетчикаНаОкончаниеПер
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CounterValuePeriodEnd")]
        public string CounterValuePeriodEnd { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Accrual")]
        public string Accrual { get; set; }


        /// <summary>
        /// Оплачено
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Payed")]
        public string Payed { get; set; }


        /// <summary>
        /// Долг
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Debt")]
        public string Debt { get; set; }

        /// <summary>
        /// ОбщиеНачисления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GeneralAccrual")]
        public string GeneralAccrual { get; set; }

        /// <summary>
        /// Сбор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Collection")]
        public string Collection { get; set; }
    }
}