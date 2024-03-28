using System.Xml.Linq;

namespace Bars.GkhGji.DomainService.GISGMP
{
    /// <summary>
    /// Класс с данными получателя
    /// Предполагается, что когда-то будет получаться из настроек, а пока захардкожено
    /// </summary>
    public static class Payee
    {
        static XNamespace comNamespace = @"http://roskazna.ru/gisgmp/xsd/Common/2.5.0";
        static XNamespace orgNamespace = @"http://roskazna.ru/gisgmp/xsd/Organization/2.5.0";

        //public static string Name { get; internal set; } = "УФК по Челябинской области (ГУ «ГЖИ Челябинской области», л/с 04692203780)";
        //public static string INN { get; internal set; } = "7451374918";
        //public static string KPP { get; internal set; } = "745301001";
        //public static string OGRN { get; internal set; } = "1147451012400";
        //public static string AccountNumber { get; internal set; } = "40101810400000010801";
        //public static string BankName { get; internal set; } = "ОТДЕЛЕНИЕ ЧЕЛЯБИНСК";
        //public static string BIK { get; internal set; } = "047501001";

        public static string Name { get; internal set; } = "УФК по Челябинской области (ГУ «ГЖИ Челябинской области», л/с 04692203780)";
        public static string INN { get; internal set; } = "7451374918";
        public static string KPP { get; internal set; } = "745101001";
        public static string OGRN { get; internal set; } = "1147451012400";
        public static string CorrespondentBankAccount { get; internal set; } = "40102810645370000062";
        public static string AccountNumber { get; internal set; } = "03100643000000016900";
        public static string BankName { get; internal set; } = "ОТДЕЛЕНИЕ ЧЕЛЯБИНСК БАНКА РОССИИ// УФК по Челябинской области г. Челябинск";
        public static string BIK { get; internal set; } = "017501500";
        //public static object BankAccountNumber { get; internal set; } = "40101810400000010801";

        public static XElement GetXml()
        {
            return new XElement(orgNamespace + "Payee",
                        new XAttribute("name", Name),
                        new XAttribute("inn", INN),
                        new XAttribute("kpp", KPP),
                        new XAttribute("ogrn", OGRN),
                        new XElement(comNamespace + "OrgAccount",
                            new XAttribute("accountNumber", AccountNumber),
                            new XElement(comNamespace + "Bank",
                                new XAttribute("name", BankName),
                                new XAttribute("bik", BIK),
                                new XAttribute("correspondentBankAccount", CorrespondentBankAccount)
                                )
                            )
                        );
        }
    }
}
