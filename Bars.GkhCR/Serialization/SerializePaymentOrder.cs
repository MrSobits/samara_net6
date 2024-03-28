namespace Bars.GkhCr.Serialization
{
    using System;
    using System.Diagnostics;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Документ выписка из системы из АРМ «Бюджетополучатель» системы АЦК-Финансы
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "EXTRWT", IsNullable = false)]
    public class ExtrWt
    {
        [XmlElement("WTFORMAT", Form = XmlSchemaForm.Unqualified)]
        public Format Item { get; set; }
    }

    [DebuggerStepThrough]
    [XmlType(TypeName = "WTFORMAT")]
    public class Format
    {
        [XmlElement(ElementName = "STATEMENTSDOCS", Form = XmlSchemaForm.Unqualified)]
        public StatementsDocs[] StatementsDocs { get; set; }

        [XmlAttribute(AttributeName = "VERSION")]
        public string Version { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(TypeName = "STATEMENTSDOCS")]
    public class StatementsDocs
    {
        [XmlElement("STATEMENTSDOC")]
        public StatementDoc[] StatementDoc { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(TypeName = "STATEMENTSDOC")]
    public class StatementDoc
    {
        [XmlArray("DEBETDOCUMENTS")]
        [XmlArrayItem("DEBETDOCUMENT", typeof(DebetDocument))]
        public DebetDocument[] DebetDocuments { get; set; }

        [XmlArray("CREDITDOCUMENTS")]
        [XmlArrayItem("CREDITDOCUMENT", typeof(CreditDocument))]
        public CreditDocument[] CreditDocuments { get; set; }

        [XmlAttribute("BUDGETID")]
        public string BudgetId { get; set; }

        [XmlAttribute("BUDGETNAME")]
        public string BudgetName { get; set; }

        [XmlAttribute("BUDGETYEAR")]
        public string BudgetYear { get; set; }

        [XmlAttribute("CLOSINGBALANCE")]
        public string ClosingBalance { get; set; }

        [XmlAttribute("COMPBUDGETINN")]
        public string CompBudgetInn { get; set; }

        /// <summary>
        /// КПП ФО, исполняющего бюджет
        /// </summary>
        [XmlAttribute("COMPBUDGETKPP")]
        public string CompBudgetKpp { get; set; }

        /// <summary>
        /// Наименование ФО, исполняющего бюджет
        /// </summary>
        [XmlAttribute("COMPBUDGETNAME")]
        public string CompBudgetName { get; set; }

        /// <summary>
        /// Всего документов по кредиту
        /// </summary>
        [XmlAttribute("CREDITDOCSNUM")]
        public string CreditDocsNum { get; set; }

        /// <summary>
        /// Обороты по кредиту
        /// </summary>
        [XmlAttribute("CREDITTURNOVER")]
        public string CreditTurnOver { get; set; }

        [XmlAttribute("DEBETDOCSNUM")]
        public string DebetDocsNum { get; set; }

        [XmlAttribute("DEBETTURNOVER")]
        public string DebetTurnOver { get; set; }

        [XmlAttribute("DOCUMENTDATE")]
        public string DocumentDate { get; set; }

        [XmlAttribute("DOCUMENTNUMBER")]
        public string DocumentNumber { get; set; }

        [XmlAttribute("ESTIMATENAME")]
        public string EstimateName { get; set; }

        [XmlAttribute("ESTIMATETYPE")]
        public string EstimateType { get; set; }

        /// <summary>
        /// Последний день операции по счету
        /// </summary>
        [XmlAttribute("LASTSTATEMENTDATE")]
        public string LastStatementDate { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        [XmlAttribute("OPENINGBALANCE")]
        public string OpeningBalance { get; set; }

        /// <summary>
        /// Счет для финансирования
        /// </summary>
        [XmlAttribute("PAYERACCOUNTNUMBER")]
        public string PayerAccountNumber { get; set; }

        [XmlAttribute("PAYERBANKBRANCHCODE")]
        public string PayerBankBranchCode { get; set; }

        [XmlAttribute("PAYERBANKBRANCHNAME")]
        public string PayerBankBranchName { get; set; }

        /// <summary>
        /// БИК банка
        /// </summary>
        [XmlAttribute("PAYERBIC")]
        public string PayerBic { get; set; }

        [XmlAttribute("PAYERID")]
        public string PayerId { get; set; }

        /// <summary>
        /// ИНН организации – владельца сметы
        /// </summary>
        [XmlAttribute("PAYERINN")]
        public string PayerInn { get; set; }

        [XmlAttribute("PAYERKPP")]
        public string PayerKpp { get; set; }

        /// <summary>
        /// Название организации – владельца бланка расходов
        /// </summary>
        [XmlAttribute("PAYERNAME")]
        public string PayerName { get; set; }

        /// <summary>
        /// Номер счета ОФК организации-владельца сметы
        /// </summary>
        [XmlAttribute("PAYEROFKACCOUNTNUMBER")]
        public string PayerOfkAccountNumber { get; set; }

        [XmlAttribute("PAYEROFKCODE")]
        public string PayerOfkCode { get; set; }

        [XmlAttribute("PAYEROFKID")]
        public string PayerOfKid { get; set; }

        [XmlAttribute("PAYEROFKINN")]
        public string PayerOfkInn { get; set; }

        [XmlAttribute("PAYEROFKKPP")]
        public string PayerOfkKpp { get; set; }

        [XmlAttribute("PAYEROFKNAME")]
        public string PayerOfkName { get; set; }

        [XmlAttribute("PAYERUFKACCOUNTNUMBER")]
        public string PayerUfkAccountNumber { get; set; }

        [XmlAttribute("PAYERUFKINN")]
        public string PayerUfkInn { get; set; }

        [XmlAttribute("PAYERUFKKPP")]
        public string PayerUfkKpp { get; set; }

        [XmlAttribute("PAYERUFKNAME")]
        public string PayerUfkName { get; set; }

        [XmlAttribute("PERSONAL_ACCOUNT_FLAG")]
        public string PersonalAccountFlag { get; set; }

        /// <summary>
        /// Дата выписки
        /// </summary>
        [XmlAttribute("STATEMENTDATE")]
        public string StatementDate { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(TypeName = "DEBETDOCUMENT")]
    public class DebetDocument
    {
        [XmlAttribute("AMOUNT")]
        public string Amount { get; set; }

        [XmlAttribute("BUDGRECEIVERINN")]
        public string BudgReceiverInn { get; set; }

        [XmlAttribute("BUDGRECEIVERKPP")]
        public string BudgReceiverKpp { get; set; }

        [XmlAttribute("BUDGRECEIVERNAME")]
        public string BudgReceiverName { get; set; }

        [XmlAttribute("COMPDOCDATE")]
        public string CompDocDate { get; set; }

        /// <summary>
        /// Номер исполняющего документа
        /// Только, если исполняющим документом является плат. поручение.
        /// </summary>
        [XmlAttribute("COMPDOCNUMBER")]
        public string CompDocNumber { get; set; }

        [XmlAttribute("CONTRAGENTACCOUNTNUMBER")]
        public string ContragentAccountNumber { get; set; }

        [XmlAttribute("CONTRAGENTBANKBRANCHCODE")]
        public string ContragentBankBranchCode { get; set; }

        [XmlAttribute("CONTRAGENTBANKBRANCHNAME")]
        public string ContragentBankBranchName { get; set; }

        [XmlAttribute("CONTRAGENTBIC")]
        public string ContragentBic { get; set; }

        [XmlAttribute("CONTRAGENTID")]
        public string ContragentId { get; set; }

        [XmlAttribute("CONTRAGENTINN")]
        public string ContragentInn { get; set; }

        [XmlAttribute("CONTRAGENTKPP")]
        public string ContragentKpp { get; set; }

        [XmlAttribute("CONTRAGENTNAME")]
        public string ContragentName { get; set; }

        [XmlAttribute("CONTRAGENTOFKCODE")]
        public string ContragentOfkCode { get; set; }

        [XmlAttribute("CONTRAGENTOFKINN")]
        public string ContragentOfkInn { get; set; }

        [XmlAttribute("CONTRAGENTOFKKPP")]
        public string ContragentOfkKpp { get; set; }

        [XmlAttribute("CONTRAGENTOFKNAME")]
        public string ContragentOfkName { get; set; }

        [XmlAttribute("CONTRAGENTUFKACCOUNTNUMBER")]
        public string ContragentUfkAccountNumber { get; set; }

        [XmlAttribute("CONTRAGENTUFKINN")]
        public string ContragentUfkInn { get; set; }

        [XmlAttribute("CONTRAGENTUFKKPP")]
        public string ContragentUfkKpp { get; set; }

        [XmlAttribute("CONTRAGENTUFKNAME")]
        public string ContragentUfkName { get; set; }

        [XmlAttribute("DOCCLASS")]
        public string DocClass { get; set; }

        [XmlAttribute("DOCCLASSID")]
        public string DocClassId { get; set; }

        [XmlAttribute("DOCUMENTDATE")]
        public string DocumentDate { get; set; }

        [XmlAttribute("DOCUMENTID")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [XmlAttribute("DOCUMENTNUMBER")]
        public string DocumentNumber { get; set; }

        [XmlAttribute("GROUND")]
        public string Ground { get; set; }

        [XmlAttribute("KCSR")]
        public string Kcsr { get; set; }

        [XmlAttribute("KDE")]
        public string Kde { get; set; }

        [XmlAttribute("KDF")]
        public string Kdf { get; set; }

        [XmlAttribute("KDR")]
        public string Kdr { get; set; }

        [XmlAttribute("KESR")]
        public string Kesr { get; set; }

        [XmlAttribute("KFSR")]
        public string Kfsr { get; set; }

        [XmlAttribute("KIF")]
        public string Kif { get; set; }

        [XmlAttribute("KVR")]
        public string Kvr { get; set; }

        [XmlAttribute("KVSR")]
        public string Kvsr { get; set; }

        [XmlAttribute("OPERKIND")]
        public string OperKind { get; set; }

        [XmlAttribute("OPERKINDLS")]
        public string OperKindLs { get; set; }

        [XmlAttribute("PAYIDENTDOCUMENTDATE")]
        public string PayIdentDocumentDate { get; set; }

        [XmlAttribute("PAYIDENTDOCUMENTNUMBER")]
        public string PayIdentDocumentNumber { get; set; }

        [XmlAttribute("PAYIDENTGROUND")]
        public string PayIdentGround { get; set; }

        /// <summary>
        /// Код доходов Админ. - Классификатор администраторов поступлений и выбытий (доходы)
        /// </summary>
        [XmlAttribute("PAYIDENTKODDOHKADMD")]
        public string PayIdentKodDohkAdmD { get; set; }

        [XmlAttribute("PAYIDENTKODDOHKD")]
        public string PayIdentKodDohKd { get; set; }

        [XmlAttribute("PAYIDENTKODDOHKESD")]
        public string PayIdentKodDohKesD { get; set; }

        [XmlAttribute("PAYIDENTOKATO")]
        public string PayIdentOkato { get; set; }

        [XmlAttribute("PAYIDENTPERIOD")]
        public string PayIdentPeriod { get; set; }

        [XmlAttribute("PAYIDENTSTATUS")]
        public string PayIdentStatus { get; set; }

        [XmlAttribute("PAYIDENTTYPE")]
        public string PayidentType { get; set; }

        [XmlAttribute("PAYKIND")]
        public string PayKind { get; set; }

        [XmlAttribute("QUEUEKIND")]
        public string QueueKind { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(TypeName = "CREDITDOCUMENT")]
    public class CreditDocument
    {
        [XmlAttribute("AMOUNT")]
        public string Amount { get; set; }

        [XmlAttribute("BUDGRECEIVERINN")]
        public string BudgReceiverInn { get; set; }

        [XmlAttribute("BUDGRECEIVERKPP")]
        public string BudgReceiverKpp { get; set; }

        [XmlAttribute("BUDGRECEIVERNAME")]
        public string BudgReceiverName { get; set; }

        [XmlAttribute("COMPDOCDATE")]
        public string CompDocDate { get; set; }

        [XmlAttribute("COMPDOCNUMBER")]
        public string CompDocNumber { get; set; }

        [XmlAttribute("CONTRAGENTACCOUNTNUMBER")]
        public string ContragentAccountNumber { get; set; }

        [XmlAttribute("CONTRAGENTBANKBRANCHCODE")]
        public string ContragentBankBranchCode { get; set; }

        [XmlAttribute("CONTRAGENTBANKBRANCHNAME")]
        public string ContragentBankBranchName { get; set; }

        [XmlAttribute("CONTRAGENTBIC")]
        public string ContragentBic { get; set; }

        [XmlAttribute("CONTRAGENTID")]
        public string ContragentId { get; set; }

        [XmlAttribute("CONTRAGENTINN")]
        public string ContragentInn { get; set; }

        [XmlAttribute("CONTRAGENTKPP")]
        public string ContragentKpp { get; set; }

        [XmlAttribute("CONTRAGENTNAME")]
        public string ContragentName { get; set; }

        [XmlAttribute("CONTRAGENTOFKCODE")]
        public string ContragentOfkCode { get; set; }

        [XmlAttribute("CONTRAGENTOFKINN")]
        public string ContragentOfkInn { get; set; }

        [XmlAttribute("CONTRAGENTOFKKPP")]
        public string ContragentOfkKpp { get; set; }

        [XmlAttribute("CONTRAGENTOFKNAME")]
        public string ContragentOfkName { get; set; }

        [XmlAttribute("CONTRAGENTUFKACCOUNTNUMBER")]
        public string ContragentUfkAccountNumber { get; set; }

        [XmlAttribute("CONTRAGENTUFKINN")]
        public string ContragentUfkInn { get; set; }

        [XmlAttribute("CONTRAGENTUFKKPP")]
        public string ContragentUfkKpp { get; set; }

        [XmlAttribute("CONTRAGENTUFKNAME")]
        public string ContragentUfkName { get; set; }

        [XmlAttribute("DOCCLASS")]
        public string DocClass { get; set; }

        [XmlAttribute("DOCCLASSID")]
        public string DocClassId { get; set; }

        [XmlAttribute("DOCUMENTDATE")]
        public string DocumentDate { get; set; }

        [XmlAttribute("DOCUMENTID")]
        public string DocumentId { get; set; }

        [XmlAttribute("DOCUMENTNUMBER")]
        public string DocumentNumber { get; set; }

        [XmlAttribute("GROUND")]
        public string Ground { get; set; }

        [XmlAttribute("KCSR")]
        public string Kcsr { get; set; }

        [XmlAttribute("KDE")]
        public string Kde { get; set; }

        [XmlAttribute("KDF")]
        public string Kdf { get; set; }

        /// <summary>
        /// Доп. КР
        /// </summary>
        [XmlAttribute("KDR")]
        public string Kdr { get; set; }

        [XmlAttribute("KESR")]
        public string Kesr { get; set; }

        [XmlAttribute("KFSR")]
        public string Kfsr { get; set; }

        [XmlAttribute("KIF")]
        public string Kif { get; set; }

        [XmlAttribute("KVR")]
        public string Kvr { get; set; }

        [XmlAttribute("KVSR")]
        public string Kvsr { get; set; }

        [XmlAttribute("OPERKIND")]
        public string OperKind { get; set; }

        [XmlAttribute("OPERKINDLS")]
        public string OperKindLs { get; set; }

        [XmlAttribute("PAYKIND")]
        public string PayKind { get; set; }

        [XmlAttribute("QUEUEKIND")]
        public string QueueKind { get; set; }

        [XmlAttribute("PAYIDENTDOCUMENTDATE")]
        public string PayIdentDocumentDate { get; set; }

        [XmlAttribute("PAYIDENTDOCUMENTNUMBER")]
        public string PayIdentDocumentNumber { get; set; }

        [XmlAttribute("PAYIDENTGROUND")]
        public string PayIdentGround { get; set; }

        [XmlAttribute("PAYIDENTKODDOHKADMD")]
        public string PayIdentKodDohkAdmD { get; set; }

        [XmlAttribute("PAYIDENTKODDOHKD")]
        public string PayIdentKodDohKd { get; set; }

        [XmlAttribute("PAYIDENTKODDOHKESD")]
        public string PayIdentKodDohKesD { get; set; }

        [XmlAttribute("PAYIDENTOKATO")]
        public string PayIdentOkato { get; set; }

        [XmlAttribute("PAYIDENTPERIOD")]
        public string PayIdentPeriod { get; set; }

        [XmlAttribute("PAYIDENTSTATUS")]
        public string PayIdentStatus { get; set; }

        [XmlAttribute("PAYIDENTTYPE")]
        public string PayIdentType { get; set; }
    }
}
