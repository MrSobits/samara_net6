namespace Bars.Gkh.RegOperator.Wcf.Contracts.ExchangeDocument
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ExchangeDocument
    {
        [DataMember]
        [XmlElement(ElementName = "Заголовок файла")]
        public FileHeader FileHeader { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Секция платежного документа")]
        public PaymentDoc PaymentDoc { get; set; }
    }

    [DataContract]
    public class FileHeader
    {
        [DataMember]
        [XmlAttribute(AttributeName = "1CClientBankExchange")]
        public string FileInnerAttribute { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Общие сведения")]
        public CommonInfo CommonInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Сведения об условиях отбора передаваемых данных")]
        public ConditionInfo ConditionInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Секция передачи остатков по расчетному счету")]
        public Remains Remains { get; set; }
    }

    [DataContract]
    public class PaymentDoc
    {
        [DataMember]
        [XmlElement(ElementName = "Шапка платежного документа")]
        public PaymentHeader Header { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Квитанция по платежному документу")]
        public PaymentBill Bill { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Реквизиты плательщика")]
        public PayerDetails PayerDetails { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Реквизиты получателя")]
        public RecieverDetails RecieverDetails { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Реквизиты платежа")]
        public PaymentDetails Details { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Дополнительные реквизиты для платежей в бюджетную систему Российской Федерации")]
        public AdditionalPaymentDetails AdditionalDetails { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Дополнительные реквизиты для отдельных видов документов")]
        public AdditionalDocDetails AdditionalDocDetails { get; set; }
    }

    [DataContract]
    public class PaymentHeader
    {
        [DataMember]
        [XmlAttribute(AttributeName = "СекцияДокумент=<Вид документа>")]
        public string Section { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Номер")]
        public string Number { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Дата")]
        public DateTime Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Сумма")]
        public decimal Sum { get; set; }

    }

    [DataContract]
    public class PaymentBill
    {
        [DataMember]
        [XmlAttribute(AttributeName = "КвитанцияДата")]
        public DateTime Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "КвитанцияВремя")]
        public DateTime Time { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "КвитанцияСодержание")]
        public string Content { get; set; }
    }

    [DataContract]
    public class PayerDetails
    {
        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикСчет")]
        public string Account { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ДатаСписано")]
        public DateTime Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Плательщик")]
        public string Reciever { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикИНН")]
        public string Inn { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Плательщик1")]
        public string Payer1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Плательщик2")]
        public string Payer2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Плательщик3")]
        public string Payer3 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Плательщик4")]
        public string Payer4 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикРасчСчет")]
        public string PayAccount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикБанк1")]
        public string Bank1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикБанк2")]
        public string Bank2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикБИК")]
        public string Bik { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикКорсчет")]
        public string Kor { get; set; }
    }

    [DataContract]
    public class RecieverDetails
    {
        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательСчет")]
        public string Account { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ДатаПоступило")]
        public DateTime Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Получатель")]
        public string Reciever { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательИНН")]
        public string Inn { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Получатель1")]
        public string Reciever1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Получатель2")]
        public string Reciever2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Получатель3")]
        public string Reciever3 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Получатель4")]
        public string Reciever4 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательРасчСчет")]
        public string RecAccount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательБанк1")]
        public string Bank1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательБанк2")]
        public string Bank2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательБИК")]
        public string Bik { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательКорсчет")]
        public string Kor { get; set; }
    }

    [DataContract]
    public class PaymentDetails
    {
        [DataMember]
        [XmlAttribute(AttributeName = "ВидПлатежа")]
        public string Type { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ВидОплаты")]
        public int Kind { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Код")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа")]
        public string Purpose { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 1")]
        public string Purpose1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 2")]
        public string Purpose2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 3")]
        public string Purpose3 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 4")]
        public string Purpose4 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 5")]
        public string Purpose5 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НазначениеПлатежа 6")]
        public string Purpose6 { get; set; }
    }

    [DataContract]
    public class AdditionalPaymentDetails
    {
        [DataMember]
        [XmlAttribute(AttributeName = "СтатусСоставителя")]
        public string MakerStatus { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлательщикКПП")]
        public string PayerKpp { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПолучательКПП")]
        public string RecieverKpp { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательКБК")]
        public string KbkPerformance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ОКАТО")]
        public string Okato { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательОснования")]
        public string BasePerformance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательПериода")]
        public string PeriodPerformance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательНомера")]
        public string NumberPerformance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательДаты")]
        public DateTime DatePerformance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПоказательТипа")]
        public string TypePerformance { get; set; }

    }

    [DataContract]
    public class AdditionalDocDetails
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Очередность")]
        public string Order { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "СрокАкцепта")]
        public int AcceptionDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ВидАккредитива")]
        public string AccrType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "СрокПлатежа")]
        public DateTime PaymenTime { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "УсловиеОплаты1")]
        public string PaymentCondition1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "УсловиеОплаты2")]
        public string PaymentCondition2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "УсловиеОплаты3")]
        public string PaymentCondition3 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ПлатежПоПредст")]
        public string PaymentRepresent { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ДополнУсловия")]
        public string AdditionalConditions { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "НомерСчетаПоставщика")]
        public string ProviderAccountNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ДатаОтсылкиДок")]
        public DateTime DocSendDate { get; set; }

    }

    [DataContract]
    public class ConditionInfo
    {
        [XmlAttribute(AttributeName = "ДатаНачала")]
        public DateTime BeginDate { get; set; }

        [XmlAttribute(AttributeName = "ДатаКонца")]
        public DateTime EndDate { get; set; }

        [XmlAttribute(AttributeName = "РасчСчет")]
        public string BankingAccount { get; set; }

        [XmlAttribute(AttributeName = "Документ")]
        public string Document { get; set; }
    }

    [DataContract]
    public class Remains
    {
        [XmlAttribute(AttributeName = "СекцияРасчСчет")]
        public string BeginBankingAccount { get; set; }

        [XmlAttribute(AttributeName = "ДатаНачала")]
        public DateTime BeginDate { get; set; }

        [XmlAttribute(AttributeName = "ДатаКонца")]
        public DateTime EndDate { get; set; }

        [XmlAttribute(AttributeName = "РасчСчет")]
        public string BankingAccount { get; set; }

        [XmlAttribute(AttributeName = "НачальныйОстаток")]
        public decimal FirstRemain { get; set; }

        [XmlAttribute(AttributeName = "ВсегоПоступило")]
        public decimal TotalCame { get; set; }

        [XmlAttribute(AttributeName = "ВсегоСписано")]
        public decimal TotalWriteOff { get; set; }

        [XmlAttribute(AttributeName = "КонечныйОстаток")]
        public decimal LastRemain { get; set; }

        [XmlAttribute(AttributeName = "КонецРасчСчет")]
        public string EndBankingAccount { get; set; }
    }

    [DataContract]
    [XmlRoot(ElementName = "Общие сведения")]
    public class CommonInfo
    {
        [XmlAttribute(AttributeName = "ВерсияФормата")]
        public string FormatVersion { get; set; }

        [XmlAttribute(AttributeName = "Кодировка")]
        public string Codepage { get; set; }

        [XmlAttribute(AttributeName = "Отправитель")]
        public string Sender { get; set; }

        [XmlAttribute(AttributeName = "Получатель")]
        public string Reciever { get; set; }

        [XmlAttribute(AttributeName = "ДатаСоздания")]
        public DateTime CreationDate { get; set; }

        [XmlAttribute(AttributeName = "ВремяСоздания")]
        public DateTime CreationTime { get; set; }
    }
}
