using Bars.B4.Modules.NHibernateChangeLog;
using Bars.GkhDi.Entities;

namespace Bars.GkhDI.LogMap
{
    using System.Collections.Generic;

    using Bars.B4.Utils;

    public class DisclosureInfoPercentLogMap : AuditLogMap<DisclosureInfoPercent>
    {

        readonly Dictionary<string, string> dictNames = new Dictionary<string, string>()
                                                            {
                                                                { "DisclosureInfoPercentProvider", "Общий процент по раскрытию"},
                                                                { "GeneralDataPercent", "Раздел 'Общие сведения'"},
                                                                { "TerminateContractPercent", "Раздел 'Сведения о расторгнутых договорах'"},
                                                                { "MembershipUnionsPercent", "Раздел 'Членство в объединениях'"},
                                                                { "FinActivityPercent", "Раздел 'Финансовая деятельность'"},
                                                                { "FundsInfoPercent", "Раздел 'Сведения о фондах'"},
                                                                { "AdminResponsibilityPercent", "Раздел 'Административная ответственность'"},
                                                                { "DocumentsPercent", "Раздел 'Документы'"},
                                                                { "InfoOnContrPercent", "Раздел 'Сведения о договорах'"},
                                                                { "RealObjsPercent", "Процент 'Объекты в управлении'"},
                                                                { "ManOrgInfoPercent", "Процент 'Сведения об УО'"}
                                                            };

        public DisclosureInfoPercentLogMap()
        {
            Name("Процент по Раскрытию информации по 731 ПП РФ");

            Description(x => string.Format("{0} - {1} - {2}", 
                x != null &&
                x.DisclosureInfo != null &&
                x.DisclosureInfo.ManagingOrganization != null &&
                x.DisclosureInfo.ManagingOrganization.Contragent != null ?
                x.DisclosureInfo.ManagingOrganization.Contragent.Name : string.Empty,
                dictNames.ContainsKey(x.Code) ? dictNames[x.Code] : x.Code,
                x.DisclosureInfo != null &&
                x.DisclosureInfo.PeriodDi != null ?
                x.DisclosureInfo.PeriodDi.Name: string.Empty));

            MapProperty(x => x.Percent, "Percent", "Процент");
        }
    }
}
