namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Entities;
    using Entities;
    using Enums;
    using Enum;
    using Gkh.Report;

    public class OwnersProtocolsControlManOrgReport : MakingProtocolsOwnersControlReport
    {

        public override string Name
        {
            get
            {
                return "Контроль внесения прокотолов собраний собственников о выборе УО -2";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль внесения прокотолов собраний собственников о выборе УО -2";
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
                return "Reports.GKH.OwnersProtocolsControlManOrg";
            }
        }

        protected override Dictionary<long, ManagOrgInfoProxy> GetManOrgDictionary(IQueryable<long> realtyObjIdsQuery)
        {
            var currentDate = DateTime.Today.AddDays(1);

            var moContractDirectManagServ = Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();

            var manOrgContractRealityObjectQuery = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => realtyObjIdsQuery.Contains(x.RealityObject.Id))
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= currentDate)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= currentDate);

            var manOrgContractIdsQuery = manOrgContractRealityObjectQuery.Select(x => x.ManOrgContract.Id);

            var contractRelationParentIdsList = Container.Resolve<IDomainService<ManOrgContractRelation>>()
                    .GetAll()
                    .Where(x => manOrgContractIdsQuery.Contains(x.Parent.Id))
                    .Where(x => x.Children.StartDate <= currentDate)
                    .Where(x => x.Children.EndDate == null || x.Children.EndDate >= currentDate)
                    .Select(x => x.Parent.Id)
                    .Distinct()
                    .ToList();

            var propertyOwenrsProtocols = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();

            return manOrgContractRealityObjectQuery
                .Select(x => new
                {
                    RobjectId = x.RealityObject.Id,
                    ManOrgId = x.ManOrgContract.Id,
                    ManagingOrganizationName = x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag ?
                        moContractDirectManagServ.GetAll().Any(y => y.Id == x.ManOrgContract.Id && y.IsServiceContract) ? "Непосредственное управление с договором на оказание услуг" : "Непосредственное управление"
                        : x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    x.ManOrgContract.TypeContractManOrgRealObj,
                    TypeManagement = x.ManOrgContract.ManagingOrganization != null ? x.ManOrgContract.ManagingOrganization.TypeManagement.GetEnumMeta().Display : "Непосредственное управление",
                    FileInfo = propertyOwenrsProtocols.GetAll()
                                .Where(z => z.TypeProtocol == PropertyOwnerProtocolType.SelectManOrg)
                                .Where(z => z.RealityObject.Id == x.RealityObject.Id)
                                .Select(z => z.DocumentFile)
                                .FirstOrDefault()
                })
                .AsEnumerable()
                .GroupBy(x => x.RobjectId)
                .ToDictionary(
                x => x.Key,
                x =>
                {
                    var manOrgsList = x.Select(y => new ManagOrgInfoProxy
                    {
                        Id = y.ManOrgId,
                        Name = y.ManagingOrganizationName,
                        TypeContract = y.TypeContractManOrgRealObj,
                        TypeManagement = y.TypeManagement,
                        HaveDocument = y.FileInfo != null ? "Да" : "Нет"

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
}