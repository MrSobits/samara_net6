namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;    
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using Entities;
    using Enums;

    public class ShortProgramByTypeWork : BasePrintForm
    {
        #region Dependency injection members

        public IWindsorContainer Container { get; set; }

        public IDomainService<ShortProgramRealityObject> ShortProgRealObjDomain { get; set; }

        public IDomainService<ShortProgramRecord> ShortProgRecordDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private List<long> typeWorkIds;

        public ShortProgramByTypeWork()
            : base(new ReportTemplateBinary(Properties.Resources.ShortProgramByTypeWork))
        {

        }

        public override string Name
        {
            get
            {
                return "Отчет по краткосрочной программе";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по краткосрочной программе";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ShortProgramByTypeWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.ShortProgramByTypeWork";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                                  ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                                  : new List<long>();

            var strTypeWorkIds = baseParams.Params.GetAs<string>("typeWorks");

            typeWorkIds = !string.IsNullOrEmpty(strTypeWorkIds)
                ? strTypeWorkIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            dateTimeReport = date.HasValue ? date.Value : DateTime.Now.Date;
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realObjsByMu =
                ShortProgRealObjDomain.GetAll()
                                      .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                      .Where(x => x.ProgramVersion.IsMain)
                                      .Select(
                                          x =>
                                          new
                                              {
                                                  x.Id,
                                                  x.RealityObject.Address,
                                                  MuId = x.RealityObject.Municipality.Id,
                                                  MuName = x.RealityObject.Municipality.Name
                                              })
                                      .AsEnumerable()
                                      .OrderBy(x => x.MuName)
                                      .GroupBy(x => x.MuId)
                                      .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Address));

            var works =
                ShortProgRecordDomain.GetAll()
                                     .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.ShortProgramObject.RealityObject.Municipality.Id))
                                     .WhereIf(typeWorkIds.Any(), x => typeWorkIds.Contains(x.Work.Id))
                                     .Where(x => x.ShortProgramObject.ProgramVersion.IsMain)
                                     .Select(x => new
                                                      {
                                                          x.ShortProgramObject.Id, 
                                                          WorkName = x.Work.Name, 
                                                          x.Cost,
                                                          WorkId = x.Work.Id,
                                                          IsWork = x.Work.TypeWork == TypeWork.Work
                                                      })
                                     .AsEnumerable()
                                     .GroupBy(x => x.Id)
                                     .ToDictionary(x => x.Key);


            reportParams.SimpleReportParams["ReportDate"] = dateTimeReport.ToShortDateString();
            reportParams.SimpleReportParams["RegionSum"] = works.Values.Sum(x => x.Sum(y => y.Cost.RoundDecimal(2)));
            reportParams.SimpleReportParams["SubjectRoCnt"] = realObjsByMu.Values.Select(x => x.Count()).Sum();
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            var colInd = 1;
            foreach (var realObjByMu in realObjsByMu)
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["Municipality"] = realObjByMu.Value.Select(x => x.MuName).First();

                var sectionRealObj = sectionMu.ДобавитьСекцию("sectionRealObj");

                var muWorksSum = 0M;
                foreach (var realObjData in realObjByMu.Value)
                {
                    if (works.ContainsKey(realObjData.Id))
                    {
                        var tempWorks = works[realObjData.Id];
                        sectionRealObj.ДобавитьСтроку();
                        var sectionWork = sectionRealObj.ДобавитьСекцию("sectionWork");

                        foreach (var work in tempWorks)
                        {
                            sectionWork.ДобавитьСтроку();
                            sectionWork["Number"] = colInd++;
                            sectionRealObj["Address"] = realObjData.Address;

                            muWorksSum += work.Cost.RoundDecimal(2);

                            sectionWork["Sum"] = work.Cost.RoundDecimal(2);
                            sectionWork["TypeWorks"] = work.WorkName;
                        }

                        sectionRealObj["RoWorkCnt"] = tempWorks.Where(x => x.IsWork).Select(x => x.WorkId).Distinct().Count();
                        sectionRealObj["RoSum"] = tempWorks.Sum(x => x.Cost.RoundDecimal(2));
                    }
                }

                sectionMu["MunicipalitySum"] = muWorksSum;
                sectionMu["MunicipalityRoCnt"] = realObjByMu.Value.Count();
            }
        }
    }
}