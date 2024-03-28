namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using Gkh.Utils;
    using Overhaul.Entities;
    using Properties;

    public class ProgramVersionReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<VersionRecord> VersionRecDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<StructuralElementWork> SeWorkDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgRealObjDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        #endregion

        public ProgramVersionReport()
            : base(new ReportTemplateBinary(Resources.ProgramVersion))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Версии ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Версии ДПКР";
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
                return "B4.controller.report.ProgramVersion";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.ProgramVersionReport";
            }
        }

        private long[] municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodEndYear = config.ProgrammPeriodEnd;

            var currentDate = DateTime.Now.Date;

            var worksDict = SeWorkDomain.GetAll()
                                    .Select(x => new
                                    {
                                        structId = x.StructuralElement.Id,
                                        WorkName = x.Job.Work.Name
                                    })
                                    .AsEnumerable()
                                    .GroupBy(x => x.structId)
                                    .ToDictionary(x => x.Key, y => y.First().WorkName);

            var muNameById = MunicipalityDomain.GetAll()
                                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Id))
                                .ToDictionary(x => x.Id, y => y.Name);

            var workNamesByStage3Dict = Stage1Domain.GetAll()
               .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
               .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
               .Select(x => new
               {
                   St3Id = x.Stage2Version.Stage3Version.Id,
                   SeId = x.StructuralElement.StructuralElement.Id
               })
               .AsEnumerable()
                .Select(x => new
                { 
                    x.St3Id,
                    WorkName = worksDict.ContainsKey(x.SeId) ? worksDict[x.SeId] : string.Empty,
                })
               .GroupBy(x => x.St3Id)
               .ToDictionary(x => x.Key, y => y.Select(x => x.WorkName).ToList());

            var manOrgByRealObjDict = ManOrgRealObjDomain.GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                        .Where(x => x.ManOrgContract.StartDate <= currentDate
                                && (x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate >= DateTime.Now
                                    || !x.ManOrgContract.EndDate.HasValue) && x.ManOrgContract != null)
                        .OrderByDescending(x => x.ManOrgContract.EndDate)
                        .Select(x => new
                        {
                            x.ManOrgContract.ManagingOrganization.Contragent.Name,
                            RoId = x.RealityObject.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Name).First());

            var versionsData = Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => x.ProgramVersion.IsMain && (x.IndexNumber > 0 || x.Year < periodEndYear))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    x.Id,
                    MuId = x.RealityObject.Municipality.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    RoId = x.RealityObject.Id,
                    x.CommonEstateObjects,
                    x.Year,
                    x.IndexNumber,
                    x.Point,
                    x.Sum
                })
                .OrderBy(x => x.Municipality)
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Year)
                                                .ThenBy(x => x.IndexNumber)
                                                .ToList());


            reportParams.SimpleReportParams["ReportDate"] = currentDate.ToShortDateString();

            var sectMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            var number = 1;
            foreach (var muData in versionsData)
            {
                sectMunicipality.ДобавитьСтроку();
                sectMunicipality["Municipality"] = muNameById.ContainsKey(muData.Key)
                                                       ? muNameById[muData.Key]
                                                       : string.Empty;

                var sectRealObj = sectMunicipality.ДобавитьСекцию("sectionRealObj");

                foreach (var roData in muData.Value)
                {
                    sectRealObj.ДобавитьСтроку();                    
                    sectRealObj["Number"] = number;
                    sectRealObj["IndexNumber"] = roData.IndexNumber;
                    sectRealObj["Address"] = roData.Address;
                    sectRealObj["ManOrg"] = manOrgByRealObjDict.ContainsKey(roData.RoId) ? manOrgByRealObjDict[roData.RoId] : string.Empty;
                    sectRealObj["Ceo"] = roData.CommonEstateObjects;
                    sectRealObj["TypeWorks"] = workNamesByStage3Dict.ContainsKey(roData.Id) && workNamesByStage3Dict[roData.Id].Any() ? 
                        workNamesByStage3Dict[roData.Id].AggregateWithSeparator(", ") : string.Empty;
                    sectRealObj["Sum"] = roData.Sum;
                    sectRealObj["Year"] = roData.Year;
                    sectRealObj["Point"] = roData.Point;

                    number++;
                }
            }

        } 
    }
}