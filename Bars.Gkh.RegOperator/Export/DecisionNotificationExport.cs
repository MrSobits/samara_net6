using Bars.Gkh.Decisions.Nso.Domain;

namespace Bars.Gkh.RegOperator.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;

    public class DecisionNotificationExport : BaseDataExportService
    {
        public IDomainService<ManagingOrgRealityObject> RoManOrgRoDomain { get; set; }

        public IDomainService<RegOperator> RegOperatorDomain { get; set; }

        public IDomainService<CreditOrgDecision> CreditOrgDecisionDomain { get; set; }

        public IDomainService<DecisionNotification> DecisionNotificationDomain { get; set; }

        public IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain { get; set; }

        public IRealityObjectDecisionsService RoDecisionService { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            // Чтобы правильно работали фильтры и сортировка у меня не было выхода как тупо все вытянуть на сервак

            var manOrgByRoDict = RoManOrgRoDomain.GetAll()
                .Where(x => DecisionNotificationDomain.GetAll().Any(y => x.RealityObject.Id == y.Protocol.RealityObject.Id))
                .Where(x => x.ManagingOrganization.ActivityDateEnd == null)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ContragentName = x.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.First().ContragentName);

            var regopName = RegOperatorDomain.GetAll()
                .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                .Select(x => x.Contragent.Name)
                .FirstOrDefault();

             var accountOwnerDecision = AccountOwnerDecisionDomain.GetAll()
                .Where(x => DecisionNotificationDomain.GetAll().Any(y => x.Protocol.RealityObject.Id == y.Protocol.RealityObject.Id))
                .Select(x => new { x.Protocol.RealityObject.Id, x.DecisionType })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().DecisionType);

            var creditOrgByProtocolDict = CreditOrgDecisionDomain.GetAll()
                .Select(x => new { x.Protocol.Id, x.Decision.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().Name);

            var roDecisDict = RoDecisionService.GetRobjectsFundFormation(((IQueryable<long>) null));

            var result = DecisionNotificationDomain.GetAll()
                .Select(x => new
                {
                    roId = (long?)x.Protocol.RealityObject.Id,
                    protocolId = (long?)x.Protocol.Id,
                    x.Id,
                    Number = x.Number ?? string.Empty,
                    Date = x.Date <= DateTime.MinValue ? (DateTime?)null : x.Date,
                    AccountNum = x.AccountNum != null ? "'" + x.AccountNum : string.Empty, //показываем счет в текстовом формате в отчете
                    OpenDate = x.OpenDate <= DateTime.MinValue ? (DateTime?)null : x.OpenDate,
                    CloseDate = x.CloseDate <= DateTime.MinValue ? (DateTime?)null : x.CloseDate,
                    IncomeNum = x.IncomeNum ?? string.Empty,
                    RegistrationDate = x.RegistrationDate <= DateTime.MinValue ? (DateTime?)null : x.RegistrationDate,
                    Address = x.Protocol.RealityObject.Address ?? string.Empty,
                    Mu = x.Protocol.RealityObject.Municipality.Name ?? string.Empty,
                    MoSettlement = x.Protocol.RealityObject.MoSettlement.Name ?? string.Empty,
                    x.State,
                    HasCertificate = x.BankDoc != null ? "Да" : "Нет"
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.Date,
                    x.AccountNum,
                    x.OpenDate,
                    x.CloseDate,
                    x.IncomeNum,
                    x.RegistrationDate,
                    x.Address,
                    FormFundType = x.roId.HasValue && roDecisDict.ContainsKey(x.roId.Value) ? roDecisDict[x.roId.Value].First().Item2.GetEnumMeta().Display : string.Empty,
                    x.Mu,
                    x.MoSettlement,
                    x.State,
                    x.HasCertificate,
                    OrgName = x.roId.HasValue && accountOwnerDecision.ContainsKey(x.roId.Value) && accountOwnerDecision[x.roId.Value] == AccountOwnerDecisionType.Custom
                        ? (manOrgByRoDict.ContainsKey(x.roId.Value) ? manOrgByRoDict[x.roId.Value] : string.Empty)
                        : regopName,
                    CreditOrgName = x.protocolId.HasValue && creditOrgByProtocolDict.ContainsKey(x.protocolId.Value)
                        ? creditOrgByProtocolDict[x.protocolId.Value]
                        : string.Empty
                })
                .AsQueryable()
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();

            return result;
        }
    }
}