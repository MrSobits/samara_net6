namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет сумма по видам работ объекта КР
    /// </summary>
    public class CrWorkTypeSumReport : BasePrintForm
    {
        private long[] municipalityIds;
        private long programCrId;
        private readonly List<string> psdWorks = new List<string> { "1018", "1019" };   // коды работ ПСД
        private readonly List<string> tehWorks = new List<string> { "1020" };           // коды работ Технадзора
        public IWindsorContainer Container { get; set; }

        public CrWorkTypeSumReport()
            : base(new ReportTemplateBinary(Properties.Resources.CrWorkTypeSum))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CrWorkTypeSum";
            }
        }

        public override string Name
        {
            get { return "Сумма по видам работ объекта КР"; }
        }

        public override string Desciption
        {
            get { return "Сумма по видам работ объекта КР"; }
        }

        public override string GroupName
        {
            get { return "Капитальный ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CrWorkTypeSum"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                      ? baseParams.Params["municipalityIds"].ToString()
                      : string.Empty;

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            var objectCrService = this.Container.Resolve<IDomainService<ObjectCr>>();
            var typeWorkCrService = this.Container.Resolve<IDomainService<TypeWorkCr>>();

            reportParams.SimpleReportParams["Program"] = programCr.Name;
            reportParams.SimpleReportParams["Date"] = DateTime.Today.ToShortDateString();

            var objectsCrQuery = objectCrService.GetAll()
                         .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Where(x => x.ProgramCr.Id == this.programCrId);

            var objectsCrOrderQuery = objectsCrQuery                     
                         .OrderBy(x => x.RealityObject.Municipality.Name)
                         .ThenBy(x => x.RealityObject.Address)
                         .Select(x => new
                         {
                             objCrId = x.Id,
                             x.RealityObject.Address,
                             muName = x.RealityObject.Municipality.Name,
                             stateName = x.State.Name,
                             accepted = x.DateAcceptCrGji ?? DateTime.MinValue,
                         });

            var objectsCrDict = objectsCrOrderQuery
                                    .AsEnumerable()
                                    .GroupBy(x => x.objCrId)
                                    .ToDictionary(x => x.Key,
                                                    x => new
                                                    {
                                                        address = x.Select(y => y.Address).FirstOrDefault(),
                                                        muName = x.Select(y => y.muName).FirstOrDefault(),
                                                        stateName = x.Select(y => y.stateName).FirstOrDefault(),
                                                        accepted = x.Select(y => y.accepted).FirstOrDefault()
                                                    });

            var objCrIdQuery = objectsCrQuery.Select(x => x.Id);

            var sumByTypeWorks =
                typeWorkCrService.GetAll()
                                 .Where(x => objCrIdQuery.Contains(x.ObjectCr.Id))
                                 .Select(x => new { x.ObjectCr.Id, x.Work.Code, TypeWork = (Nullable<TypeWork>) x.Work.TypeWork, sum = x.Sum ?? 0M })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key,
                                    x => new
                                    {
                                        allSum = x.Sum(y => y.sum).ToDecimal(), // сумма по всем работам
                                        psdSum = x.Where(y => psdWorks.Contains(y.Code) && y.TypeWork == TypeWork.Service).Sum(y => y.sum).ToDecimal(), // работы ПСД
                                        smrSum = x.Where(y => y.TypeWork == TypeWork.Work).Sum(y => y.sum).ToDecimal(), // работы СМР
                                        tehSum = x.Where(y => tehWorks.Contains(y.Code) && y.TypeWork == TypeWork.Service).Sum(y => y.sum).ToDecimal()  // работы Технадзор
                                    });


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var nn = 0;
            decimal total = 0, totalSmr = 0, totalPsd = 0, totalTeh = 0, totalOther = 0;
            foreach (var objectCr in objectsCrDict)
            {
                nn++;
                section.ДобавитьСтроку();

                section["NN"] = nn;
                section["Municipality"] = objectCr.Value.muName;
                section["Address"] = objectCr.Value.address;
                section["State"] = objectCr.Value.stateName;

                var accepted = objectCr.Value.accepted;
                section["Accepted"] = accepted != DateTime.MinValue ? accepted.ToShortDateString() : string.Empty;

                var allSum = 0M;
                var psdSum = 0M;
                var smrSum = 0M;
                var tehSum = 0M;
                var otherSum = 0M;

                if (sumByTypeWorks.ContainsKey(objectCr.Key))
                {
                    // сумма по всем работам
                    allSum = sumByTypeWorks[objectCr.Key].allSum;
                    total += allSum.ToDecimal();

                    // работы ПСД
                    psdSum = sumByTypeWorks[objectCr.Key].psdSum;
                    totalPsd += psdSum.ToDecimal();

                    // работы СМР
                    smrSum = sumByTypeWorks[objectCr.Key].smrSum;
                    totalSmr += smrSum.ToDecimal();

                    // работы Технадзор
                    tehSum = sumByTypeWorks[objectCr.Key].tehSum;
                    totalTeh += tehSum.ToDecimal();

                    // иные расходы
                    otherSum = allSum - psdSum - smrSum - tehSum;
                    totalOther += otherSum.ToDecimal();
                }

                section["Sum"] = allSum;
                section["SumSMR"] = smrSum;
                section["SumPSD"] = psdSum;
                section["SumTeh"] = tehSum;
                section["SumOther"] = otherSum;
            }

            reportParams.SimpleReportParams["Total"] = total;
            reportParams.SimpleReportParams["TotalSMR"] = totalSmr;
            reportParams.SimpleReportParams["TotalPSD"] = totalPsd;
            reportParams.SimpleReportParams["TotalTeh"] = totalTeh;
            reportParams.SimpleReportParams["TotalOther"] = totalOther;
        }
    }
}