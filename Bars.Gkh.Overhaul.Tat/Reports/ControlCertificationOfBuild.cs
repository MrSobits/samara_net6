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

    class ControlCertificationOfBuild : BasePrintForm
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
            var manOrgContractRoService = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();
            var roStructuralElementService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roMissingCeoService = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            using (this.Container.Using(manOrgContractRoService, realityObjectService, roStructuralElementService, roMissingCeoService))
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

                var realityObjectStructuralElementKeys = realityObjectStructuralElement.Select(x => x.Key).ToList();

                var structuralElementInfo = roStructuralElementService
                    .GetAll()
                    .Where(x => realityObjectStructuralElementKeys.Contains(x.RealityObject.Id))
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                    .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                    .Where(x => x.RealityObject.BuildYear != null)
                    .Select(x => new
                    {
                        RealObjId = x.RealityObject.Id,
                        x.LastOverhaulYear,
                        x.Volume,
                        CommonEstateObjectId = x.StructuralElement.Group.CommonEstateObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => new
                    {
                        x.RealObjId,
                        x.CommonEstateObjectId
                    })
                    .ToDictionary(x => x.Key, x => x.ToList());

                var missingStructuralElementInfo = roMissingCeoService
                    .GetAll()
                    .Where(x => realityObjectStructuralElementKeys.Contains(x.RealityObject.Id))
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                    .Where(x => x.RealityObject.BuildYear != null)
                    .Select(x => new
                    {
                        RealObjId = x.RealityObject.Id,
                        CommonEstateObjectId = x.MissingCommonEstateObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RealObjId)
                    .ToDictionary(x => x.Key, x => x.ToList());

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

                            //для расчета процентов
                            var countCeo = 0;

                            //если у констр характер в доме, в группе есть хотябы одна запись где не заполнены: Объем или 
                            //год установки, всему Констр элемету ставим 0
                            foreach (var ceoId in ceoIds)
                            {
                                var roId = realtyObjectInfo.roId;

                                //выбираем по текущему дому, группу элементов
                                var structuralElementListInfo = structuralElementInfo.Where(x => x.Key.RealObjId == roId)
                                    .Where(x => x.Key.CommonEstateObjectId == ceoId);

                                sectionRo[string.Format("haveElement{0}", ceoId)] = 0;

                                foreach (var structuralElementByCeo in structuralElementListInfo.Select(x => x.Value))
                                {

                                    //общие количество элементов в группе
                                    var structuralElementCount = structuralElementByCeo.Count;

                                    //элементы с заполнненым объемом и годом
                                    var filledElement = 0;

                                    foreach (var structuralElement in structuralElementByCeo)
                                    {
                                        if (structuralElement.Volume != 0 && structuralElement.LastOverhaulYear != 0)
                                        {
                                            filledElement += 1;
                                        }
                                    }
                                    //если заполненых равное колво общего числа ставим 1
                                    if (filledElement >= structuralElementCount)
                                    {
                                        sectionRo[string.Format("haveElement{0}", ceoId)] = 1;

                                        countCeo += 1;
                                    }
                                }

                                // Отсутствие конструктивных элементов
                                var missingStructuralElementListInfo = missingStructuralElementInfo.Where(x => x.Key == roId);
                                foreach (var missingStructuralElementList in missingStructuralElementListInfo.Select(x => x.Value))
                                {
                                    foreach (var missingStructuralElemen in missingStructuralElementList)
                                    {
                                        if (missingStructuralElemen.CommonEstateObjectId == ceoId)
                                        {
                                            sectionRo[string.Format("haveElement{0}", ceoId)] = "-";
                                        }
                                    }
                                }
                            }

                            var currentRoPercent = ceoIds.Count != 0 ? countCeo / ceoIds.Count.ToDecimal() : 0;
                            sumPercentByMu += currentRoPercent;
                            sumPercentByManOrg += currentRoPercent;
                            sectionRo["PercentOccup"] = currentRoPercent;
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

        public Dictionary<string, List<RealtyObjProxy>> GetRealtyObjects()
        {
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();

            using (this.Container.Using(realityObjectService))
            {
                return realityObjectService
                        .GetAll()
                        .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                        .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                         .Select(x => new
                         {
                             x.Municipality.Name,
                             roId = x.Id,
                             x.Address
                         })
                        .AsEnumerable()
                        .GroupBy(x => x.Name)
                        .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new RealtyObjProxy { roId = y.roId, Address = y.Address }).ToList()); 
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
                        .Where(x => x.RealityObject.BuildYear != null)
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                    //groupId = x.StructuralElement.Group.Id
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

        public long? MoId;

        public long? MoParentId;
    }
}

