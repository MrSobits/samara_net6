namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Domain.CollectionExtensions;
    using Entities;
    using GkhRf.Entities;
    using Gkh.Domain;

    public class ChargeReport : BasePrintForm
    {
        public ChargeReport()
            : base(new ReportTemplateBinary(Properties.Resources.ChargeReport))
        {
        }

        #region Properties

        public IDomainService<TransferObject> TransferObjectDomain { get; set; }
        public IDomainService<ContractRfObject> ContractRfObjectDomain { get; set; }
        public IDomainService<RealityObjectChargeAccountOperation> RealityObjectChargeAccountOperationDomain { get; set; }
        
        private long[] municipalityIds;

        private long[] manOrgIds;

        private long[] stateIds;

        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Отчет по начислениям"; }
        }

        public override string Desciption
        {
            get { return "Отчет по начислениям"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ChargeReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.ChargeReport"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
            
            manOrgIds = baseParams.Params.GetAs("manOrgIds", string.Empty).ToLongArray();

            stateIds = baseParams.Params.GetAs("stateIds", string.Empty).ToLongArray();

            startDate = baseParams.Params.GetAs<DateTime>("startDate");
            endDate = baseParams.Params.GetAs<DateTime>("endDate");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var contractObjectsQuery = ContractRfObjectDomain.GetAll()
                .Where(x => x.ContractRf.DocumentDate >= new DateTime(2014, 1, 1))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(manOrgIds.Length > 0, x => manOrgIds.Contains(x.ContractRf.ManagingOrganization.Id));

            var transfersFoundByManorgDict = TransferObjectDomain.GetAll()
                .Where(x => contractObjectsQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .Where(x => x.TransferRecord.DateFrom >= startDate)
                .Where(x => x.TransferRecord.DateFrom <= endDate)
                .WhereIf(stateIds.Length > 0, x => stateIds.Contains(x.TransferRecord.State.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    x.TransferredSum,
                    manOrgId = (long?)x.TransferRecord.TransferRf.ContractRf.ManagingOrganization.Id
                })
                .AsEnumerable()
                .Where(x => x.manOrgId.HasValue)
                .GroupBy(x => x.manOrgId.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.roId)
                          .ToDictionary(y => y.Key, y => y.Sum(z => z.TransferredSum ?? 0)));

            var roOperations = RealityObjectChargeAccountOperationDomain.GetAll()
                .Where(x => contractObjectsQuery.Any(y => y.RealityObject.Id == x.Account.RealityObject.Id))
                .Where(x => x.Date >= startDate)
                .Where(x => x.Date <= endDate)
                .GroupBy(x => x.Account.RealityObject.Id)
                .Select(x => new
                    {
                        x.Key,
                        charged = x.Sum(y => y.ChargedPenalty + y.ChargedTotal),
                        paid = x.Sum(y => y.PaidPenalty + y.PaidTotal)
                    })
                .ToDictionary(x => x.Key);

            var data = contractObjectsQuery
                .Select(x => new
                {
                    manorgName = x.ContractRf.ManagingOrganization.Contragent.Name,
                    manOrgId = x.ContractRf.ManagingOrganization.Id,
                    municipalityName = x.RealityObject.Municipality.Name,
                    address = x.RealityObject.Address,
                    roId = x.RealityObject.Id,
                })
                .AsEnumerable()
                .GroupBy(x => x.municipalityName)
                .Select(x => new
                {
                    name = x.Key,
                    data = x.GroupBy(y => new { y.manOrgId, y.manorgName })
                        .Select(y =>
                        {
                            var transfers = transfersFoundByManorgDict.Get(y.Key.manOrgId)
                                            ?? new Dictionary<long, decimal>();

                            var robjectList = y
                                .Select(z =>
                                {
                                    var transfer = transfers.Get(z.roId);
                                    var charged = 0m;
                                    var paid = 0m;

                                    if (roOperations.ContainsKey(z.roId))
                                    {
                                        var operations = roOperations[z.roId];

                                        charged = operations.charged;
                                        paid = operations.paid;
                                    }

                                    return new
                                    {
                                        z.address,
                                        z.roId,
                                        transfer,
                                        charged,
                                        paid,
                                        debt = charged - paid,
                                        reminder = paid - transfer
                                    };
                                })
                                .ToArray()
                                .Distinct();

                            return new { y.Key.manOrgId, y.Key.manorgName, robjectList };
                        })
                        .ToArray()
                })
                .ToArray();

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");
            var sectionManagingOrganization = sectionMunicipality.ДобавитьСекцию("sectionManagingOrganization");
            var sectionRealtyObject = sectionManagingOrganization.ДобавитьСекцию("sectionRealtyObject");

            foreach (var municipality in data.OrderBy(x => x.name))
            {
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["municipalityName"] = municipality.name;

                foreach (var manOrg in municipality.data.OrderBy(x => x.manorgName))
                {
                    sectionManagingOrganization.ДобавитьСтроку();
                    sectionManagingOrganization["manOrgName"] = manOrg.manorgName;
                    
                    var num = 0;

                    foreach (var realtyObject in manOrg.robjectList.OrderBy(x => x.address))
                    {
                        sectionRealtyObject.ДобавитьСтроку();
                        sectionRealtyObject["num"] = ++num;
                        sectionRealtyObject["manOrgName"] = manOrg.manorgName;
                        sectionRealtyObject["address"] = realtyObject.address;
                        sectionRealtyObject["charged"] = realtyObject.charged;
                        sectionRealtyObject["paid"] = realtyObject.paid;
                        sectionRealtyObject["debt"] = realtyObject.debt;
                        sectionRealtyObject["transferred"] = realtyObject.transfer;
                        sectionRealtyObject["remainder"] = realtyObject.reminder;
                    }

                    sectionManagingOrganization["chargedMo"] = manOrg.robjectList.Sum(x => x.charged);
                    sectionManagingOrganization["paidMo"] = manOrg.robjectList.Sum(x => x.paid); 
                    sectionManagingOrganization["debtMo"] = manOrg.robjectList.Sum(x => x.debt); 
                    sectionManagingOrganization["transferredMo"] = manOrg.robjectList.Sum(x => x.transfer);
                    sectionManagingOrganization["remainderMo"] = manOrg.robjectList.Sum(x => x.reminder); 
                }

                sectionMunicipality["chargedMu"] = municipality.data.Sum(x => x.robjectList.Sum(y => y.charged));
                sectionMunicipality["paidMu"] = municipality.data.Sum(x => x.robjectList.Sum(y => y.paid));
                sectionMunicipality["debtMu"] = municipality.data.Sum(x => x.robjectList.Sum(y => y.debt));
                sectionMunicipality["transferredMu"] = municipality.data.Sum(x => x.robjectList.Sum(y => y.transfer));
                sectionMunicipality["remainderMu"] = municipality.data.Sum(x => x.robjectList.Sum(y => y.reminder));
            }

            reportParams.SimpleReportParams["chargedTotal"] = data.SafeSum(z => z.data.Sum(x => x.robjectList.Sum(y => y.charged)));
            reportParams.SimpleReportParams["paidTotal"] = data.SafeSum(z => z.data.Sum(x => x.robjectList.Sum(y => y.paid)));
            reportParams.SimpleReportParams["debtTotal"] = data.SafeSum(z => z.data.Sum(x => x.robjectList.Sum(y => y.debt)));
            reportParams.SimpleReportParams["transferredTotal"] = data.SafeSum(z => z.data.Sum(x => x.robjectList.Sum(y => y.transfer)));
            reportParams.SimpleReportParams["remainderTotal"] = data.SafeSum(z => z.data.Sum(x => x.robjectList.Sum(y => y.reminder)));
        }
    }
}