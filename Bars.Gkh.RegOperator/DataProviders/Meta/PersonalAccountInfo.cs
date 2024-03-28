namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    class PersonalAccountInfo
    {
        public string ИдентификаторПлательщика { get; set; }

        public string ТипПлательщика { get; set; }

        public string ФИО { get; set; }

        public string НаименованиеРайона { get; set; }

        public string НаселенныйПункт { get; set; }

        public string Улица { get; set; }

        public string Дом { get; set; }

        public string БукваДома { get; set; }

        public string КорпусДома { get; set; }

        public string Секция { get; set; }

        public string НомерКвартиры { get; set; }

        public string БукваКвартиры { get; set; }

        public decimal ПлощадьПомещения { get; set; }

        public decimal Тариф { get; set; }

        public decimal СальдоНаНачало { get; set; }

        public decimal КрНачисленоЗаПериод { get; set; }

        public decimal КрПерерасчет { get; set; }

        public decimal КрОплаченоЗаПериод { get; set; }

        public decimal ПеняНачисленоЗаПериод { get; set; }

        public decimal ПеняПерерасчет { get; set; }

        public decimal ПеняОплаченоЗаПериод { get; set; }

        public string ОтчетныйПериод { get; set; }

        public string ИнформационноеСообщение { get; set; }
    }
}
