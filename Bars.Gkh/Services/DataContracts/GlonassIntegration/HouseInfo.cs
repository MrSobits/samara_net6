namespace Bars.Gkh.Services.DataContracts.GlonassIntegration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Модель информации по дому
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "HouseInfo")]
    public class HouseInfo
    {
        /// <summary>Идентификтаор дома</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>Адрес дома</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        /// <summary>Общая площадь</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public string TotalArea { get; set; }

        /// <summary>Общая площадь жилых и нежилых помещений</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiveUnliveArea")]
        public string LiveUnliveArea { get; set; }

        /// <summary>Жилая площадь, кв.м</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "LiveArea")]
        public string LiveArea { get; set; }

        /// <summary>Нежилая площадь, кв.м</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NotLivingArea")]
        public string NotLivingArea { get; set; }

        /// <summary>Этажность</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Floor")]
        public string Floor { get; set; }

        /// <summary>Год сдачи в эксплуатацию</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExplYear")]
        public string ExplYear { get; set; }

        /// <summary>Количество квартир</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FlatNumber")]
        public string FlatNumber { get; set; }

        /// <summary>Количество подъездов</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "EntranceNumber")]
        public string EntranceNumber { get; set; }

        /// <summary>Тип кровли</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofType")]
        public string RoofType { get; set; }

        /// <summary>Материал кровли</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofMaterial")]
        public string RoofMaterial { get; set; }

        /// <summary>Материал стен</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "WallMaterial")]
        public string WallMaterial { get; set; }

        /// <summary>Материал перекрытий</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FlatMaterial")]
        public string FlatMaterial { get; set; }

        /// <summary>Страхование</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Insurance")]
        public string Insurance { get; set; }

        /// <summary>УО</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public string ManOrg { get; set; }

        /// <summary>Тип системы отопления</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HeatingType")]
        public string HeatingType { get; set; }

        /// <summary>Тип системы горячего водоснабжения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HotWaterType")]
        public string HotWaterType { get; set; }

        /// <summary>Газификация</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GasType")]
        public string GasType { get; set; }

        /// <summary>Расположение выходов на чердак (кровлю)</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RoofEntrance")]
        public string RoofEntrance { get; set; }

        /// <summary>Расположение узлов ввода теплоснабжения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HeatingEntrance")]
        public string HeatingEntrance { get; set; }

        /// <summary>Расположение узлов ввода водоснабжения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "WaterEntrance")]
        public string WaterEntrance { get; set; }

        /// <summary>Расположение узлов ввода электроснабжения </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ElectroEntrance")]
        public string ElectroEntrance { get; set; }
    }
}