namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetManOrgInfoResponse")]
    public class GetManOrgInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Periods")]
        public DiPeriod[] Period { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "DiPeriod")]
    public class DiPeriod
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PeriodCode")]
        public long PeriodCode { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ManOrg")]
        public ManOrg ManOrg { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "ManOrg")]
    public class ManOrg
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "JurAddress")]
        public string JurAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FioDirector")]
        public string FioDirector { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OGRN")]
        public string Ogrn { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RegYear")]
        public string RegYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OgrnRegistration")]
        public string OgrnRegistration { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MailAddress")]
        public string MailAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Phone")]
        public string Phone { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DispatcherPhone")]
        public string DispatcherPhone { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Twitter")]
        public string Twitter { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguRegNumber")]
        public string FrguRegNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguOrgNumber")]
        public string FrguOrgNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguServiceNumber")]
        public string FrguServiceNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OfficialWebsite")]
        public string OfficialWebsite { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactAddress")]
        public string FactAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RevComMember")]
        public string RevComMember { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DirectionMember")]
        public string DirectionMember { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeManagment")]
        public string TypeManagment { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CurrentDirector")]
        public CurrentDirector CurrentDirector { get; set; }

        [DataMember]
        [XmlElement(ElementName = "WorkMode")]
        public WorkMode WorkMode { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ReceptionCitizens")]
        public WorkMode ReceptionCitizens { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DispatcherWork")]
        public WorkMode DispatcherWork { get; set; }

        [DataMember]
        [XmlElement(ElementName = "TermContracts")]
        public TermContracts TermContracts { get; set; }

        [DataMember]
        [XmlElement(ElementName = "MembershipUnions")]
        public MembershipUnions MembershipUnions { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FinanceActivity")]
        public FinanceActivity FinanceActivity { get; set; }

        [DataMember]
        [XmlElement(ElementName = "BaseIndicators")]
        public BaseIndicators BaseIndicators { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RealityObjectList")]
        public RealityObjs Houses { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OrganizationForm")]
        public string OrganizationForm { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "INN")]
        public string INN { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Fax")]
        public string Fax { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DispatcherAdress")]
        public string DispatcherAdress { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Charter")]
        public File Charter { get; set; }

        [DataMember]
        [XmlElement(ElementName = "License")]
        public License License { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Rating")]
        public Rating Rating { get; set; }
    }


    [DataContract]
    [XmlType(TypeName = "CurrentDirector")]
    public class CurrentDirector
    {
        [DataMember]
        [XmlAttribute(AttributeName = "FIO")]
        public string Fio { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Position")]
        public string Position { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Phone")]
        public string Phone { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Day")]
    public class Day
    {
        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartTime { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishTime { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Pause")]
        public string Pause { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AroundClock")]
        public string AroundClock { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "WorkMode")]
    public class WorkMode
    {
        [DataMember]
        [XmlElement(ElementName = "Monday")]
        public Day Monday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Tuesday")]
        public Day Tuesday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Wednesday")]
        public Day Wednesday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Thursday")]
        public Day Thursday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Friday")]
        public Day Friday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Saturday")]
        public Day Saturday { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Sunday")]
        public Day Sunday { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "TermContracts")]
    public class TermContracts
    {
        [DataMember]
        [XmlAttribute(AttributeName = "NoTerminate")]
        public string NoTerminate { get; set; }

        [DataMember]
        [XmlElement(ElementName = ("TermContract"))]
        public TermContract[] Contracts { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "TermContract")]
    public class TermContract
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Reason")]
        public string Reason { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "MembershipUnions")]
    public class MembershipUnions
    {
        [DataMember]
        [XmlElement(ElementName = "MembershipUnions")]
        public Union[] Unions { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Union")]
    public class Union
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OfficialWebsite")]
        public string OfficialWebsite { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "FinanceActivity")]
    public class FinanceActivity
    {
        [DataMember]
        [XmlElement(ElementName = "BookBalance")]
        public BaseDoc BookBalance { get; set; }

        [DataMember]
        [XmlElement(ElementName = "BookBalanceAnnex")]
        public BaseDoc BookBalanceAnnex { get; set; }

        [DataMember]
        [XmlElement(ElementName = "EstimateCurrentYear")]
        public BaseDoc EstimateCurrentYear { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "EstimatePrevYear")]
        public BaseDoc EstimatePrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ReportEstimatePrevYear")]
        public BaseDoc ReportEstimatePrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RevComCurrentYear")]
        public BaseDoc RevComCurrentYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RevComPrevYear")]
        public BaseDoc RevComPrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RevComPrevPrevYear")]
        public BaseDoc RevComPrevPrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditCurrentYear")]
        public BaseDoc AuditCurrentYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditPrevYear")]
        public BaseDoc AuditPrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditPrevPrevYear")]
        public BaseDoc AuditPrevPrevYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IncomeAndExpenses")]
        public FinActivityRealObjs IncomeAndExpenses { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FundsInfo")]
        public InfoFunds FundsInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Utilities")]
        public Utilities Utilities { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "FinActivityRealObjs")]
    public class FinActivityRealObjs
    {
        [DataMember]
        [XmlAttribute(AttributeName = "PresentedToRepay")]
        public string PresentedToRepay { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ReceivedProvidedService")]
        public string ReceivedProvidedService { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumDebt")]
        public string SumDebt { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumFactExpense")]
        public string SumFactExpense { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumIncome")]
        public decimal SumIncome { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Object")]
        public FinActivityRealObj[] FinActRealObjs { get; set; }
    }
     
    [DataContract]
    [XmlType(TypeName = "Object")]
    public class FinActivityRealObj
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaLiving")]
        public string AreaLiving { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaMkd")]
        public string AreaMkd { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PresentedToRepay")]
        public string PresentedToRepay { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ReceivedProvidedService")]
        public string ReceivedProvidedService { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumDebt")]
        public string SumDebt { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumFactExpense")]
        public string SumFactExpense { get; set; }

        [DataMember]
        [XmlIgnore]
        public decimal PresentedToRepayDec { get; set; }

        [DataMember]
        [XmlIgnore]
        public decimal ReceivedProvidedServiceDec { get; set; }

        [DataMember]
        [XmlIgnore]
        public decimal SumDebtDec { get; set; }

        [DataMember]
        [XmlIgnore]
        public decimal SumFactExpenseDec { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumIncome")]
        public decimal SumIncome { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "InfoFunds")]
    public class InfoFunds
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Payment")]
        public string Payment { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FundItem")]
        public InfoFund[] Funds { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "FundItem")]
    public class InfoFund
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Size")]
        public string Size { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AsOf")]
        public string Date { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "BaseIndicators")]
    public class BaseIndicators
    {
        [DataMember]
        [XmlElement(ElementName = "TermsOfService")]
        public TermsOfService TermsOfService { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "TermsOfService")]
    public class TermsOfService
    {
        [DataMember]
        [XmlElement(ElementName = "ProjectContract")]
        public BaseDoc ProjectContract { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ListCommunalService")]
        public BaseDoc ListCommunalService { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ListServiceApartment")]
        public BaseDoc ListServiceApartment { get; set; }

        [DataMember]
        [XmlElement(ElementName = "InfoAdminResponsibility")]
        public InfoAdminResponsibility InfoAdminResponsibility { get; set; }
    }


    [DataContract]
    [XmlType(TypeName = "BaseDoc")]
    public class BaseDoc
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long IdFile { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameFile")]
        public string NameFile { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NotAvailable")]
        public string NotAvailable { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "State")]
        public string State { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "InfoAdminResponsibility")]
    public class InfoAdminResponsibility
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IsAdminResponsibility")]
        public string IsAdminResponsibility { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AdminResponsibility")]
        public AdminResponsibility[] AdminResponsibilities { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "AdminResponsibility")]
    public class AdminResponsibility
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ControlOrg")]
        public string ControlOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ViolCount")]
        public string ViolCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FineDate")]
        public string FineDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumFine")]
        public string SumFine { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DatePayFine")]
        public string DatePayFine { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Arrangements")]
        public string Arrangements { get; set; }

        [DataMember]
        [XmlElement(ElementName = "File")]
        public BaseDoc File { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "RealityObjs")]
    public class RealityObjs
    {
        [DataMember]
        [XmlAttribute(AttributeName = "AreaMkd")]
        public string AreaMkd { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RealityObject")]
        public RealityObj[] RealityObjList { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "RealityObject")]
    public class RealityObj
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Municipality")]
        public string Municipality { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaMkd")]
        public string AreaMkd { get; set; }

        [DataMember]
        [XmlIgnore]
        public decimal AreaMkdDec { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "GkhCode")]
        public string GkhCode { get; set; }
    }

    [DataContract, XmlType(TypeName = "License")]
    public class License
    {
        [DataMember]
        [XmlAttribute(AttributeName = "NumLicense")]
        public string NumLicense { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DataLicense")]
        public string DataLicense { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OrgLicense")]
        public string OrgLicense { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocLicense")]
        public File DocLicense { get; set; }
    }

    [DataContract, XmlType(TypeName = "Rating")]
    public class Rating
    {
        [DataMember]
        [XmlAttribute(AttributeName = "WorkersCount")]
        public string WorkersCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ProportionSf")]
        public string ProportionSf { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ProportionMo")]
        public string ProportionMo { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StaffRegularAdministrative")]
        public string StaffRegularAdministrative { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StaffRegularEngineers")]
        public string StaffRegularEngineers { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StaffRegularLabor")]
        public string StaffRegularLabor { get; set; }
    }

    [DataContract, XmlType(TypeName = "Utilities")]
    public class Utilities
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IncomeProvision")]
        public string IncomeProvision { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Paid")]
        public string Paid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsPopulationAtBeginningPeriod")]
        public string DebtsPopulationAtBeginningPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsPopulationAtEndPeriod")]
        public string DebtsPopulationAtEndPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsForUtilities")]
        public string DebtsForUtilities { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaidIndicationsPU")]
        public string PaidIndicationsPU { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaidBillsNeed")]
        public string PaidBillsNeed { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaymentOfClaims")]
        public string PaymentOfClaims { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ServiceCommunPlat")]
        public ServiceCommunPlat[] ServiceCommunPlat { get; set; }
    }

    [DataContract, XmlType(TypeName = "ServiceCommunPlat")]
    public class ServiceCommunPlat
    {
        [DataMember]
        [XmlAttribute(AttributeName = "NameService")]
        public string NameService { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IncomeProvision")]
        public decimal IncomeProvision { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Paid")]
        public decimal Paid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsPopulationAtBeginningPeriod")]
        public decimal DebtsPopulationAtBeginningPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsPopulationAtEndPeriod")]
        public decimal DebtsPopulationAtEndPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DebtsForUtilities")]
        public decimal DebtsForUtilities { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaidIndicationsPU")]
        public decimal PaidIndicationsPU { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaidBillsNeed")]
        public decimal PaidBillsNeed { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaymentOfClaims")]
        public decimal PaymentOfClaims { get; set; }
    }

    [DataContract, XmlType(TypeName = "File")]
    public class File
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long IdFile { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Extension")]
        public string Extension { get; set; }
    }
}