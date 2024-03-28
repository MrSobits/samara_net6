namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class CountWorkCrReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programmCrId;

        private List<long> municipalityIds;

        private List<long> finSourceListId;

        private List<long> typeWorkIds;

        public CountWorkCrReport()
            : base(new ReportTemplateBinary(Properties.Resources.CountWorkCrReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Отчет по количеству работ в программе капремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по количеству работ в программе капремонта";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты КР";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CountWorkCrReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CountWorkCrReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsString = baseParams.Params["municipalityIds"].ToString();
            municipalityIds = string.IsNullOrEmpty(municipalityIdsString)
                                     ? new List<long>()
                                     : municipalityIdsString.Split(',').Select(x => x.ToLong()).ToList();

            this.programmCrId = baseParams.Params["programCrId"].ToLong();

            var finSourceIdsString = baseParams.Params["finSources"].ToString();
            this.finSourceListId = string.IsNullOrEmpty(finSourceIdsString)
                                  ? new List<long>()
                                  : finSourceIdsString.Split(',').Select(x => x.ToLong()).ToList();

            var strTypeWorkIds = baseParams.Params.GetAs<string>("typeWorks");

            this.typeWorkIds = !string.IsNullOrEmpty(strTypeWorkIds)
                ? strTypeWorkIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var codeList = this.Container.Resolve<IDomainService<Work>>().GetAll()
                .WhereIf(this.typeWorkIds.Count > 0, x => this.typeWorkIds.Contains(x.Id))
                .Where(x => x.TypeWork == TypeWork.Work)
                .Select(x => new { x.Code, x.Consistent185Fz, x.Name })
                .AsEnumerable()
                .OrderBy(x => x.Name)
                .GroupBy(x => x.Consistent185Fz)
                .ToDictionary(x => x.Key, x => x.Select(y => new WorkCrProxy { Code = y.Code, Name = y.Name }).ToList());

            // создание и заполнение вертикальных секций
            var vSection185Fz = reportParams.ComplexReportParams.ДобавитьСекцию("section185fz");
            var vSectionNo185Fz = reportParams.ComplexReportParams.ДобавитьСекцию("sectionNo185fz");

            var works185Fz = codeList.ContainsKey(true) ? codeList[true] : new List<WorkCrProxy>();
            var worksNo185Fz = codeList.ContainsKey(false) ? codeList[false] : new List<WorkCrProxy>();
            var num = 0;
            this.FillVerticalSection(vSection185Fz, works185Fz, ref num);
            this.FillVerticalSection(vSectionNo185Fz, worksNo185Fz, ref num);

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programmCrId);
            reportParams.SimpleReportParams["Programm"] = program.Name;

            var municipalityGroupDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name, x.Group })
                .OrderBy(x => x.Name)
                .AsEnumerable()
                .GroupBy(x => x.Group ?? string.Empty)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Id, y => y.Name));

            var objectsCrCount = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programmCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    MuId = x.RealityObject.Municipality.Id,
                    x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Select(y => y.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            var worksCountMunicipality = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Select(x => new
                {
                    ProgramId = x.ObjectCr.ProgramCr.Id,
                    ObjectCrId = x.ObjectCr.Id,
                    MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                    FinSourceId = x.FinanceSource.Id,
                    WorkCode = x.Work.Code,
                    WorkId = x.Work.Id,
                    x.Work.TypeWork
                })
                .Where(x => x.ProgramId == this.programmCrId)
                .Where(x => x.TypeWork == TypeWork.Work)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.MunicipalityId))
                .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.FinSourceId))
                .WhereIf(this.typeWorkIds.Count > 0, x => this.typeWorkIds.Contains(x.WorkId))
                .Select(x => new
                {
                    x.MunicipalityId,
                    x.ObjectCrId,
                    x.WorkCode
                })
                .AsEnumerable()
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, 
                //брать нужно уникальные работы в рамках одного объекта кр
                    y => y.GroupBy(x => x.ObjectCrId)
                        .ToDictionary(x => x.Key, z => z.Select(x => x.WorkCode).Distinct()));

            var worksCount = worksCountMunicipality
                .Select(x => new
                {
                    MunicipalityId = x.Key,
                    Works = 
                        x.Value
                            .SelectMany(y => y.Value)
                            .GroupBy(y => y)
                            .ToDictionary(y => y.Key, y => y.Count())
                })
                .ToDictionary(x => x.MunicipalityId, y => y.Works);

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var sectionGroupMunicipality = sectionGroup.ДобавитьСекцию("sectionGroupMunicipality");
            var sectionNoGroupMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionNoGroupMunicipality");

            foreach (var group in municipalityGroupDict.OrderBy(x => x.Key))
            {
                var distinctRobjectsCount = group.Value.Where(x => objectsCrCount.ContainsKey(x.Key)).Sum(x => objectsCrCount[x.Key]);

                if (distinctRobjectsCount == 0)
                {
                    continue;
                }

                var section = sectionNoGroupMunicipality;
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sectionGroup.ДобавитьСтроку();
                    sectionGroup["Name"] = group.Key;
                    sectionGroup["ItogGroup"] = distinctRobjectsCount;
                    section = sectionGroupMunicipality;
                }

                foreach (var municipality in group.Value)
                {
                    var objectCrCount = objectsCrCount.ContainsKey(municipality.Key) ? objectsCrCount[municipality.Key] : 0;

                    if (objectCrCount == 0)
                    {
                        continue;
                    }

                    section.ДобавитьСтроку();
                    section["Rayon"] = municipality.Value;
                    section["HouseCount"] = objectCrCount;

                    if (worksCount.ContainsKey(municipality.Key))
                    {
                        var muWorkDict = worksCount[municipality.Key];

                        muWorkDict.ForEach(x => section[string.Format("param{0}", x.Key)] = x.Value);

                        section["ItogRab"] = muWorkDict.Values.Sum();
                        section["NaDom"] = objectCrCount > 0 ? Math.Floor(muWorkDict.Values.Sum().ToDecimal() / objectCrCount) : 0;
                    }
                }
            }

            var totalHouseCount = objectsCrCount.Sum(x => x.Value);
            reportParams.SimpleReportParams["IHouseCount"] = totalHouseCount;

            var allWorkCounList = worksCount.SelectMany(x => x.Value)
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Value));

            allWorkCounList.ForEach(x => reportParams.SimpleReportParams[string.Format("Iparam{0}", x.Key)] = x.Value);

            var allWorkSum = allWorkCounList.Sum(x => x.Value);
            reportParams.SimpleReportParams["IItogRab"] = allWorkSum;
            reportParams.SimpleReportParams["INaDom"] = totalHouseCount > 0 ? Math.Floor(allWorkSum.ToDecimal() / totalHouseCount) : 0;
        }

        private void FillVerticalSection(Section section, List<WorkCrProxy> works, ref int num)
        {
            foreach (var work in works)
            {
                ++num;
                section.ДобавитьСтроку();
                section["colNum"] = num;
                section["workName"] = work.Name;
                section["param"] = string.Format("$param{0}$", work.Code);
                section["Iparam"] = string.Format("$Iparam{0}$", work.Code);
            }
        }
    }

    internal class WorkCrProxy
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}