namespace Bars.Gkh.Regions.Chelyabinsk.Reports
{
    using System;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Regions.Chelyabinsk.Properties;

    using Castle.Windsor;

    class UnderstandingHomeReport : BasePrintForm
    {
        public UnderstandingHomeReport()
            : base(new ReportTemplateBinary(Resources.UnderstandingHomeReport))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Общие сведения по домам"; }
        }

        public override string Desciption
        {
            get { return "Общие сведения по домам"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.UnderstandingHomeReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.UnderstandingHomeReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var manOrgContracts =
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                    .GetAll()
                    .Where(m => m.ManOrgContract.StartDate
                                <= DateTime.Now
                                && (m.ManOrgContract.EndDate == null
                                    || m.ManOrgContract.EndDate
                                    >= DateTime.Now));

            var data = Container.Resolve<IDomainService<LongTermPrObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x =>
                    (x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                    && (x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated 
                        || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                        && !x.RealityObject.IsNotInvolvedCr
                    )
                .Select(x => new
                {
                    MuName = x.RealityObject.Municipality.Name,
                    RoId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    x.RealityObject.BuildYear,
                    x.RealityObject.DateCommissioning,
                    x.RealityObject.PrivatizationDateFirstApartment,
                    x.RealityObject.DateTechInspection,
                    RepInadvisbleYesNo = (bool?)x.RealityObject.IsRepairInadvisable,
                    TypeProjname = x.RealityObject.TypeProject.Name,
                    x.RealityObject.PhysicalWear,
                    x.RealityObject.AreaLivingNotLivingMkd,
                    x.RealityObject.MaximumFloors,
                    x.RealityObject.Floors,
                    x.RealityObject.NumberEntrances,
                    x.RealityObject.NumberApartments,
                    x.RealityObject.NumberLifts
                });

            var contracts =
                manOrgContracts.Where(x => data.Any(y => x.RealityObject.Id == y.RoId))
                    .Select(x => new { x.RealityObject.Id, x.ManOrgContract.ManagingOrganization.Contragent.Name })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Name)));

            var res = data
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .ToArray();

            var num = 1;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realObj in res)
            {
                var obj = realObj;

                section.ДобавитьСтроку();

                section["Num"] = num++;
                section["Municipality"] = realObj.MuName;
                section["Contragent"] = contracts.ContainsKey(obj.RoId) ? contracts[obj.RoId] : string.Empty;
                section["Address"] = realObj.Address;
                section["BuildYear"] = realObj.BuildYear;
                section["ExploitationDate"] = realObj.DateCommissioning;
                section["FirstPrivatizDate"] = realObj.PrivatizationDateFirstApartment != null ? realObj.PrivatizationDateFirstApartment.ToStr() : string.Empty;
                section["LastTechSurvey"] = realObj.DateTechInspection;
                section["RepairImpractical"] = realObj.RepInadvisbleYesNo == false ? "Ремонт целесообразен" : "Ремонт не целесообразен";
                section["SeriesTypeProj"] = realObj.TypeProjname;
                section["PhysDeterioration"] = realObj.PhysicalWear;
                section["TotalArea"] = realObj.AreaLivingNotLivingMkd;
                section["MaxFloor"] = realObj.MaximumFloors;
                section["MinFloor"] = realObj.Floors;
                section["PorchCount"] = realObj.NumberEntrances;
                section["RoomCount"] = realObj.NumberApartments;
                section["LiftCount"] = realObj.NumberLifts;
            }
        }
    }
}
