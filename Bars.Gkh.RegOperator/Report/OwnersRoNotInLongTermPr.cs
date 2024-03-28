using Bars.Gkh.Overhaul.DomainService;

namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class OwnersRoNotInLongTermPr : BasePrintForm
    {
        public OwnersRoNotInLongTermPr()
            : base(new ReportTemplateBinary(Properties.Resources.OwnersRoNotInLongTermPr))
        {
        }

        public IWindsorContainer Container { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        #region Входящие параметры
        private long[] municipalityIds;
        private long[] parentMoIds;
        private DateTime actualityDate;
        #endregion

        public override string Name
        {
            get
            {
                return "Собственники МКД, отсутствующих в ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Собственники МКД, отсутствующих в ДПКР";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.OwnersRoNotInLongTermPr";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhRegOp.OwnersRoNotInLongTermPr";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var parentMoIdsList = baseParams.Params.GetAs("parentMoIds", string.Empty);
            parentMoIds = !string.IsNullOrEmpty(parentMoIdsList)
                                  ? parentMoIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
            
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            actualityDate = baseParams.Params.GetAs("actualityDate", DateTime.MinValue);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjectsInDpkrService = this.Container.ResolveAll<IRealityObjectDpkrDataService>().FirstOrDefault();
            if (realtyObjectsInDpkrService == null)
            {
                throw new Exception("Не найдена реализация интерфейса IRealityObjectDpkrDataService");
            }

            var realtyObjectsInDpkr = realtyObjectsInDpkrService.GetRobjectsInProgram().Select(x => x.Id);

            var realityObjectsNotInDpkrQuery = this.RealityObjectDomain.GetAll()
                .Where(x => !realtyObjectsInDpkr.Contains(x.Id))
                .WhereIf(parentMoIds.Length > 0, x => parentMoIds.Contains(x.Municipality.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MoSettlement.Id));

            var realityObjectsNotInDpkrIdsQuery = realityObjectsNotInDpkrQuery.Select(x => x.Id);

            var personalAccountsQuery = this.BasePersonalAccountDomain.GetAll()
                    .Where(x => x.Room.RealityObject != null)
                    .Where(x => realityObjectsNotInDpkrIdsQuery.Contains(x.Room.RealityObject.Id));

            var personalAccountsIdsQuery = personalAccountsQuery.Select(x => (long)x.Id);
            
            var personalAccountsActualAreaShare = this.EntityLogLightDomain.GetAll()
                    .Where(x => x.ClassName == "BasePersonalAccount")
                    .Where(x => x.PropertyName == "AreaShare")
                    .Where(x => personalAccountsIdsQuery.Contains(x.EntityId))
                    .Where(x => x.DateActualChange < this.actualityDate.AddDays(1))
                    .Select(x => new
                    {
                        x.EntityId,
                        x.DateActualChange,
                        x.PropertyValue
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.EntityId)
                    .ToDictionary(
                    x => x.Key,
                    x => x.OrderByDescending(y => y.DateActualChange).First().PropertyValue.ToDecimal());

            var personalAccounts = personalAccountsQuery
                    .Select(x => new { RoId = x.Room.RealityObject.Id, x.PersonalAccountNum, x.AreaShare, x.Id })
                    .AsEnumerable()
                    .Select(x => new { x.RoId, x.PersonalAccountNum, 
                        AreaShare = personalAccountsActualAreaShare.ContainsKey(x.Id) 
                        ? personalAccountsActualAreaShare[x.Id]
                        : 0
                    })
                    .Where(x => x.AreaShare > 0)
                    .Select(x => new { x.RoId, x.PersonalAccountNum })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, x => x.ToList());

            var realityObjectsNotInDpkr = realityObjectsNotInDpkrQuery
                .Select(x => new
                {
                    RoId = x.Id,
                    x.Address,
                    ParentMuId = x.Municipality.Id,
                    MuId = (long?)x.MoSettlement.Id,
                    ParentMuName = x.Municipality.Name,
                    MuName = x.MoSettlement.Name
                })
                .OrderBy(x => x.ParentMuName)
                .ThenBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .AsEnumerable()
                .GroupBy(x => new { x.ParentMuId, x.ParentMuName })
                .Select(x => new
                    {
                        ParentMu = x.Key,
                        Children = x.GroupBy(y => new { y.MuId, y.MuName })
                            .Select(y => 
                                {
                                    var data = y.Where(z => personalAccounts.ContainsKey(z.RoId))
                                        .Select(z => new
                                        {
                                            z.RoId,
                                            z.Address,
                                            AccountData = personalAccounts[z.RoId]
                                        })
                                        .ToList();

                                    return new { ChildMo = y.Key, data };
                                })
                            .Where(y => y.data.Count > 0)
                            .ToList()
                    })
                .Select(x => new 
                    {
                        x.ParentMu,
                        x.Children,
                        anyAccount = x.Children.Any(y => y.data.Any())
                    })
                .Where(x => x.anyAccount)
                .ToList();

            var sectionParentMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionParentMu");
            var sectionMu = sectionParentMu.ДобавитьСекцию("sectionMu");
            var sectionMuName = sectionMu.ДобавитьСекцию("sectionMuName");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");

            foreach (var realityObjectsByParentMu in realityObjectsNotInDpkr)
            {
                sectionParentMu.ДобавитьСтроку();
                sectionParentMu["parentMoName"] = realityObjectsByParentMu.ParentMu.ParentMuName;

                foreach (var realityObjectsByMu in realityObjectsByParentMu.Children)
                {
                    sectionMu.ДобавитьСтроку();
                    if (realityObjectsByMu.ChildMo.MuName != null)
                    {
                        sectionMuName.ДобавитьСтроку();
                        sectionMuName["moName"] = realityObjectsByMu.ChildMo.MuName;
                    }

                    var num = 0;

                    foreach (var realtyObject in realityObjectsByMu.data)
                    {
                        ++num;

                        var roAccounts = realtyObject.AccountData;

                        foreach (var roAccount in roAccounts)
                        {
                            sectionRo.ДобавитьСтроку();
                            sectionRo["num"] = num.ToStr();
                            sectionRo["address"] = realtyObject.Address;
                            sectionRo["account"] = roAccount.PersonalAccountNum;
                        }
                    }
                }
            }
        }
    }
}