namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Properties;

    using Castle.Windsor;

    public class WeeklyDisclosureInfoReport : BasePrintForm
    {
        private List<long> municipalities;

        private int DiStarted;

        private int DiFull;

        private long periodId;

        private bool? transsferredManag;

        public IWindsorContainer Container { get; set; }

        public WeeklyDisclosureInfoReport() : base(new ReportTemplateBinary(Resources.WeeklyDiReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Еженедельный отчет по раскрытию информации";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Еженедельный отчет по раскрытию информации";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие информации";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.WeeklyDI";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.WeeklyDI";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params["municipalityIds"].ToString();
            municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                                 ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                                 : new List<long>();
            DiStarted = baseParams.Params.GetAs<int>("diStarted");
            DiFull = baseParams.Params.GetAs<int>("diFull");
            periodId = baseParams.Params.GetAs<long>("period");
            transsferredManag = baseParams.Params["transsferredManag"].To<bool?>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var period = Container.Resolve<IDomainService<PeriodDi>>().GetAll().FirstOrDefault(x => x.Id == periodId);

            var transferredMo = Container.Resolve<IDomainService<ManOrgJskTsjContract>>()
                         .GetAll()
                         .Where(x => x.IsTransferredManagement == YesNoNotSet.Yes)
                         .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                         .Where(x =>
                                ((x.StartDate.HasValue && period.DateStart.HasValue
                                  && (x.StartDate.Value >= period.DateStart.Value)
                                  || !period.DateStart.HasValue)
                                 && (x.StartDate.HasValue && period.DateEnd.HasValue && (period.DateEnd.Value >= x.StartDate.Value) || !period.DateEnd.HasValue))
                                || ((x.StartDate.HasValue && period.DateStart.HasValue && (period.DateStart.Value >= x.StartDate.Value) || !x.StartDate.HasValue)
                                    && (x.StartDate.HasValue && period.DateEnd.HasValue
                                        && (x.EndDate.Value >= period.DateStart.Value)
                                        || !x.EndDate.HasValue)))
                         .Select(x => x.ManagingOrganization.Id)
                         .Distinct()
                         .ToList();

            var listMuManOrg = Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                         .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Contragent.Municipality.Id))
                         .Where(x => x.TypeManagement != TypeManagementManOrg.Other && ((x.ActivityDateEnd.HasValue && x.ActivityDateEnd > period.DateStart)
                              || (!x.ActivityDateEnd.HasValue && x.ActivityGroundsTermination == GroundsTermination.NotSet)))
                         .Select(x => new
                         {
                             IdMun = x.Contragent.Municipality.Id,
                             MuName = x.Contragent.Municipality.Name, 
                             IdManOrg = x.Id, 
                             NameManOrg = x.Contragent.Name,
                             typeManOrg = x.TypeManagement
                         })
                         .ToList()
                         .AsQueryable()
                          .WhereIf(transsferredManag == false, x => !transferredMo.Contains(x.IdManOrg))
                          .ToList();

            var listManOrg = listMuManOrg.Select(x => x.IdManOrg).ToList();
            
            var start = 1000;
            
            var serviceDisclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>();

            var tmpFirstId = listManOrg.Take(start).ToArray();
            var ManOrgDiList = serviceDisclosureInfo.GetAll()
                         .Where(x => tmpFirstId.Contains(x.ManagingOrganization.Id) && x.PeriodDi.Id == periodId)
                         .Select(x => new { IdManOrg = x.ManagingOrganization.Id, IdDi = x.Id })
                         .ToList();

            while (start < listManOrg.Count)
            {
                var tmpId = listManOrg.Skip(start).Take(1000).ToArray();
                ManOrgDiList.AddRange(serviceDisclosureInfo.GetAll()
                             .Where(x => tmpId.Contains(x.ManagingOrganization.Id) && x.PeriodDi.Id == periodId)
                             .Select(x => new { IdManOrg = x.ManagingOrganization.Id, IdDi = x.Id }));

                start += 1000;
            }

            var dictDiscInfo = ManOrgDiList.GroupBy(x => x.IdManOrg)
                                           .ToDictionary(x => x.Key, x => x.Select(y => y.IdDi).First());

            var listDisclosureInfoIds = dictDiscInfo.Values.Distinct().ToArray();

            start = 1000;
            var firstId = listDisclosureInfoIds.Take(1000).ToArray();

            var serviceDisclosureInfoPercent = Container.Resolve<IDomainService<DisclosureInfoPercent>>();
            var listDiPercent = serviceDisclosureInfoPercent.GetAll()
                         .Where(x => x.Code == "DisclosureInfoPercentProvider" && firstId.Contains(x.DisclosureInfo.Id))
                         .Select(x => new { IdDIndo = x.DisclosureInfo.Id, x.Percent })
                         .ToList();

            while (start < listDisclosureInfoIds.Length)
            {
                var tmpId = listDisclosureInfoIds.Skip(start).Take(1000).ToArray();

                listDiPercent.AddRange(serviceDisclosureInfoPercent.GetAll()
                             .Where(x => x.Code == "DisclosureInfoPercentProvider" && tmpId.Contains(x.DisclosureInfo.Id))
                             .Select(x => new { IdDIndo = x.DisclosureInfo.Id, x.Percent })
                             .ToList());

                start += 1000;
            }

            var diPercentDict = listDiPercent.GroupBy(x => x.IdDIndo).ToDictionary(x => x.Key, x => x.Select(y => y.Percent).LastOrDefault());

            var dataDict = listMuManOrg.GroupBy(x => x.MuName).ToDictionary(x => x.Key, x => x.ToList());

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("SectionMu");
            var section = sectionMu.ДобавитьСекцию("SectionData");
            int num = 0;
            int startDisclosuresInfoTotal = 0;
            int endDisclosuresInfoTotal = 0;
            int tranferManOrgTotal = 0;

            foreach (var municipality in dataDict.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["name"] = municipality.Key;

                int startDisclosuresInfoMu = 0;
                int endDisclosuresInfoMu = 0;
                int tranferManOrg = 0;

                foreach (var manorg in municipality.Value)
                {
                    var percent = 0m;
                    if (dictDiscInfo.ContainsKey(manorg.IdManOrg))
                    {
                        var diId = dictDiscInfo[manorg.IdManOrg];
                        if (diPercentDict.ContainsKey(diId))
                        {
                            percent = diPercentDict[diId] != null ? diPercentDict[diId].ToDecimal() : 0M;
                        }
                    }

                    section.ДобавитьСтроку();
                    section["num"] = ++num;
                    section["MunicipalityName"] = manorg.MuName;
                    section["ManOrgName"] = manorg.NameManOrg;
                    section["startDisclosuresInfo"] = percent >= DiStarted ? 1 : 0;
                    section["endDisclosuresInfo"] = percent >= DiFull ? 1 : 0;
                    section["TypeManOrg"] = manorg.typeManOrg.GetEnumMeta().Display;
                    section["tranferManOrg"] = transferredMo.Contains(manorg.IdManOrg) ? "1" : "0";
                    tranferManOrg += transferredMo.Contains(manorg.IdManOrg) ? 1 : 0;

                    startDisclosuresInfoMu += percent >= DiStarted ? 1 : 0;
                    endDisclosuresInfoMu += percent >= DiFull ? 1 : 0;
                }                

                sectionMu["startDisclosuresInfo"] = startDisclosuresInfoMu;
                sectionMu["endDisclosuresInfo"] = endDisclosuresInfoMu;
                sectionMu["tranferManOrg"] = tranferManOrg;

                startDisclosuresInfoTotal += startDisclosuresInfoMu;
                endDisclosuresInfoTotal += endDisclosuresInfoMu;
                tranferManOrgTotal += tranferManOrg;
            }

            reportParams.SimpleReportParams["startDisclosuresInfo"] = startDisclosuresInfoTotal;
            reportParams.SimpleReportParams["endDisclosuresInfo"] = endDisclosuresInfoTotal;
            reportParams.SimpleReportParams["tranferManOrg"] = tranferManOrgTotal;
        }
    }
}