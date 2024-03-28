namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    internal class LongtermProgOverhaulInfo : BasePrintForm
    {
        public LongtermProgOverhaulInfo()
            : base(new ReportTemplateBinary(Properties.Resources.LongtermProgOverhaulInfo))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Информация о долгосрочной программе капитального ремонта"; }
        }

        public override string Desciption
        {
            get { return "Информация о долгосрочной программе капитального ремонта"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.LongtermProgOverhaulInfo"; }
        }

        public override string RequiredPermission
        {
            get { return "Ovrhl.LongtermProgOverhaulInfo"; }
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
            var correctionService = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();

            var correctionQuery =
                correctionService.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Municipality != null)
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Any(),
                        x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id));

            var robjects = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.Municipality.Id)
                        || municipalityIds.Contains(x.MoSettlement.Id))
                .Where(y => correctionQuery.Any(x => x.RealityObject.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    MunName = x.Municipality.Name,
                    x.Address,
                    x.MaximumFloors,
                    x.NumberEntrances,
                    x.DateCommissioning,
                    x.AreaMkd
                })
                .OrderBy(x => x.MunName)
                .ThenBy(x => x.Address)
                .ToList();

            var apartInfo = Container.Resolve<IDomainService<RealityObjectApartInfo>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Where(x => x.RealityObject != null)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            var houseInfo = Container.Resolve<IDomainService<RealityObjectHouseInfo>>().GetAll()
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                         || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Where(x => x.RealityObject != null)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Count());

            var commonEstateObjects =
                Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                    .Where(x => x.IncludedInSubjectProgramm)
                    .Select(x => new {x.Id, x.Name})
                    .OrderBy(x => x.Name)
                    .ToList();

#warning Нет фильтрации по основной/неосновной программе

            var commonEstateObjectsDict =
                correctionService.GetAll()
                    .WhereIf(municipalityIds.Length > 0,
                        x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                            || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        CeoId = x.Stage2.CommonEstateObject.Id,
                        x.Stage2.Sum,
                        x.PlanYear
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, x => x.GroupBy(y => y.PlanYear)
                        .ToDictionary(y => y.Key, y => y.Select(z => new {z.CeoId, z.Sum}).ToList()));

            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("VerticalSection");

            foreach (var commonEstateObject in commonEstateObjects)
            {
                verticalSection.ДобавитьСтроку();

                verticalSection["CeoName"] = commonEstateObject.Name;
                verticalSection["PlanYear"] = "$year" + commonEstateObject.Id + "$";
                verticalSection["OverhaulCost"] = "$sum" + commonEstateObject.Id + "$";
            }

            var num = 1;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var rowCount = 0;
            foreach (var ro in robjects)
            {
                if (!commonEstateObjectsDict.ContainsKey(ro.Id))
                {
                    continue;
                }

                var ceoData = commonEstateObjectsDict[ro.Id];

                section.ДобавитьСтроку();

                section["Num"] = num++;
                section["Mun"] = ro.MunName;
                section["Address"] = ro.Address;
                section["FloorCount"] = ro.MaximumFloors;
                section["PorchCount"] = ro.NumberEntrances;
                section["YearCommissioning"] = ro.DateCommissioning;

                section["LivArea"] =
                    (apartInfo.ContainsKey(ro.Id) ? apartInfo[ro.Id] : 0)
                    + (houseInfo.ContainsKey(ro.Id) ? houseInfo[ro.Id] : 0);

                section["TotalArea"] = ro.AreaMkd.ToDecimal()/1000;

                int i = 0;
                foreach (var ceo in ceoData.OrderBy(x => x.Key))
                {
                    if (rowCount >= 65000)
                    {
                        throw new Exception("Количество строк превышает 65000, выберите другие параметры");
                    }

                    if (i > 0)
                    {
                        section.ДобавитьСтроку();
                    }

                    rowCount++;

                    foreach (var ooi in ceo.Value)
                    {
                        section["year" + ooi.CeoId] = ceo.Key;
                        section["sum" + ooi.CeoId] = ooi.Sum;
                    }

                    i++;
                }
            }
        }
    }
}