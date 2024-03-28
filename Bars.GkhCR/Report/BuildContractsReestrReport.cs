namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Реестр договоров подряда ГЖИ
    /// </summary>
    public class BuildContractsReestrReport : BasePrintForm
    {
        #region параметры
        private long programCrId;

        private long[] municipalityIds;

        private bool contractsFilter;
        #endregion

        public BuildContractsReestrReport()
            : base(new ReportTemplateBinary(Properties.Resources.BuildContractsReestr))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.BuildContractsReestr"; }
        }

        public override string Name
        {
            get { return "Реестр договоров подряда ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Реестр договоров подряда ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Капитальный ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.BuildContractsReestrReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params.GetAs<long>("programCrId");
            this.municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            this.contractsFilter = baseParams.Params.GetAs<bool>("contractsFilter");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCrDomain = this.Container.Resolve<IDomainService<ProgramCr>>();
            var objectCrDomain = this.Container.Resolve<IDomainService<ObjectCr>>();
            var buildContractDomain = this.Container.Resolve<IDomainService<BuildContract>>();
            var typeWorkDomain = this.Container.Resolve<IDomainService<TypeWorkCr>>();

            using (this.Container.Using(programCrDomain, objectCrDomain, buildContractDomain, typeWorkDomain))
            {
                // выбранная программа КР
                var program = programCrDomain.GetAll()
                    .Where(x => x.Id == this.programCrId)
                    .Select(x => new { x.Period.DateEnd, x.Period.DateStart })
                    .FirstOrDefault();

                if (program == null)
                {
                    return;
                }

                reportParams.SimpleReportParams["year"] = program.DateEnd.Value.Year;

                var objectCrQuery = objectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == this.programCrId)
                    .WhereIf(this.municipalityIds != null, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

                var objectCrIds = objectCrQuery.Select(x => x.Id);

                // объекты КР текущей программы КР
                var objectsCr = objectCrQuery
                    .Select(x => new
                    {
                        x.Id,
                        RealtyObjId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MuName = x.RealityObject.Municipality.Name,
                        MuId = x.RealityObject.Municipality.Id,
                        x.GjiNum
                    })
                    .OrderBy(x => x.MuName)
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key,
                        y => y.Select(z => new { z.Id, z.RealtyObjId, z.Address, z.GjiNum, z.MuName }).OrderBy(x => x.Address).ToList());

                var buildContractQuery = buildContractDomain.GetAll()
                    .Where(x => objectCrIds.Contains(x.ObjectCr.Id));

                var buildContractsDict = buildContractQuery
                    .WhereIf(this.contractsFilter, x => x.State.Name == "Утверждено ГЖИ")
                    .Select(x => new
                    {
                        CrObjectId = x.ObjectCr.Id,
                        x.TypeContractBuild
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.CrObjectId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(z => new
                            {
                                CountSmr = y.Count(t => t.TypeContractBuild == TypeContractBuild.Smr),
                                CountLift = y.Count(t => t.TypeContractBuild == TypeContractBuild.Lift),
                                CountDevices = y.Count(t => t.TypeContractBuild == TypeContractBuild.Device),
                                CountEnergySurvey = y.Count(t => t.TypeContractBuild == TypeContractBuild.EnergySurvey)
                            })
                            .FirstOrDefault());

                // работы КР
                var liftWorks = new List<string> { "15", "14", "142", "143", "141" };
                var deviceWorks = new List<string> { "29", "11", "88", "9", "10", "7", "8" };

                var typeWorksCrDict = typeWorkDomain.GetAll()
                    .Where(x => x.Work != null && objectCrIds.Contains(x.ObjectCr.Id) && x.IsActive)
                    .Select(x => new
                    {
                        ObjectCrId = x.ObjectCr.Id,
                        x.Sum,
                        x.Work.Code,
                        x.Work.TypeWork
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrId)
                    .ToDictionary(x => x.Key,
                        y => y.Select(z => new
                            {
                                Sum = y.Sum(t => t.Sum),
                                CountWorksLift = y.Count(t => liftWorks.Contains(t.Code)),
                                CountWorksDevices = y.Count(t => deviceWorks.Contains(t.Code)),
                                CountWorks = y.Count(t =>
                                    t.TypeWork == TypeWork.Work && !liftWorks.Contains(t.Code) && !deviceWorks.Contains(t.Code))
                            })
                            .FirstOrDefault());

                var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("section");
                var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");

                foreach (var municipality in objectsCr.Keys)
                {
                    sectionMu.ДобавитьСтроку();
                    sectionMu["Municipality"] = objectsCr[municipality].Select(x => x.MuName).FirstOrDefault();

                    decimal itogFinanceLimit = 0;
                    long itogoSmr = 0;
                    long itogoElevator = 0;
                    long itogoDevices = 0;
                    long itogoEnergySurvey = 0;

                    foreach (var objectCr in objectsCr[municipality])
                    {
                        sectionRo.ДобавитьСтроку();
                        sectionRo["ReestrNumber"] = objectCr.GjiNum;
                        sectionRo["Municipality"] = objectCr.MuName;
                        sectionRo["Address"] = objectCr.Address;

                        if (!buildContractsDict.ContainsKey(objectCr.Id))
                        {
                            sectionRo["Smr"] = 0;
                            sectionRo["Elevator"] = 0;
                            sectionRo["Devices"] = 0;
                            sectionRo["EnergySurvey"] = 0;
                        }
                        else
                        {
                            var objectCrContractInfo = buildContractsDict[objectCr.Id];
                            sectionRo["Smr"] = objectCrContractInfo.CountSmr;
                            sectionRo["Elevator"] = objectCrContractInfo.CountLift;
                            sectionRo["Devices"] = objectCrContractInfo.CountDevices;
                            sectionRo["EnergySurvey"] = objectCrContractInfo.CountEnergySurvey;

                            itogoSmr += objectCrContractInfo.CountSmr;
                            itogoElevator += objectCrContractInfo.CountLift;
                            itogoDevices += objectCrContractInfo.CountDevices;
                            itogoEnergySurvey += objectCrContractInfo.CountEnergySurvey;
                        }

                        if (!typeWorksCrDict.ContainsKey(objectCr.Id))
                        {
                            sectionRo["FinanceLimit"] = 0;
                            sectionRo["CountLift"] = 0;
                            sectionRo["CountDevices"] = 0;
                            sectionRo["CountWorks"] = 0;
                        }
                        else
                        {
                            var objectCrTypeWorkInfo = typeWorksCrDict[objectCr.Id];
                            sectionRo["FinanceLimit"] = objectCrTypeWorkInfo.Sum;
                            sectionRo["CountLift"] = objectCrTypeWorkInfo.CountWorksLift;
                            sectionRo["CountDevices"] = objectCrTypeWorkInfo.CountWorksDevices;
                            sectionRo["CountWorks"] = objectCrTypeWorkInfo.CountWorks;
                            itogFinanceLimit += objectCrTypeWorkInfo.Sum ?? 0;
                        }
                    }

                    sectionMu["ItogFinanceLimit"] = itogFinanceLimit;
                    sectionMu["ItogSmr"] = itogoSmr;
                    sectionMu["ItogElevator"] = itogoElevator;
                    sectionMu["itogDevices"] = itogoDevices;
                    sectionMu["itogEnergySurvey"] = itogoEnergySurvey;
                }
            }
        }

        public override string ReportGenerator
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}