namespace Bars.GkhCr.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class InformOnHousesIncludedProgramCrReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        private List<long> listMuIds;
        private List<long> periodIds;
        private List<long> stateIds;

        public InformOnHousesIncludedProgramCrReport()
            : base(new ReportTemplateBinary(Properties.Resources.InformOnHousesIncludedProgramCr))
        {
        }

        public override string Name
        {
            get
            {
                return "Информация по домам включенных в программу капитального ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация по домам включенных в программу капитального ремонта";
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
                return "B4.controller.report.InformOnHousesIncludedProgramCr"; 
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.InformOnHousesIncludedProgramCr"; 
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunIds = baseParams.Params.GetAs("municipalityIds", string.Empty);
            listMuIds = !string.IsNullOrEmpty(strMunIds) ? strMunIds.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();

            var strPeriodIds = baseParams.Params.GetAs("periodIds", string.Empty);
            periodIds = !string.IsNullOrEmpty(strPeriodIds) ? strPeriodIds.Split(',').Select(x => x.ToLong()).ToList(): new List<long>();

            var strStateIds = baseParams.Params.GetAs("stateIds", string.Empty);
            stateIds = !string.IsNullOrEmpty(strStateIds) ? strStateIds.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var listYear = new List<int>();
            for (var i = 0; i < 26; i++)
            {
                listYear.Add(2014 + i);
            }

            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var dictMunicipality =
                serviceMunicipality.GetAll()
                                   .WhereIf(listMuIds.Count > 0, x => listMuIds.Contains(x.Id))
                                   .Select(x => new { x.Id, x.Name })
                                   .OrderBy(x => x.Name)
                                   .ToDictionary(x => x.Id, x => x.Name);

            var municipalityIds = dictMunicipality.Keys.ToList();

            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var dataReport =  serviceObjectCr.GetAll()
               .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
               .WhereIf(periodIds.Count > 0, x => periodIds.Contains(x.ProgramCr.Period.Id))
               .WhereIf(stateIds.Count > 0, x => stateIds.Contains(x.State.Id))
               .Select(x => new
                           {
                               objCrId = x.Id,
                               roId = x.RealityObject.Id,
                               muId = x.RealityObject.Municipality.Id,
                               startDateYear = x.ProgramCr.Period.DateStart.Year,
                               endDateYear = x.ProgramCr.Period.DateEnd.Value.Year
                           })
               .OrderBy(x => x.muId)
               .AsEnumerable()
               .GroupBy(x => x.muId)
               .ToDictionary(
                    x => x.Key,
                    y =>
                    {
                        var listEqualsYear = y.Where(z => z.startDateYear == z.endDateYear).ToList();
                        var listNotEqualsYear = y.Where(z => z.startDateYear != z.endDateYear).ToList();

                        var dictResult = listYear.ToDictionary(year => year, year => listEqualsYear.Where(z => z.startDateYear == year).Select(z => z.roId).Distinct().Count());

                        var dictStartYear = listNotEqualsYear.GroupBy(z => z.startDateYear).ToDictionary(z => z.Key, z => z.Select(i => i.roId).Distinct().Count());
                        var dictEndYear = listNotEqualsYear.GroupBy(z => z.endDateYear).ToDictionary(z => z.Key, z => z.Select(i => i.roId).Distinct().Count());

                        dictStartYear.ForEach(z => dictResult[z.Key] += z.Value);
                        dictEndYear.ForEach(z => dictResult[z.Key] += z.Value);

                        return dictResult;
                    });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var row = 1;

            foreach (var mu in dictMunicipality)
            {
                section.ДобавитьСтроку();
                section["row"] = row++;
                section["mu"] = mu.Value;

                if (!dataReport.ContainsKey(mu.Key))
                {
                    foreach (var year in listYear)
                    {
                        section["data" + year] = 0;
                    }

                    continue;
                }

                foreach (var data in dataReport[mu.Key])
                {
                    section["data" + data.Key] = data.Value;
                }
            }

            var dictData = dataReport.SelectMany(x => x.Value).ToList();

            foreach (var year in listYear)
            {
                reportParams.SimpleReportParams["total" + year] = dictData.Where(x => x.Key == year).Sum(x => x.Value);
            }
        }
    }
}