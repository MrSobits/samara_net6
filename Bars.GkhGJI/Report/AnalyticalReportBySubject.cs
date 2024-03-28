using System;
using System.Linq;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;

    using Bars.GkhGji.DomainService;

    public class AnalyticalReportBySubject : BasePrintForm
    {
        public AnalyticalReportBySubject()  : base(new ReportTemplateBinary(Properties.Resources.AnalyticalReportBySubject))
        {
        }
        public IAppealCitsService<ViewAppealCitizens> AppealCitsService { get; set; }

        public IDomainService<StatSubjectGji> StatSubjectDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppCitStatSubjectDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        private int year;

        public override string Name
        {
            get
            {
                return "Аналитический отчёт по тематике";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Аналитический отчёт по тематике";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.AnalyticalReportBySubject";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.AnalyticalReportBySubject";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            year = baseParams.Params.GetAs<int>("year");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Year"] = year;

            var statSubjects = StatSubjectDomain.GetAll()
                .OrderBy(x => x.Code)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList();

            var appCitStatSubj = this.AppealCitsService.FilterByActiveAppealCits(this.AppCitStatSubjectDomain.GetAll(), x => x.AppealCits.State)
                  .Where(x => x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value.Year == year)
                  .Select(x => new
                  {
                      x.Id,
                      SubjId = x.Subject.Id,
                      x.AppealCits.DateFrom,
                      AppCitId = x.AppealCits.Id
                  })
                  .AsEnumerable();

            var statSubjByQuarter = appCitStatSubj
                  .GroupBy(x => x.SubjId)
                  .ToDictionary(x => x.Key, y => new {
                          FirstQuarter = y.Count(x => x.DateFrom < new DateTime(year, 4, 1)),
                          SecondQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 4, 1) && x.DateFrom < new DateTime(year, 7, 1)),
                          ThirdQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 7, 1) && x.DateFrom < new DateTime(year, 10, 1)),
                          FouthQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 10, 1)),
                          AllCnt = y.Count()
                      });

            var appCitCntByQuarter = this.AppealCitsService.FilterByActiveAppealCits(this.AppealCitsDomain.GetAll(), x => x.State)
                  .Where(x => x.DateFrom.HasValue && x.DateFrom.Value.Year == year)
                  .Select(x => new
                  {
                      x.Id,
                      x.DateFrom
                  })
                  .ToList()
                  .Return(y => new
                  {
                          FirstQuarter = y.Count(x => x.DateFrom < new DateTime(year, 4, 1)),
                          SecondQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 4, 1) && x.DateFrom < new DateTime(year, 7, 1)),
                          ThirdQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 7, 1) && x.DateFrom < new DateTime(year, 10, 1)),
                          FouthQuarter = y.Count(x => x.DateFrom >= new DateTime(year, 10, 1)),
                  });

            var firstQuarterCount = statSubjByQuarter.Values.SafeSum(x => x.FirstQuarter);
            var secondQuarterCount = statSubjByQuarter.Values.SafeSum(x => x.SecondQuarter);
            var thirdQuarterCount = statSubjByQuarter.Values.SafeSum(x => x.ThirdQuarter);
            var fouthQuarterCount = statSubjByQuarter.Values.SafeSum(x => x.FouthQuarter);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var number = 1;
            foreach (var statSubject in statSubjects)
            {
                section.ДобавитьСтроку();
                section["Number"] = number++;
                section["StatSubject"] = statSubject.Name;

                var statSubjInfo = statSubjByQuarter.Get(statSubject.Id);
                section["Quarter1Count"] = statSubjInfo != null  ? statSubjInfo.FirstQuarter : 0;
                section["Quarter1Percent"] = statSubjInfo != null && firstQuarterCount > 0  ? (statSubjInfo.FirstQuarter.ToDecimal() / firstQuarterCount * 100).RoundDecimal(1) : 0;
                section["Quarter2Count"] = statSubjInfo != null ? statSubjInfo.SecondQuarter : 0;
                section["Quarter2Percent"] = statSubjInfo != null && secondQuarterCount > 0 ? (statSubjInfo.SecondQuarter.ToDecimal() / secondQuarterCount * 100).RoundDecimal(1) : 0;
                section["Quarter3Count"] = statSubjInfo != null ? statSubjInfo.ThirdQuarter : 0;
                section["Quarter3Percent"] = statSubjInfo != null && thirdQuarterCount > 0 ? (statSubjInfo.ThirdQuarter.ToDecimal() / thirdQuarterCount * 100).RoundDecimal(1) : 0;
                section["Quarter4Count"] = statSubjInfo != null ? statSubjInfo.FouthQuarter : 0;
                section["Quarter4Percent"] = statSubjInfo != null && fouthQuarterCount > 0 ? (statSubjInfo.FouthQuarter.ToDecimal() / fouthQuarterCount * 100).RoundDecimal(1) : 0;
            }

            reportParams.SimpleReportParams["Quarter1TotalCount"] = firstQuarterCount;
            reportParams.SimpleReportParams["Quarter2TotalCount"] = secondQuarterCount;
            reportParams.SimpleReportParams["Quarter3TotalCount"] = thirdQuarterCount;
            reportParams.SimpleReportParams["Quarter4TotalCount"] = fouthQuarterCount;
            reportParams.SimpleReportParams["Quarter1TotalPercent"] = firstQuarterCount == 0 ? 0 : 100;
            reportParams.SimpleReportParams["Quarter2TotalPercent"] = secondQuarterCount == 0 ? 0 : 100;
            reportParams.SimpleReportParams["Quarter3TotalPercent"] = thirdQuarterCount == 0 ? 0 : 100;
            reportParams.SimpleReportParams["Quarter4TotalPercent"] = fouthQuarterCount == 0 ? 0 : 100;


            reportParams.SimpleReportParams["Quarter1AppCitCount"] = appCitCntByQuarter.FirstQuarter;
            reportParams.SimpleReportParams["Quarter2AppCitCount"] = appCitCntByQuarter.SecondQuarter;
            reportParams.SimpleReportParams["Quarter3AppCitCount"] = appCitCntByQuarter.ThirdQuarter;
            reportParams.SimpleReportParams["Quarter4AppCitCount"] = appCitCntByQuarter.FouthQuarter; 
        }
    }
}
