namespace Bars.Gkh.Regions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.ManOrg;
    using Bars.Gkh.Enums;
    using Castle.Windsor;

    /// <summary>
    /// Реестр сведений, предоставляемых ТСЖ
    /// </summary>
    public class TsjProvidedInfoReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> _municipalities;

        public TsjProvidedInfoReport()
            : base(new ReportTemplateBinary(Properties.Resources.TsjRegistry))
        {

        }

        public override void PrepareReport(ReportParams reportParams)
        {
            const string dateFormat = "dd.MM.yyyy";
            var manOrgDomain = Container.Resolve<IDomainService<ManagingOrganization>>();
            var tsjInfoDomain = Container.Resolve<IDomainService<ManagingOrgRegistry>>();
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("секция_данные");
            var ustavSection = reportParams.ComplexReportParams.ДобавитьСекцию("секция_устав");
            var protocolSection = reportParams.ComplexReportParams.ДобавитьСекцию("секция_протокол");
            var membersSection = reportParams.ComplexReportParams.ДобавитьСекцию("секция_члены_тсж");
            using (Container.Using(manOrgDomain, tsjInfoDomain))
            {

                var tsjs = manOrgDomain.GetAll()
                    .Where(x => x.TypeManagement == TypeManagementManOrg.TSJ)
                    .WhereIf(_municipalities.Any(),
                        x =>
                            x.Contragent != null && x.Contragent.Municipality != null &&
                            _municipalities.Contains(x.Contragent.Municipality.Id)).ToList();
                var tsjIds = tsjs.Select(t => t.Id).ToArray();

                var infoDict =
                    tsjInfoDomain.GetAll().Where(x => tsjIds.Contains(x.ManagingOrganization.Id))
                        .GroupBy(x => x.ManagingOrganization.Id)
                        .ToDictionary(group => group.Key, group => group.ToList());

                var ustavMaxCount = infoDict.Max(x => x.Value.Count(y => y.InfoType == TsjInfoType.Regulations));
                var protocolMaxCount = infoDict.Max(x => x.Value.Count(y => y.InfoType == TsjInfoType.CreationDecision));
                var membersMaxCount = infoDict.Max(x => x.Value.Count(y => y.InfoType == TsjInfoType.ParticipantsRegistry));


                for (var i = 0; i < protocolMaxCount; i++)
                {
                    protocolSection.ДобавитьСтроку();
                    protocolSection["Протокол"] = string.Format("$Протокол_{0}$", i);
                }

                for (var i = 0; i < membersMaxCount; i++)
                {
                    membersSection.ДобавитьСтроку();
                    membersSection["ЧленыТсж"] = string.Format("$ЧленыТсж_{0}$", i);
                }

                for (var i = 0; i < ustavMaxCount; i++)
                {
                    ustavSection.ДобавитьСтроку();
                    ustavSection["УставЕгрюл"] = string.Format("$УставЕгрюл_{0}$", i);
                    ustavSection["УставРег"] = string.Format("$УставРег_{0}$", i);
                }


                foreach (var tsj in tsjs)
                {
                    section.ДобавитьСтроку();
                    section["НомерДела"] = tsj.CaseNumber;
                    section["НаименованиеТсж"] = tsj.Contragent.Name;
                    section["Огрн"] = tsj.Contragent.Ogrn;
                    section["Инн"] = tsj.Contragent.Inn;
                    section["МО"] = tsj.Contragent.Municipality != null ? tsj.Contragent.Municipality.Name : string.Empty;
                    if (tsj.Contragent.FiasJuridicalAddress != null)
                    {
                        section["Индекс"] = tsj.Contragent.FiasJuridicalAddress.PostCode;
                        section["НаселенныйПункт"] = tsj.Contragent.FiasJuridicalAddress.PlaceName;
                        section["УлицаДом"] = string.Format("{0}, {1}",
                            tsj.Contragent.FiasJuridicalAddress.StreetName, tsj.Contragent.FiasJuridicalAddress.House);
                    }
                    if (tsj.Contragent.FiasFactAddress != null)
                    {
                        section["УлицаДомФакт"] = string.Format("{0}, {1}",
                            tsj.Contragent.FiasFactAddress.StreetName, tsj.Contragent.FiasFactAddress.House);
                    }

                    if (tsj.TsjHead != null)
                    {
                        section["ПредседательТсж"] = tsj.TsjHead.FullName;
                        section["ПредседательТсжКонтакт"] = string.Format("{0}, {1}", tsj.TsjHead.Phone,
                            tsj.TsjHead.Email);
                    }

                    if (infoDict.ContainsKey(tsj.Id))
                    {
                        var i = 0;
                        foreach (var reg in infoDict[tsj.Id].Where(x => x.InfoType == TsjInfoType.Regulations).OrderBy(x => x.InfoDate))
                        {
                            section[string.Format("УставЕгрюл_{0}", i)] = FormattedDate(reg.EgrulDate, dateFormat);
                            section[string.Format("УставРег_{0}", i)] = string.Format("{0}, {1}", FormattedDate(reg.InfoDate, dateFormat), reg.RegNumber);
                            i++;
                        }

                        var j = 0;
                        foreach (var dec in infoDict[tsj.Id].Where(x => x.InfoType == TsjInfoType.CreationDecision).OrderBy(x => x.InfoDate))
                        {
                            section[string.Format("Протокол_{0}", j)] = string.Format("{0}, {1}", FormattedDate(dec.InfoDate, dateFormat), dec.RegNumber);
                            j++;
                        }

                        var k = 0;
                        foreach (var part in infoDict[tsj.Id].Where(x => x.InfoType == TsjInfoType.ParticipantsRegistry).OrderBy(x => x.InfoDate))
                        {
                            section[string.Format("ЧленыТсж_{0}", k)] = string.Format("{0}, {1}", FormattedDate(part.InfoDate, dateFormat), part.RegNumber);
                            k++;
                        }
                    }
                }
            }
        }

        public override string Name
        {
            get { return "Реестр сведений, предоставляемых ТСЖ"; }
        }

        public override string Desciption
        {
            get { return "Реестр сведений, предоставляемых ТСЖ"; }
        }

        public override string GroupName
        {
            get
            {
                return "ГЖИ";
            }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.TsjProvidedInfo"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.TsjProvidedInfoReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {

            var municipalitiesParam = baseParams.Params.GetAs("municipalityIds", string.Empty);


            _municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                                 ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                                 : new List<long>();
        }

        private string FormattedDate(DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : string.Empty;
        }
    }
}
