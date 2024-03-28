using System.Xml.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService.GISGMP
{
    /// <summary>
    /// Класс с данными получателя
    /// Предполагается, что когда-то будет получаться из настроек, а пока захардкожено
    /// </summary>
    public static class Payee
    {
        static XNamespace comNamespace = @"http://roskazna.ru/gisgmp/xsd/Common/2.5.0";
        static XNamespace orgNamespace = @"http://roskazna.ru/gisgmp/xsd/Organization/2.5.0";

        public static string Name { get; internal set; } = "Государственная жилищная инспекция Воронежской области";
        public static string INN { get; internal set; } = "3664032439";
        public static string KPP { get; internal set; } = "366401001";
        public static string OGRN { get; internal set; } = "1033600084968";
        public static string AccountNumber { get; internal set; } = "40102810945370000023";
        public static string OrgAccountNumber { get; internal set; } = "03100643000000013100";
        public static string BankName { get; internal set; } = "ОТДЕЛЕНИЕ ВОРОНЕЖ БАНКА РОССИИ//УФК по Воронежской области г. Воронеж";
        public static string BIK { get; internal set; } = "012007084";
        //public static object BankAccountNumber { get; internal set; } = "40101810400000010801";

        public static XElement GetXml()
        {
            return new XElement(orgNamespace + "Payee",
                       new XAttribute("name", Name),
                       new XAttribute("inn", INN),
                       new XAttribute("kpp", KPP),
                       new XAttribute("ogrn", OGRN),
                       new XElement(comNamespace + "OrgAccount",
                           new XAttribute("accountNumber", OrgAccountNumber),
                           new XElement(comNamespace + "Bank",
                               new XAttribute("name", BankName),
                               new XAttribute("bik", BIK),
                               new XAttribute("correspondentBankAccount", AccountNumber)
                               )
                           )
                       );
        }
    }
}
