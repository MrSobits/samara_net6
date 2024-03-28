using Bars.B4.Modules.FIAS;

namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    class CtrlCertOfBuildConsiderMissingCeo : BasePrintForm
    {
        public CtrlCertOfBuildConsiderMissingCeo()
            : base(new ReportTemplateBinary(Resources.CtrlCertOfBuildConsiderMissingCeo))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }
        public override string Name
        {
            get { return "Контроль паспортизации домов с учетом отсутствующих элементов"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов с учетом отсутствующих элементов"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CtrlCertOfBuildConsiderMissingCeo"; }
        }

        public override string RequiredPermission
        {
            get { return "Ovrhl.CtrlCertOfBuildConsiderMissingCeo"; }
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
            var manOrgContractRoService = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var municipalityService = Container.Resolve<IDomainService<Municipality>>();

            using (this.Container.Using(manOrgContractRoService, municipalityService))
            {
                var commonEstateObjectsQuery = this.CreateVerticalColums(reportParams);
                var ceoIds = commonEstateObjectsQuery.ToList();

                var realityObjByMuDict = this.GetRealtyObjects();
                var realityObjectStructuralElement = this.GetData(commonEstateObjectsQuery);
                var realObjMissingCommonEstObj = this.GetMissingCeo(commonEstateObjectsQuery);
                var dictRoPercent = this.GetRoPercent(commonEstateObjectsQuery);

                var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
                var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
                var num = 1;
                var sumPercent = 0M;
                var listAllPercent = new List<decimal>();

                var manOrgByRo = manOrgContractRoService
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                    .Where(x => x.ManOrgContract.ManagingOrganization != null)
                    .Select(x => new
                    {
                        moName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                        roId = x.RealityObject.Id
                    })
                    .ToList();

                foreach (var realtyObjectsByMu in realityObjByMuDict.OrderBy(x => x.Key))
                {
                    sectionMu.ДобавитьСтроку();
                    sectionMu["MuName"] = realtyObjectsByMu.Key;
                    var sumPercentByMu = 0M;

                    var listPercent = new List<decimal>();

                    foreach (var realtyObjectInfo in realtyObjectsByMu.Value.OrderBy(x => x.Address))
                    {
                        sectionRo.ДобавитьСтроку();

                        var realtyObjectId = realtyObjectInfo.roId;
                        RealtyObjProxy info = realtyObjectInfo;

                        sectionRo["Num"] = num++;
                        sectionRo["MO"] = realtyObjectsByMu.Key;

                        sectionRo["AD"] = realtyObjectInfo.MoParentId == null 
                            ? municipalityService.GetAll()
                                .Where(x => x.ParentMo != null && x.ParentMo.Id == info.MoId)
                                .Select(x => x.Name)
                                .FirstOrDefault()
                            : "";

                        sectionRo["Address"] = realtyObjectInfo.Address;
                        sectionRo["ManOrg"] = manOrgByRo
                            .Where(x => x.roId == realtyObjectId)
                            .Select(x => x.moName)
                            .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                        var currentRoCeoIdsList = realityObjectStructuralElement.ContainsKey(realtyObjectInfo.roId)
                            ? realityObjectStructuralElement[realtyObjectInfo.roId]
                            : new List<long>();
                        var currentRoMissingCeoIdsList = realObjMissingCommonEstObj.ContainsKey(realtyObjectInfo.roId)
                            ? realObjMissingCommonEstObj[realtyObjectInfo.roId].Where(x => !currentRoCeoIdsList.Contains(x)).ToList()
                            : new List<long>();

                        var tempCeoCount = ceoIds.Count - currentRoMissingCeoIdsList.Count();
                        var currentRoPercent = tempCeoCount != 0 ? currentRoCeoIdsList.Count / tempCeoCount.ToDecimal() : 0;
                        currentRoPercent = currentRoPercent > 1 ? 1 : currentRoPercent;

                        sumPercentByMu += currentRoPercent;
                        sectionRo["PercentOccup"] = currentRoPercent;

                        var percent = dictRoPercent.ContainsKey(realtyObjectInfo.roId)
                                          ? dictRoPercent[realtyObjectInfo.roId]
                                          : 0M;

                        sectionRo["PercentStrEl"] = percent;
                        listPercent.Add(percent);
                        listAllPercent.Add(percent);

                        foreach (var ceoId in ceoIds)
                        {
                            sectionRo[string.Format("haveElement{0}", ceoId)] = currentRoCeoIdsList.Contains(ceoId)
                                ? "1"
                                : currentRoMissingCeoIdsList.Contains(ceoId)
                                    ? "-"
                                    : "0";
                        }
                    }

                    var currentMuPercent = sumPercentByMu / realtyObjectsByMu.Value.Count;
                    sumPercent += currentMuPercent;
                    sectionMu["AverageMun"] = currentMuPercent;
                    sectionMu["PercentStrElMun"] = listPercent.Average();
                }

                var count = realityObjByMuDict.Select(y => y.Value).Count();
                var averageAllMun = count != 0 ? sumPercent / count : 0;
                reportParams.SimpleReportParams["AverageAllMun"] = averageAllMun;

                reportParams.SimpleReportParams["PercentStrElAllMun"] = listAllPercent.Average(); 
            }
        }

        public Dictionary<string, List<RealtyObjProxy>> GetRealtyObjects()
        {
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();

            using (this.Container.Using(realityObjectService))
            {
                return realityObjectService
                        .GetAll()
                        .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                        .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                         .Select(x => new
                         {
                             x.Municipality.Name,
                             roId = x.Id,
                             x.Address,
                             MoId = x.Municipality.Id,
                             MoParentId = x.Municipality.ParentMo != null ? x.Municipality.ParentMo.Id : (long?)null
                         })
                        .AsEnumerable()
                        .GroupBy(x => x.Name)
                        .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new RealtyObjProxy { roId = y.roId, Address = y.Address, MoId = y.MoId, MoParentId = y.MoParentId }).ToList()); 
            }
        }

        // получение групп конструктивных элементов по домам
        public Dictionary<long, List<long>> GetData(IQueryable<long> commonEstateObjectsQuery)
        {
            var roStructuralElementService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            using (this.Container.Using(roStructuralElementService))
            {
                return roStructuralElementService
                        .GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                        .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                            ceoId = x.StructuralElement.Group.CommonEstateObject.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(y => y.ceoId).Distinct().ToList()); 
            }
        }

        // получение отсутствующих объектов общего имущества по домам
        public Dictionary<long, List<long>> GetMissingCeo(IQueryable<long> commonEstateObjectsQuery)
        {
            var roMissingCeoService = this.Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            using (this.Container.Using(roMissingCeoService))
            {
                return roMissingCeoService
                        .GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                        .Where(x => commonEstateObjectsQuery.Contains(x.MissingCommonEstateObject.Id))
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                            ceoId = x.MissingCommonEstateObject.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(y => y.ceoId).Distinct().ToList()); 
            }
        }

        // заполнение вертикальной секции
        public IQueryable<long> CreateVerticalColums(ReportParams reportParams)
        {
            var commonEstateObjectService = this.Container.Resolve<IDomainService<CommonEstateObject>>();

            using (this.Container.Using(commonEstateObjectService))
            {
                var commonEstateObjectsQuery = commonEstateObjectService
                    .GetAll()
                    .Where(x => x.IncludedInSubjectProgramm);

                var commonEstateObjects =
                    commonEstateObjectsQuery
                    .Select(x => new { x.Id, x.Name })
                    .OrderBy(x => x.Name)
                    .ToList();


                var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GroupCost");

                foreach (var commonEstateObject in commonEstateObjects)
                {
                    verticalSection.ДобавитьСтроку();
                    verticalSection["GroupName"] = commonEstateObject.Name;
                    verticalSection["haveElement"] = string.Format("$haveElement{0}$", commonEstateObject.Id);
                }

                return commonEstateObjectsQuery.Select(x => x.Id); 
            }
        }

        private Dictionary<long, decimal> GetRoPercent(IQueryable<long> commonEstateObjectsQuery)
        {
            var roStructuralElementService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            using (this.Container.Using(roStructuralElementService))
            {
                return roStructuralElementService
                        .GetAll()
                        .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                            x.Volume
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(
                            x => x.Key,
                            v =>
                                {
                                    var countRo = v.Count();
                                    var countRoRes = v.Count(y => y.Volume > 0);
                                    return (decimal.Divide(countRoRes, countRo) * 100.0M);
                                }); 
            }
        }
    }
}

