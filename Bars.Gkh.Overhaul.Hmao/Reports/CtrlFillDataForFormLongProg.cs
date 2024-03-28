using Bars.Gkh.Overhaul.Hmao.PriorityParams;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;

    using Castle.Windsor;
    using Enums;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Utils;

    /// <summary>
    /// Контроль расчета долгосрочной программы
    /// </summary>
    internal class CtrlFillDataForFormLongProg : BasePrintForm
    {
        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        public CtrlFillDataForFormLongProg()
            : base(new ReportTemplateBinary(Properties.Resources.CtrlFillDataForFormLongProg))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Domain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public IDomainService<WorkPrice> WorkPriceDomain { get; set; }

        public IDomainService<StructuralElementGroup> StructElGroupDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<CommonEstateObject> CeoDomain { get; set; }

        public IDomainService<StructuralElement> StructElDomain { get; set; }

        public override string Name
        {
            get { return "Контроль расчета долгосрочной программы"; }
        }

        public override string Desciption
        {
            get { return "Контроль расчета долгосрочной программы"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CtrlFillDataForFormLongProg"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.CtrlFillDataForFormLongProg"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;

            var dictStructElWork = this.StructElWorkDomain.GetAll()
                .Select(x => new
                {
                    SeId = x.StructuralElement.Id,
                    JobId = x.Job.Id,
                    JobName = x.Job.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.SeId)
                .ToDictionary(x => x.Key, y => y.ToArray());

            var dictPrices = this.WorkPriceDomain.GetAll()
                .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.Year == startYear)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key,
                    z => z.GroupBy(x => x.Job.Id).ToDictionary(x => x.Key,
                        y => y.Select(x => new
                        {
                            x.SquareMeterCost,
                            x.NormativeCost
                        }).First()));

            var ceoDict = this.CeoDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CeoType = x.GroupType.Name,
                    x.IsMain,
                    x.IncludedInSubjectProgramm
                })
                .ToDictionary(x => x.Id);

            var seGroupDict = this.StructElGroupDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Required,
                    x.UseInCalc
                })
                .ToDictionary(x => x.Id);

            var seNameById = this.StructElDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToDictionary(x => x.Id, y => y.Name);

            var roData = this.RealityObjectDomain.GetAll()
                .Select(x => new RealityObjectProxy
                {
                    Id = x.Id,
                    Municipality = x.Municipality.Name,
                    Settlement = x.MoSettlement.Name,
                    Address = x.Address,
                    DateCommissioning = x.DateCommissioning,
                    AreaMkd = x.AreaMkd,
                    AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                    AreaLiving = x.AreaLiving,
                    TypeHouse = x.TypeHouse,
                    BuildYear = x.BuildYear,
                    NumberApartments = x.NumberApartments,
                    PhysicalWear = x.PhysicalWear,
                    IsNotInvolvedCr = x.IsNotInvolvedCr,
                    IsRepairInadvisable = x.IsRepairInadvisable,
                    StateName = x.State.Name
                })
                .ToList();

            if (this.ModifyEnumerableService != null)
            {
                roData = this.ModifyEnumerableService.ReplaceProperty(roData, ".", x => x.Municipality, x => x.Address).ToList();
            }

            var roInfoDict = roData.ToDictionary(x => x.Id);

            var data = this.Stage1Domain.GetAll()
                .WhereIf(this.municipalityIds.Any(),
                    x => this.municipalityIds.Contains(x.StructuralElement.RealityObject.Municipality.Id)
                        || this.municipalityIds.Contains(x.StructuralElement.RealityObject.MoSettlement.Id))
                .OrderBy(x => x.StructuralElement.RealityObject.Municipality.Name)
                .ThenBy(x => x.StructuralElement.RealityObject.MoSettlement.Name)
                .ThenBy(x => x.StructuralElement.RealityObject.Address)
                .ThenBy(x => x.Year)
                .Select(x => new
                {
                    MunicipalityId = x.StructuralElement.RealityObject.Municipality.Id,
                    RoId = x.StructuralElement.RealityObject.Id,
                    RoSeId = x.StructuralElement.Id,
                    StructElId = x.StructuralElement.StructuralElement.Id,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.Wearout,
                    x.StructuralElement.Volume,
                    x.Year,
                    x.Sum,
                    x.Stage2.Stage3.StoredCriteria,
                    x.Stage2.Stage3.StoredPointParams,
                    x.Stage2.Stage3.IndexNumber,
                    CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                    StructElGroupId = x.StructuralElement.StructuralElement.Group.Id,
                    x.StructuralElement.StructuralElement.CalculateBy,
                    x.StructuralElement.StructuralElement.LifeTime,
                    x.StructuralElement.StructuralElement.LifeTimeAfterRepair
                })
                .ToArray();

            var roseFirstYear = data.Select(x => new {x.RoSeId, x.Year})
                .GroupBy(x => x.RoSeId)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.Year).Select(x => x.Year).First());

            var usedCriteries = data.GroupBy(x => x.MunicipalityId)
                .Select(x => x.First())
                .SelectMany(x => x.StoredCriteria.OrderBy(y => y.Value).Select(y => y.Criterion))
                .Distinct()
                .ToList();
            var usedPointParam = data.GroupBy(x => x.MunicipalityId)
                .Select(x => x.First())
                .SelectMany(x => x.StoredPointParams.Select(y => y.Code))
                .Distinct()
                .ToList();

            this.CreateVertSections(reportParams, usedCriteries, usedPointParam);

            var sectionRealObj = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRealObj");

            var number = 1;
            foreach (var item in data)
            {
                var jobs = dictStructElWork.ContainsKey(item.StructElId) ? dictStructElWork[item.StructElId] : null;

                var jobIndex = 0;
                do
                {
                    sectionRealObj.ДобавитьСтроку();
                    sectionRealObj["Number"] = number++;
                    sectionRealObj["IndexNumber"] = item.IndexNumber;

                    if (roInfoDict.ContainsKey(item.RoId))
                    {
                        var roInfo = roInfoDict[item.RoId];
                        sectionRealObj["Municipality"] = roInfo.Municipality;
                        sectionRealObj["Settlement"] = roInfo.Settlement;
                        sectionRealObj["Address"] = roInfo.Address;
                        sectionRealObj["TypeHouse"] = roInfo.TypeHouse.GetEnumMeta().Display;
                        sectionRealObj["State"] = roInfo.StateName;
                        sectionRealObj["BuildYear"] = roInfo.BuildYear;
                        sectionRealObj["AreaMkd"] = roInfo.AreaMkd;
                        sectionRealObj["AreaLivingNotLivingMkd"] = roInfo.AreaLivingNotLivingMkd;
                        sectionRealObj["AreaLiving"] = roInfo.AreaLiving;
                        sectionRealObj["NumberApartments"] = roInfo.NumberApartments;
                        sectionRealObj["PhysicalWear"] = roInfo.PhysicalWear;
                        sectionRealObj["IsNotInvolvedCr"] = roInfo.IsNotInvolvedCr ? 1 : 0;
                        sectionRealObj["IsRepairInadvisable"] = roInfo.IsRepairInadvisable ? 1 : 0;
                        sectionRealObj["DateCommissioning"] = roInfo.DateCommissioning;
                    }

                    if (ceoDict.ContainsKey(item.CeoId))
                    {
                        sectionRealObj["Ceo"] = ceoDict[item.CeoId].Name;
                        sectionRealObj["CeoType"] = ceoDict[item.CeoId].CeoType;
                        sectionRealObj["IsMain"] = ceoDict[item.CeoId].IsMain ? 1 : 0;
                        sectionRealObj["IncludedInSubjectProgramm"] = ceoDict[item.CeoId].IncludedInSubjectProgramm ? 1 : 0;
                    }

                    if (seGroupDict.ContainsKey(item.StructElGroupId))
                    {
                        sectionRealObj["StructElGroup"] = seGroupDict[item.StructElGroupId].Name;
                        sectionRealObj["Required"] = seGroupDict[item.StructElGroupId].Required ? 1 : 0;
                        sectionRealObj["UseInCalc"] = seGroupDict[item.StructElGroupId].UseInCalc ? 1 : 0;
                    }

                    sectionRealObj["StructEl"] = seNameById.ContainsKey(item.StructElId) ? seNameById[item.StructElId] : string.Empty;
                    sectionRealObj["LastOverhaulYear"] = item.LastOverhaulYear;
                    sectionRealObj["Wear"] = item.Wearout;
                    sectionRealObj["Volume"] = item.Volume;
                    sectionRealObj["PlanYear"] = item.Year;
                    sectionRealObj["Volume"] = item.Volume;
                    sectionRealObj["Sum"] = item.Sum;
                    sectionRealObj["Lifetime"] = roseFirstYear.Get(item.RoSeId) == item.Year
                        ? item.LifeTime
                        : item.LifeTimeAfterRepair > 0
                            ? item.LifeTimeAfterRepair
                            : item.LifeTime;

                    if (jobs != null)
                    {
                        var job = jobs[jobIndex];

                        var workPrice = dictPrices.ContainsKey(item.MunicipalityId)
                            && dictPrices[item.MunicipalityId].ContainsKey(job.JobId)
                                ? dictPrices[item.MunicipalityId][job.JobId]
                                : null;

                        sectionRealObj["Job"] = job.JobName;

                        if (workPrice != null)
                        {
                            sectionRealObj["WorkPrice"] = item.CalculateBy == PriceCalculateBy.Volume
                                ? workPrice.NormativeCost
                                : workPrice.SquareMeterCost;
                        }
                    }


                    foreach (var criteia in item.StoredCriteria)
                    {
                        sectionRealObj[criteia.Criterion] = criteia.Value;
                    }

                    foreach (var pointParam in item.StoredPointParams)
                    {
                        sectionRealObj[pointParam.Code] = pointParam.Value;
                    }

                    jobIndex++;
                }
                while (jobs != null && jobs.Length > jobIndex);
            }
        }

        private void CreateVertSections(ReportParams reportParams, List<string> usedCriterias, List<string> usedPointParams)
        {
            var priorityParamsByCode = this.Container.ResolveAll<IProgrammPriorityParam>()
                .Select(x => new { x.Code, x.Name })
                .ToDictionary(x => x.Code, y => y.Name);

            var pointParamsByCode = this.Container.ResolveAll<IPriorityParams>()
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, y => y.Name);

            var vertSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");

            foreach (var criteria in usedCriterias.Where(x => x != "PointsParam"))
            {
                vertSection.ДобавитьСтроку();
                vertSection["CriteriaName"] = priorityParamsByCode.ContainsKey(criteria)
                    ? priorityParamsByCode[criteria]
                    : string.Empty;

                vertSection["CriteriaValue"] = $"${criteria}$";
            }

            if (usedCriterias.Contains("PointsParam"))
            {
                vertSection.ДобавитьСтроку();
                vertSection["CriteriaName"] = "Баллы";
                vertSection["CriteriaValue"] = "$PointsParam$";
            }

            foreach (var pointParam in usedPointParams)
            {
                vertSection.ДобавитьСтроку();
                vertSection["CriteriaName"] = pointParamsByCode.ContainsKey(pointParam)
                    ? pointParamsByCode[pointParam]
                    : string.Empty;

                vertSection["CriteriaValue"] = $"${pointParam}$";
            }
        }

        private class RealityObjectProxy
        {
            public long Id { get; set; }
            public string Municipality { get; set; }
            public string Settlement { get; set; }
            public string Address { get; set; }
            public DateTime? DateCommissioning { get; set; }
            public decimal? AreaMkd { get; set; }
            public decimal? AreaLivingNotLivingMkd { get; set; }
            public decimal? AreaLiving { get; set; }
            public TypeHouse TypeHouse { get; set; }
            public int? BuildYear { get; set; }
            public int? NumberApartments { get; set; }
            public decimal? PhysicalWear { get; set; }
            public bool IsNotInvolvedCr { get; set; }
            public bool IsRepairInadvisable { get; set; }
            public string StateName { get; set; }
        }
    }
}