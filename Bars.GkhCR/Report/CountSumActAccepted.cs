namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class CountSumActAccepted : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] programCrIds;

        public CountSumActAccepted()
            : base(new ReportTemplateBinary(Properties.Resources.CountSumActAccepted))
        {
        }

        public override string Name
        {
            get { return "Количество и сумма принятых госжилинспекцикй актов по форме КС-2"; }
        }

        public override string Desciption
        {
            get { return "Количество и сумма принятых госжилинспекцикй актов по форме КС-2"; }
        }

        public override string GroupName
        {
            get { return "Отчеты Кап.ремонта"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CountSumActAccepted"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CountSumActAccepted";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var programCrIdsList = baseParams.Params.ContainsKey("programCrIds")
              ? baseParams.Params["programCrIds"].ToString()
              : string.Empty;

            programCrIds = !string.IsNullOrEmpty(programCrIdsList) ? programCrIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            reportParams.SimpleReportParams["ДатаОтчета"] = DateTime.Now.ToShortDateString();

            var programs = Container.Resolve<IDomainService<ProgramCr>>().GetAll()
                .Where(x => this.programCrIds.Contains(x.Id))
                .Select(y => y.Name)
                .ToList();

            reportParams.SimpleReportParams["Программа"] = programs.Any() ? programs.Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)) : string.Empty;
            
            var crObjects = Container.Resolve<IDomainService<TypeWorkCr>>()
                .GetAll()
                .Where(x => this.programCrIds.Contains(x.ObjectCr.ProgramCr.Id))
                .Select(x => new
                {
                    municipality = x.ObjectCr.RealityObject.Municipality.Id,
                    sum = x.Sum ?? 0,
                    x.Work.TypeWork,
                    objectCrId = x.ObjectCr.Id
                })
                .ToList()
                .GroupBy(x => x.municipality)
                .ToDictionary(x => x.Key, 
                x => new
                    {
                        limitSmr = x.Where(z => z.TypeWork == TypeWork.Work).Sum(z => z.sum),
                        limitAll = x.Sum(z => z.sum),
                        count = x.Select(z => z.objectCrId).Distinct().Count()
                    });
            
            var gjiAcceptedStatusIds = Container.Resolve<IDomainService<State>>().GetAll()
                .Where(x => x.Name == "Принято ГЖИ" || x.Name == "Принят ТОДК")
                .Select(x => x.Id)
                .ToList();

            var repData = Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                         .Where(x => x.TypeWorkCr != null)
                         .Where(x => x.State != null);
            
            var data = repData
               .Where(x => programCrIds.Contains(x.ObjectCr.ProgramCr.Id))
               .Where(x => gjiAcceptedStatusIds.Contains(x.State.Id))
               .Select(x => new
               {
                   ObjectCrId = x.ObjectCr.Id,
                   MuId = x.ObjectCr.RealityObject.Municipality.Id,
                   ActSum = x.Sum ?? 0,
                   WorkType = x.TypeWorkCr.Work,
                   WorkSum = x.TypeWorkCr.Sum ?? 0
               })
               .ToList();
            
            var objCrHasAct = Container.Resolve<IDomainService<PerformedWorkAct>>()
                .GetAll()
                .Where(x => gjiAcceptedStatusIds.Contains(x.State.Id))
                .Select(x => x.ObjectCr.Id);
            
            var typeWorkSumHasActByMunicipality = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => programCrIds.Contains(x.ObjectCr.ProgramCr.Id) && objCrHasAct.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    x.ObjectCr.Id,
                    MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                    Sum = x.Sum ?? 0, 
                    x.Work.TypeWork,
                })
                .ToList()
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, 
                    y => new
                    {
                        limitAllAkt = y.Sum(z => z.Sum), 
                        limitSmrAkt = y.Where(x => x.TypeWork == TypeWork.Work).Sum(z => z.Sum)
                    });
            
            var groupedByMu = data
                .GroupBy(x => x.MuId)
                .Select(x => 
                {
                    var count = x.GroupBy(y => y.ObjectCrId)
                        .Select(y => y.Count())
                        .Count(y => y > 0);

                    return new
                               {
                                   municipality = x.Key,
                                   GjiAcceptedCount = x.Count(),
                                   GjiAcceptedSum = x.Sum(y => y.ActSum),
                                   typeWorkCount = x.Count(y => y.WorkType.TypeWork == TypeWork.Work),
                                   typeWorkSum = x.Where(y => y.WorkType.TypeWork == TypeWork.Work).Sum(z => z.ActSum),
                                   typeWorkCount1018 = x.Count(y => y.WorkType.Code == "1018"),
                                   typeWorkSum1018 = x.Where(y => y.WorkType.Code == "1018").Sum(z => z.ActSum),
                                   typeWorkCount1019 = x.Count(y => y.WorkType.Code == "1019"),
                                   typeWorkSum1019 = x.Where(y => y.WorkType.Code == "1019").Sum(z => z.ActSum),
                                   typeWorkCount1020 = x.Count(y => y.WorkType.Code == "1020"),
                                   typeWorkSum1020 = x.Where(y => y.WorkType.Code == "1020").Sum(z => z.ActSum),
                                   count
                               };
                })
                .ToDictionary(x => x.municipality);
            
            int i = 0;
            int totalCount = 0;
            int totalactcount = 0;
            int totalGjiAcceptedCount = 0;
            decimal totalGjiAcceptedSum = 0;
            int totaltypeWorkCount = 0;
            decimal totaltypeWorkSum = 0;
            int totaltypeWorkCount1018 = 0;
            decimal totaltypeWorkSum1018 = 0;
            int totaltypeWorkCount1019 = 0;
            decimal totaltypeWorkSum1019 = 0;
            int totaltypeWorkCount1020 = 0;
            decimal totaltypeWorkSum1020 = 0;
            decimal totalGjiAcceptedsmrSum = 0;
            decimal totalsmrSum = 0;
            decimal totalGjiAcceptedSmrandPsdSum = 0;
            decimal totalsmrAndPsdSum = 0;

            var municipalities = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var municipality in municipalities)
            {
                section.ДобавитьСтроку();

                section["Номер1"] = ++i;

                section["Район"] = municipality.Name;
                
                if (typeWorkSumHasActByMunicipality.ContainsKey(municipality.Id))
                {
                    var value = typeWorkSumHasActByMunicipality[municipality.Id];

                    section["ЛимитДомовЕстьАкты"] = value.limitSmrAkt;
                    section["ЛимитДомовПСДЕстьАкты"] = value.limitAllAkt;

                    totalGjiAcceptedsmrSum += value.limitSmrAkt;
                    totalGjiAcceptedSmrandPsdSum += value.limitAllAkt;
                }
                else
                {
                    section["ЛимитДомовЕстьАкты"] = 0;
                    section["ЛимитДомовПСДЕстьАкты"] = 0;
                }

                if (crObjects.ContainsKey(municipality.Id))
                {
                    var value = crObjects[municipality.Id];

                    section["КолвоДомов"] = value.count;
                    section["ЛимитДомов"] = value.limitSmr;
                    section["ЛимитДомовПСД"] = value.limitAll;

                    totalCount += value.count;
                    totalsmrSum += value.limitSmr;
                    totalsmrAndPsdSum += value.limitAll;
                }
                else
                {
                    section["КолвоДомов"] = 0;
                    section["ЛимитДомов"] = 0;
                    section["ЛимитДомовПСД"] = 0;
                }

                if (groupedByMu.ContainsKey(municipality.Id))
                {
                    var value = groupedByMu[municipality.Id];

                    section["КолвоДомовЕстьАкты"] = value.count;
                    section["КолвоАкты"] = value.GjiAcceptedCount;
                    section["СуммаАкты"] = value.GjiAcceptedSum;
                    section["КолвоАктыСМР"] = value.typeWorkCount;
                    section["СуммаАктыСМР"] = value.typeWorkSum;
                    section["КолвоАкты1018"] = value.typeWorkCount1018;
                    section["СуммаАкты1018"] = value.typeWorkSum1018;
                    section["КолвоАкты1019"] = value.typeWorkCount1019;
                    section["СуммаАкты1019"] = value.typeWorkSum1019;
                    section["КолвоАкты1020"] = value.typeWorkCount1020;
                    section["СуммаАкты1020"] = value.typeWorkSum1020;

                    totalactcount += value.count;
                    totalGjiAcceptedCount += value.GjiAcceptedCount;
                    totalGjiAcceptedSum += value.GjiAcceptedSum;
                    totaltypeWorkCount += value.typeWorkCount;
                    totaltypeWorkSum += value.typeWorkSum;
                    totaltypeWorkCount1018 += value.typeWorkCount1018;
                    totaltypeWorkSum1018 += value.typeWorkSum1018;
                    totaltypeWorkCount1019 += value.typeWorkCount1019;
                    totaltypeWorkSum1019 += value.typeWorkSum1019;
                    totaltypeWorkCount1020 += value.typeWorkCount1020;
                    totaltypeWorkSum1020 += value.typeWorkSum1020;
                }
                else
                {
                    section["КолвоДомовЕстьАкты"] = 0;
                    section["КолвоАкты"] = 0;
                    section["СуммаАкты"] = 0;
                    section["КолвоАктыСМР"] = 0;
                    section["СуммаАктыСМР"] = 0;
                    section["КолвоАкты1018"] = 0;
                    section["СуммаАкты1018"] = 0;
                    section["КолвоАкты1019"] = 0;
                    section["СуммаАкты1019"] = 0;
                    section["КолвоАкты1020"] = 0;
                    section["СуммаАкты1020"] = 0;
                }

            }

            reportParams.SimpleReportParams["ИтогоКолвоДомов"] = totalCount;
            reportParams.SimpleReportParams["ИтогоКолвоДомовЕстьАкты"] = totalactcount;
            reportParams.SimpleReportParams["ИтогоКолвоАкты"] = totalGjiAcceptedCount;
            reportParams.SimpleReportParams["ИтогоСуммаАкты"] = totalGjiAcceptedSum;
            reportParams.SimpleReportParams["ИтогоКолвоАктыСМР"] = totaltypeWorkCount;
            reportParams.SimpleReportParams["ИтогоСуммаАктыСМР"] = totaltypeWorkSum;
            reportParams.SimpleReportParams["ИтогоКолвоАкты1018"] = totaltypeWorkCount1018;
            reportParams.SimpleReportParams["ИтогоСуммаАкты1018"] = totaltypeWorkSum1018;
            reportParams.SimpleReportParams["ИтогоКолвоАкты1019"] = totaltypeWorkCount1019;
            reportParams.SimpleReportParams["ИтогоСуммаАкты1019"] = totaltypeWorkSum1019;
            reportParams.SimpleReportParams["ИтогоКолвоАкты1020"] = totaltypeWorkCount1020;
            reportParams.SimpleReportParams["ИтогоСуммаАкты1020"] = totaltypeWorkSum1020;
            reportParams.SimpleReportParams["ИтогоЛимитДомовЕстьАкты"] = totalGjiAcceptedsmrSum;
            reportParams.SimpleReportParams["ИтогоЛимитДомов"] = totalsmrSum;
            reportParams.SimpleReportParams["ИтогоЛимитДомовПСДЕстьАкты"] = totalGjiAcceptedSmrandPsdSum;
            reportParams.SimpleReportParams["ИтогоЛимитДомовПСД"] = totalsmrAndPsdSum;
        }
    }
}