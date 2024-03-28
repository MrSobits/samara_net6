namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;
    using Modules.Reforma;

    public class MakingProtocolsOwnersControlReport : BasePrintForm
    {
        public MakingProtocolsOwnersControlReport()
            : base(new ReportTemplateBinary(Properties.Resources.MakingProtocolsOwnersControl))
        {
        }

        /// <summary>
        /// Контейнер  
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис получение "Протоколы собственников помещений МКД" 
        /// </summary>
        public IPropertyOwnerProtocolsProvider PropertyOwnerProtocolsProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IManagingOrgRealityObjectService ManagingOrgRealityObjectService { get; set; }

        /// <summary>
        /// Связь между двумя договорами
        /// </summary>
        public IDomainService<ManOrgContractRelation> ManOrgContractRelationDomainService { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomainService { get; set; }


        #region Входящие параметры
        private long[] municipalityIds;
        #endregion

        public override string Name
        {
            get
            {
                return "Контроль внесения прокотолов собраний собственников о выборе УО";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль внесения прокотолов собраний собственников о выборе УО";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.MakingProtocolsOwnersControl";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.MakingProtocolsOwnersControl";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjectQuery = this.RealityObjectDomainService.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable || x.ConditionHouse == ConditionHouse.Dilapidated);

            var realtyObjectList = realtyObjectQuery
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    muName = x.Municipality.Name
                })
                .OrderBy(x => x.muName)
                .ThenBy(x => x.Address)
                .ToList();

            var realtyObjIdsQuery = realtyObjectQuery.Select(x => x.Id);
            var managOrgsDict = this.GetManOrgDictionary(realtyObjIdsQuery);
           
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realtyObject in realtyObjectList)
            {
                section.ДобавитьСтроку();
                section["muName"] = realtyObject.muName;
                section["roAddress"] = realtyObject.Address;

                if (managOrgsDict.ContainsKey(realtyObject.Id))
                {
                    var currentManOrg = managOrgsDict[realtyObject.Id];

                    section["manageType"] = currentManOrg.TypeManagement;
                    section["moName"] = currentManOrg.Name;
                    section["haveDocument"] = currentManOrg.HaveDocument;
                }
            }
        }

        protected virtual Dictionary<long, ManagOrgInfoProxy> GetManOrgDictionary(IQueryable<long> realtyObjIdsQuery)
        {
            var currentDate = DateTime.Today.AddDays(1);

                var propertyOwnerProtocolsDic = this.PropertyOwnerProtocolsProvider.GetData(realtyObjIdsQuery);

                var manOrgContractRealityObjectQuery = this.ManagingOrgRealityObjectService.GetAllActive(currentDate)
                    .Where(x => realtyObjIdsQuery.Contains(x.RealityObject.Id));

                var manOrgContractIdsQuery = manOrgContractRealityObjectQuery.Select(x => x.ManOrgContract.Id);

                var contractRelationParentIdsList = this.ManOrgContractRelationDomainService.GetAll()
                    .Where(x => manOrgContractIdsQuery.Contains(x.Parent.Id))
                    .Where(x => x.Children.StartDate <= currentDate)
                    .Where(x => x.Children.EndDate == null || x.Children.EndDate >= currentDate)
                    .Select(x => x.Parent.Id)
                    .Distinct()
                    .ToList();

                return manOrgContractRealityObjectQuery
                    .Select(
                        x => new
                        {
                            RobjectId = x.RealityObject.Id,
                            ManOrgId = x.ManOrgContract.Id,
                            ManagingOrganizationName =
                                x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                    ? "Непосредственное управление"
                                    : x.ManOrgContract.ManagingOrganization.Contragent.Name,
                            x.ManOrgContract.TypeContractManOrgRealObj,
                            TypeManagement = x.ManOrgContract.ManagingOrganization == null 
                                ? "Непосредственное управление" 
                                : x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners 
                                    ? "УК" 
                                    : x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj
                                        ? x.ManOrgContract.ManagingOrganization.TypeManagement.GetEnumMeta().Display 
                                        : "ТСЖ",
                            FileInfo = x.ManOrgContract.FileInfo != null ? "Да" : "Нет"
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RobjectId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var manOrgsList = x.Select(
                                y => new ManagOrgInfoProxy
                                {
                                    Id = y.ManOrgId,
                                    Name = y.ManagingOrganizationName,
                                    TypeContract = y.TypeContractManOrgRealObj,
                                    TypeManagement = y.TypeManagement,
                                    HaveDocument =
                                        y.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                            ? ""
                                            : propertyOwnerProtocolsDic.ContainsKey(y.RobjectId) ? "Да" : "Нет"

                                })
                                .ToList();

                            var res = new ManagOrgInfoProxy();

                            if (manOrgsList.Count == 1)
                            {
                                res = manOrgsList.First();
                            }

                            if (manOrgsList.Count == 2 && manOrgsList.Any(y => y.TypeContract == TypeContractManOrg.JskTsj))
                            {
                                var tsj = manOrgsList.FirstOrDefault(y => y.TypeContract == TypeContractManOrg.JskTsj);
                                if (tsj != null && contractRelationParentIdsList.Contains(tsj.Id))
                                {
                                    res = tsj;
                                }
                            }

                            return res;
                        });
        }
    }

    public class ManagOrgInfoProxy
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public TypeContractManOrg TypeContract { get; set; }

        public string TypeManagement { get; set; }

        public string HaveDocument { get; set; }
    }
}