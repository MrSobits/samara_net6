namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;

    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class ControlCertificationOfBuild : BasePrintForm
    {
        public ControlCertificationOfBuild()
            : base(new ReportTemplateBinary(Resources.ControlCertificationOfBuild))
        {
        }

        private long[] municipalityIds;
        private int[] houseTypes;
        public IWindsorContainer Container { get; set; }
        public override string Name
        {
            get { return "Контроль паспортизации домов"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ControlCertificationOfBuild";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.ControlCertificationOfBuild";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                                  ? houseTypesList.Split(',').Select(id => id.ToInt()).ToArray()
                                  : new int[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var manOrgContractRoService = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();

            using (this.Container.Using(manOrgContractRoService, realityObjectService))
            {
                var commonEstateObjectsQuery = this.CreateVerticalColums(reportParams);
                var ceoIds = commonEstateObjectsQuery.ToList();

                var manOrgByRoIdDict = manOrgContractRoService
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                    .Where(x => x.ManOrgContract.ManagingOrganization != null)
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                    .Select(x => new
                    {
                        moName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                        roId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.moName).ToList());

                var realtyObjByMuDict = realityObjectService
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                    .Select(x => new
                    {
                        x.Municipality.Name,
                        roId = x.Id,
                        x.Address
                    })
                    .AsEnumerable()
                    .SelectMany(x =>
                        {
                            if (manOrgByRoIdDict.ContainsKey(x.roId))
                            {
                                var manOrgs = manOrgByRoIdDict[x.roId];

                                return manOrgs
                                    .Select(y => new RealtyObjProxy
                                    {
                                        MuName = x.Name,
                                        roId = x.roId,
                                        Address = x.Address,
                                        MoName = y
                                    })
                                    .ToList();
                            }
                            else
                            {
                                return new List<RealtyObjProxy>
                                           {
                                           new RealtyObjProxy
                                               {
                                                   MuName = x.Name,
                                                   roId = x.roId,
                                                   Address = x.Address,
                                                   MoName = string.Empty
                                               }
                                           };
                            }
                        })
                    .GroupBy(x => x.MuName)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var realityObjectStructuralElement = this.GetData(commonEstateObjectsQuery);

                var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
                var sectionManOrg = sectionMu.ДобавитьСекцию("sectionManOrg");
                var sectionRo = sectionManOrg.ДобавитьСекцию("sectionRo");
                var num = 1;
                var sumPercent = 0M;

                foreach (var realtyObjectsByMu in realtyObjByMuDict.OrderBy(x => x.Key))
                {
                    sectionMu.ДобавитьСтроку();
                    sectionMu["MuName"] = realtyObjectsByMu.Key;
                    var sumPercentByMu = 0M;

                    foreach (var manOrgGrouped in realtyObjectsByMu.Value.GroupBy(x => x.MoName).OrderBy(x => x.Key))
                    {
                        sectionManOrg.ДобавитьСтроку();

                        var sumPercentByManOrg = 0M;

                        foreach (var realtyObjectInfo in manOrgGrouped.OrderBy(x => x.Address))
                        {
                            sectionRo.ДобавитьСтроку();
                            sectionRo["Num"] = num++;
                            sectionRo["MO"] = realtyObjectsByMu.Key;
                            sectionRo["ManOrg"] = realtyObjectInfo.MoName;
                            sectionRo["Address"] = realtyObjectInfo.Address;

                            var currentRoCeoIdsList = realityObjectStructuralElement.ContainsKey(realtyObjectInfo.roId)
                                ? realityObjectStructuralElement[realtyObjectInfo.roId]
                                : new List<long>();

                            var currentRoPercent = ceoIds.Count != 0 ? currentRoCeoIdsList.Count / ceoIds.Count.ToDecimal() : 0;
                            sumPercentByMu += currentRoPercent;
                            sumPercentByManOrg += currentRoPercent;
                            sectionRo["PercentOccup"] = currentRoPercent;

                            foreach (var ceoId in ceoIds)
                            {
                                var haveConElem = currentRoCeoIdsList.Contains(ceoId) ? 1 : 0;
                                sectionRo[string.Format("haveElement{0}", ceoId)] = haveConElem;
                            }
                        }

                        sectionManOrg["ManOrg"] = manOrgGrouped.Key;
                        sectionManOrg["AverageManOrg"] = sumPercentByManOrg / manOrgGrouped.Count();
                    }

                    var currentMuPercent = sumPercentByMu / realtyObjectsByMu.Value.Count;
                    sumPercent += currentMuPercent;
                    sectionMu["AverageMun"] = currentMuPercent;
                }

                var count = realtyObjByMuDict.Select(y => y.Value).Count();
                reportParams.SimpleReportParams["AverageAllMun"] = sumPercent / (count != 0 ? count : 1); 
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
                        .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                        .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                    // groupId = x.StructuralElement.Group.Id
                    ceoId = x.StructuralElement.Group.CommonEstateObject.Id
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
    }

    internal sealed class RealtyObjProxy
    {
        public long roId;

        public string Address;

        public string MuName;

        public string MoName;
    }
}

