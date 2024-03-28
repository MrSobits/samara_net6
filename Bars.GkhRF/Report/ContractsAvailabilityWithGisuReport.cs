namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class ContractsAvailabilityWithGisuReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region параметры

        private List<long> municipalityIds = new List<long>();

        private long programCrId;

        private DateTime reportDate = DateTime.Now;

        #endregion

        public ContractsAvailabilityWithGisuReport()
            : base(new ReportTemplateBinary(Properties.Resources.ContractsAvailabilityWithGisu))
        {

        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.ContractsAvailabilityWithGisu";
            }
        }

        public override string Name
        {
            get
            {
                return "Наличие договоров с ГИСУ";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Наличие договоров с ГИСУ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ContractsAvailabilityWithGisu";
            }
        }

        private List<long> ParseIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return ids.Split(',').Select(x => x.ToLong()).ToList();
            }

            return new List<long>();
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            if (baseParams.Params.ContainsKey("municipalityIds"))
            {
                this.municipalityIds = this.ParseIds(baseParams.Params["municipalityIds"].ToStr());
            }

            this.municipalityIds = baseParams.Params.ContainsKey("municipalityIds")
                                  ? this.ParseIds(baseParams.Params["municipalityIds"].ToStr())
                                  : new List<long>();

            this.programCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToLong() : -1;

            this.reportDate = baseParams.Params.ContainsKey("reportDate") ? baseParams.Params["reportDate"].ToDateTime() : DateTime.MinValue;
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["reportDate"] = this.reportDate.ToString("dd MMMM yyyy");

            var realtyObjectsInContractRf = this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                                                .Where(x => x.TypeCondition == TypeCondition.Include)
                                                .WhereIf(
                                                    this.reportDate != DateTime.MinValue,
                                                    x =>
                                                    x.IncludeDate <= this.reportDate
                                                    && (x.ContractRf.DateBegin <= this.reportDate || x.ContractRf.DateBegin == null || x.ContractRf.DateBegin <= DateTime.MinValue)
                                                    && (x.ContractRf.DateEnd >= this.reportDate || x.ContractRf.DateEnd == null || x.ContractRf.DateEnd <= DateTime.MinValue));

            var objectsCrList = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                .Where(x => x.ProgramCr.Id == this.programCrId)
                                .Select(x => new
                                                {
                                                    objecctCrId = x.Id,
                                                    realtyObjectId = x.RealityObject.Id,
                                                    hasContractGISU = realtyObjectsInContractRf.Any(y => y.RealityObject.Id == x.RealityObject.Id),
                                                    municipalityName = x.RealityObject.Municipality.Name,
                                                    x.RealityObject.Address,
                                                    x.RealityObject.AreaMkd,
                                                    x.RealityObject.DateCommissioning,
                                                    x.RealityObject.DateLastOverhaul
                                                })
                                .OrderBy(x => x.municipalityName)
                                .ThenBy(x => x.Address)
                                .ToList();

            var typeWorksDict = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                                .Select(x => new
                                {
                                    objecctCrId = x.ObjectCr.Id,
                                    x.Sum
                                })
                                .AsEnumerable()
                                .GroupBy(x => x.objecctCrId)
                                .ToDictionary(
                                    x => x.Key,
                                    x => new
                                            {
                                                sum = x.Sum(y => y.Sum),
                                                worksCount = x.Count()
                                            });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var i = 0;
            var worksCount = 0M;
            var worksSum = 0M;
            foreach (var objectCr in objectsCrList)
            {
                section.ДобавитьСтроку();
                section["number"] = ++i;
                section["moName"] = objectCr.municipalityName;
                section["address"] = objectCr.Address;
                section["contractAvailability"] = objectCr.hasContractGISU ? "Да" : "Нет";

                if (typeWorksDict.ContainsKey(objectCr.objecctCrId))
                {
                    var objectCrWorks = typeWorksDict[objectCr.objecctCrId];
                    
                    section["worksCount"] = objectCrWorks.worksCount;
                    section["worksCost"] = objectCrWorks.sum;
                    
                    worksCount += objectCrWorks.worksCount;
                    worksSum += objectCrWorks.sum ?? 0;
                }
                
                section["totalArea"] = objectCr.AreaMkd;
                section["commissioningDate"] = objectCr.DateCommissioning.HasValue ? objectCr.DateCommissioning.Value.Year.ToStr() : string.Empty;
                section["lastRepairYear"] = objectCr.DateLastOverhaul.HasValue ? objectCr.DateLastOverhaul.Value.Year.ToStr() : string.Empty;
            }

            reportParams.SimpleReportParams["roCount"] = objectsCrList.Count;
            reportParams.SimpleReportParams["countNo"] = objectsCrList.Count(x => !x.hasContractGISU);
            reportParams.SimpleReportParams["worksCountTotal"] = worksCount;
            reportParams.SimpleReportParams["costTotal"] = worksSum;
            reportParams.SimpleReportParams["areaTotal"] = objectsCrList.Sum(x => x.AreaMkd);
        }
    }
}
