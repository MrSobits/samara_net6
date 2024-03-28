namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Отчет Распределение домов по месяцам завершения КР
    /// </summary>
    public class RealObjByMonthlyCr : BasePrintForm
    {
        private long[] municipalityIds;
        private long programCrId;
        private long[] financialSourceIds;

        public RealObjByMonthlyCr()
            : base(new ReportTemplateBinary(Properties.Resources.RealObjByMonthlyCr))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.RealObjByMonthlyCr";
            }
        }

        public override string Name
        {
            get { return "Распределение домов по месяцам завершения КР"; }
        }

        public override string Desciption
        {
            get { return "Распределение домов по месяцам завершения КР"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RealObjByMonthlyCr"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                      ? baseParams.Params["municipalityIds"].ToString()
                      : string.Empty;

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var financialSourceIdStr = baseParams.Params["financialSourceIds"].ToStr();
            financialSourceIds = string.IsNullOrEmpty(financialSourceIdStr) ? new long[0] : financialSourceIdStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {

            var countRealObjMoByMonthlyList = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                          .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                          .WhereIf(this.financialSourceIds.Length > 0, x => this.financialSourceIds.Contains(x.FinanceSource.Id))
                          .Where(x => x.ObjectCr.ProgramCr.Id == programCrId
                              && x.Work.TypeWork == TypeWork.Work
                              && x.DateStartWork.HasValue
                              && x.DateEndWork.HasValue)
                          .Select(x => new
                          {
                              moId = x.ObjectCr.RealityObject.Municipality.Id,
                              moName = x.ObjectCr.RealityObject.Municipality.Name,
                              realObjId = x.ObjectCr.RealityObject.Id,
                              dateEnd = x.DateEndWork ?? DateTime.MinValue
                          })
                          .ToList();

            var countRealObjByMo = countRealObjMoByMonthlyList
                        .GroupBy(x => x.moName)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.realObjId).Distinct().Count());

            var countRealObjMoByMonthly = countRealObjMoByMonthlyList
                                          .Distinct()
                                          .GroupBy(x => x.moName)
                                          .ToDictionary(x => x.Key,
                                              x => x.GroupBy(y => y.realObjId)
                                               .ToDictionary(y => y.Key, y => y.OrderByDescending(z => z.dateEnd).FirstOrDefault())
                                               .ToList());

            reportParams.SimpleReportParams["ProgramCR"] = Container.Resolve<IDomainService<ProgramCr>>().Get(programCrId).Name;
 
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var totalCountRealObj = 0;
            var countMunicipality = 0;
            var totalCountRealObjMonthly = new int[12];

            foreach (var item in countRealObjByMo.OrderBy(x => x.Key))
            {
                section.ДобавитьСтроку();
                section["NN"] = ++countMunicipality;
                section["Municipality"] = item.Key;
                section["CountRealObjBy"] = item.Value;
                totalCountRealObj += item.Value;
                
                var month = new int[12];
                foreach (var realObj in countRealObjMoByMonthly[item.Key])
                {
                    var numMonth = realObj.Value.dateEnd.Month.ToInt();
                     month[numMonth - 1] += 1;
                }

                for (int i = 1; i <= 12; ++i)
                {
                    if (month[i - 1] != 0)
                    {
                        section["Month" + i] = month[i - 1];
                        totalCountRealObjMonthly[i - 1] += month[i - 1];
                    }                  
                }
            }

            reportParams.SimpleReportParams["TotalCountRealObj"] = totalCountRealObj;
            for (var i = 1; i <= 12; ++i)
            {
                reportParams.SimpleReportParams["TotalMonth" + i] = totalCountRealObjMonthly[i - 1];
            }
        }
    }
}
