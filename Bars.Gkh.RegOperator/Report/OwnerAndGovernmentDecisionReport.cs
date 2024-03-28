namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class OwnerAndGovernmentDecisionReport : BasePrintForm
    {
        private List<long> muIds;
        private List<long> roIds;
        private int decisionType;

        #region DomainServices

        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomain { get; set; }
        public IDomainService<GovDecision> GovDecisionDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }
        public IDomainService<CrFundFormationDecision> CrFundFormationDecisionDomain { get; set; }
        public IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain { get; set; }
        public IDomainService<MonthlyFeeAmountDecision> MonthlyFeeAmountDecDomain { get; set; }
        public IDomainService<CreditOrgDecision> CreditOrgDecisionDomain { get; set; }
        public IDomainService<MinFundAmountDecision> MinFundAmountDecisionDomain { get; set; }
        public IDomainService<AccumulationTransferDecision> AccumulationTransferDecisionDomain { get; set; }
        public IDomainService<JobYearDecision> JobYearDecisionDomain { get; set; }
        public IRepository<RealityObject> RealityObjectRepo { get; set; }
        public IGkhParams GkhParams { get; set; }

        #endregion

        #region Properties && .ctor

        public OwnerAndGovernmentDecisionReport()
            : base(new ReportTemplateBinary(Properties.Resources.OwnerAndGovernmentDecisionReport))
        {
        }

        public override string Name { get { return "Отчет о решениях собственников и ОГВ"; } }

        public override string Desciption { get { return "Отчет о решениях собственников и ОГВ"; } }

        public override string GroupName { get { return "Протоколы решений"; } }

        public override string ParamsController { get { return "B4.controller.report.OwnerAndGovernmentDecisionReport"; } }

        public override string RequiredPermission { get { return "Reports.GkhRegOp.OwnerAndGovernmentDecisionReport"; } }

        public override string ReportGenerator { get; set; }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("muIds", string.Empty);
            muIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToList()
                                  : new List<long>();

            var roIdsList = baseParams.Params.GetAs("roIds", string.Empty);
            roIds = !string.IsNullOrEmpty(roIdsList)
                                  ? roIdsList.Split(',').Select(id => id.ToLong()).ToList()
                                  : new List<long>();

            decisionType = baseParams.Params.GetAs<int>("decisionType");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Dictionary<long, string> manOrgRoDict = null;
            Dictionary<long, string> crFundFormDecDict = null;

            var appParams = GkhParams.GetParams();
            var moLevel = appParams.ContainsKey("MoLevel") && !appParams["MoLevel"].ToString().IsEmpty()
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            if (decisionType != 3)
            {
                // строим Dictionary для получения Управления домом по Id дома
                manOrgRoDict =
                    ManOrgContractRealityObjectDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(x.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.RealityObject.Id))
                        .OrderByDescending(x => x.ManOrgContract.StartDate)
                        .Select(x => new
                        {
                            x.RealityObject.Id,
                            x.ManOrgContract.TypeContractManOrgRealObj,
                            ManOrgName = x.ManOrgContract.ManagingOrganization != null
                                ? x.ManOrgContract.ManagingOrganization.Contragent.Name
                                : string.Empty
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key,
                            x =>
                            {
                                var first = x.FirstOrDefault();
                                return first != null
                                    ? first.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                        ? first.TypeContractManOrgRealObj.GetEnumMeta().Display
                                        : first.ManOrgName
                                    : string.Empty;
                            });

                // строим Dictionary для получения способа формирования фонда КР по Id протокола
                crFundFormDecDict =
                        CrFundFormationDecisionDomain.GetAll()
                            .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion 
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                            .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                            .Select(x => new
                            {
                                x.Protocol.Id,
                                x.Decision
                            })
                            .AsEnumerable()
                            .ToDictionary(x => x.Id, x => x.Decision.GetEnumMeta().Display);
            }
            
            #region ЕСЛИ ВЫБРАН ПАРАМЕТР ПРОТОКОЛ РЕШЕНИЙ СОБСТВЕННИКОВ

            if (decisionType == 1)
            {
                // строим Dictionary для получения типа владельца счета по Id протокола
                var accOwnDecDict =
                    AccountOwnerDecisionDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Protocol.Id,
                            x.DecisionType
                        })
                        .AsEnumerable()
                        .ToDictionary(x => x.Id, x => x.DecisionType.GetEnumMeta().Display);

                // строим Dictionary для получения информации о размере ежемес. взноса по Id протокола
                var monthlyFeeDict =
                    MonthlyFeeAmountDecDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Protocol.Id,
                            x.Decision
                        })
                        .AsEnumerable()
                        .ToDictionary(x => x.Id,
                            x => x
                                .Decision
                                .Where(y => y.From <= DateTime.Now)
                                .Where(y => !y.To.HasValue || y.To >= DateTime.Now)
                                .OrderByDescending(y => y.From)
                                .FirstOrDefault());

                // строим Dictionary для получения информации о кредитной организации по Id протокола
                var credOrgDecDict =
                    CreditOrgDecisionDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Protocol.Id,
                            x.Decision
                        })
                        .ToDictionary(x => x.Id,
                            x => x.Decision != null ? x.Decision.Name : string.Empty);

                // строим Dictionary для средств к зачислению за счет ранее собранных по Id протокола
                var accumTransfDecDict =
                    AccumulationTransferDecisionDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                        .ToDictionary(x => x.Protocol.Id, x => x.Decision);

                // создаем вертикальную секцию для данных протокола решений собственников
                var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("OwnerDecisSection");
                verticalSection.ДобавитьСтроку();

                var jobList =
                    JobYearDecisionDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Protocol.RealityObject.Municipality.Id
                                    : x.Protocol.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Protocol.Id,
                            x.JobYears
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .Select(x => x.First())
                        .ToList();

                // получаю все возможные названия работ для моей выборки протоколов решений
                var jobCodes = jobList
                    .SelectMany(x => x.JobYears)
                    .OrderBy(x => x.UserYear)
                    .Select(x => x.Job)
                    .Where(x => x != null)
                    .GroupBy(x => x.Name)
                    .Select(x => x.First())
                    .ToList();

                // строим Dictionary для JobYears по Id протокола
                var realtyJobYears = jobList.ToDictionary(x => x.Id, x => x.JobYears);


                // создаю вертикальную секцию для видов работ и годов выполнения
                if (jobCodes.Any())
                {
                    var ooiSection = verticalSection.ДобавитьСекцию("OoiYear");
                    foreach (var jobCode in jobCodes)
                    {
                        ooiSection.ДобавитьСтроку();
                        ooiSection["JobName"] = jobCode.Name;
                        ooiSection["Year"] = string.Format("$Year{0}$", jobCode.Id);
                    }
                }

                // получаем протоколы решений собственников согласно входным параметрам отчета
                var ownerDecProtocols =
                    RealityObjectDecisionProtocolDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion 
                                    ? x.RealityObject.Municipality.Id
                                    : x.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Id,
                            RoId = x.RealityObject.Id,
                            x.RealityObject.ExternalId,
                            Municipality = moLevel == MoLevel.MunicipalUnion 
                                ? x.RealityObject.Municipality.Name
                                : x.RealityObject.MoSettlement.Name,
                            Settlement = moLevel == MoLevel.MunicipalUnion
                                ? x.RealityObject.MoSettlement.Name
                                : string.Empty,
                            x.RealityObject.FiasAddress.PlaceName,
                            x.RealityObject.FiasAddress.StreetName,
                            x.RealityObject.FiasAddress.House,
                            x.RealityObject.FiasAddress.Housing,
                            x.RealityObject.FiasAddress.Letter,
                            x.DocumentNum,
                            x.ProtocolDate,
                            x.AuthorizedPerson,
                            x.PhoneAuthorizedPerson,
                            x.State
                        })
                        .OrderByDescending(x => x.Municipality)
                        .ThenBy(x => x.Settlement)
                        .ThenBy(x => x.PlaceName)
                        .ThenBy(x => x.StreetName)
                        .ThenBy(x => x.House)
                        .ToList();

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
                var num = 0;

                foreach (var protocol in ownerDecProtocols)
                {
                    section.ДобавитьСтроку();
                    section["Num"] = ++num;
                    section["ExternalId"] = protocol.ExternalId;
                    section["Municipality"] = protocol.Municipality;
                    section["Settlement"] = protocol.Settlement;
                    section["PlaceName"] = protocol.PlaceName;
                    section["StreetName"] = protocol.StreetName;
                    section["House"] = protocol.House;
                    section["Housing"] = protocol.Housing;
                    section["Letter"] = protocol.Letter;
                    section["ProtocolType"] = "Протокол решений собственников";
                    section["DocumentNum"] = protocol.DocumentNum;
                    section["ManOrg"] = manOrgRoDict.ContainsKey(protocol.RoId)
                        ? manOrgRoDict[protocol.RoId]
                        : string.Empty;
                    section["ProtocolDate"] = protocol.ProtocolDate;
                    section["AuthorizedPerson"] = protocol.AuthorizedPerson;
                    section["PhoneAuthorizedPerson"] = protocol.PhoneAuthorizedPerson;
                    section["CrFundDecision"] = crFundFormDecDict.ContainsKey(protocol.Id)
                        ? crFundFormDecDict[protocol.Id]
                        : string.Empty;
                    section["State"] = protocol.State != null ? protocol.State.Name : string.Empty;
                    section["AccOwnerDecision"] = accOwnDecDict.ContainsKey(protocol.Id)
                        ? accOwnDecDict[protocol.Id]
                        : string.Empty;
                    if (monthlyFeeDict.ContainsKey(protocol.Id) && monthlyFeeDict[protocol.Id] != null)
                    {
                        section["MonthlyValue"] = monthlyFeeDict[protocol.Id].Value;
                        var from = monthlyFeeDict[protocol.Id].From;
                        var to = monthlyFeeDict[protocol.Id].To;
                        section["MonthlyFrom"] = from.HasValue
                            ? from.Value.ToShortDateString()
                            : string.Empty;
                        section["MonthlyTo"] = to.HasValue
                            ? to.Value.ToShortDateString()
                            : string.Empty;
                    }
                    section["CreditOrg"] = credOrgDecDict.ContainsKey(protocol.Id)
                        ? credOrgDecDict[protocol.Id]
                        : string.Empty;
                    section["MinFundDecision"] = "40";
                    section["AccumTransfDecision"] = accumTransfDecDict.ContainsKey(protocol.Id)
                        ? accumTransfDecDict[protocol.Id].ToStr()
                        : string.Empty;

                    if (realtyJobYears.ContainsKey(protocol.Id))
                    {
                        foreach (var realtyJobYear in realtyJobYears[protocol.Id])
                        {
                            var userYear = realtyJobYear.UserYear;
                            if (realtyJobYear.Job != null)
                            {
                                section[string.Format("Year{0}", realtyJobYear.Job.Id)] = userYear.HasValue ? userYear.Value.ToStr() : string.Empty;
                            }
                        }
                    }
                }
            }

            #endregion

            #region ЕСЛИ ВЫБРАН ПАРАМЕТР ПРОТОКОЛ ОРГАНОВ ГОС. ВЛАСТИ

            else if (decisionType == 2)
            {
                // создаем вертикальную секцию для данных протокола решений органов власти
                var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GovDecisSection");
                verticalSection.ДобавитьСтроку();

                var govDecProtocols =
                    GovDecisionDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.RealityObject.Municipality.Id
                                    : x.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Id,
                            RoId = x.RealityObject.Id,
                            x.RealityObject.ExternalId,
                            Municipality = moLevel == MoLevel.MunicipalUnion
                                ? x.RealityObject.Municipality.Name
                                : x.RealityObject.MoSettlement.Name,
                            Settlement = moLevel == MoLevel.MunicipalUnion
                                ? x.RealityObject.MoSettlement.Name
                                : string.Empty,
                            x.RealityObject.FiasAddress.PlaceName,
                            x.RealityObject.FiasAddress.StreetName,
                            x.RealityObject.FiasAddress.House,
                            x.RealityObject.FiasAddress.Housing,
                            x.RealityObject.FiasAddress.Letter,
                            x.ProtocolNumber,
                            x.ProtocolDate,
                            x.AuthorizedPerson,
                            x.AuthorizedPersonPhone,
                            x.State,
                            x.FundFormationByRegop,
                            x.Destroy,
                            x.DestroyDate,
                            x.Reconstruction,
                            x.ReconstructionStart,
                            x.ReconstructionEnd,
                            x.TakeLandForGov,
                            x.TakeLandForGovDate,
                            x.TakeApartsForGov,
                            x.TakeApartsForGovDate,
                            x.MaxFund
                        })
                        .OrderByDescending(x => x.Municipality)
                        .ThenBy(x => x.Settlement)
                        .ThenBy(x => x.PlaceName)
                        .ThenBy(x => x.StreetName)
                        .ThenBy(x => x.House)
                        .ToList();

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
                var num = 0;

                foreach (var protocol in govDecProtocols)
                {
                    section.ДобавитьСтроку();
                    section["Num"] = ++num;
                    section["ExternalId"] = protocol.ExternalId;
                    section["Municipality"] = protocol.Municipality;
                    section["Settlement"] = protocol.Settlement;
                    section["PlaceName"] = protocol.PlaceName;
                    section["StreetName"] = protocol.StreetName;
                    section["House"] = protocol.House;
                    section["Housing"] = protocol.Housing;
                    section["Letter"] = protocol.Letter;
                    section["ProtocolType"] = "Протокол органов гос. власти";
                    section["DocumentNum"] = protocol.ProtocolNumber;
                    section["ManOrg"] = manOrgRoDict != null && manOrgRoDict.ContainsKey(protocol.RoId)
                        ? manOrgRoDict[protocol.RoId]
                        : string.Empty;
                    section["ProtocolDate"] = protocol.ProtocolDate;
                    section["AuthorizedPerson"] = protocol.AuthorizedPerson;
                    section["PhoneAuthorizedPerson"] = protocol.AuthorizedPersonPhone;
                    section["CrFundDecision"] = protocol.FundFormationByRegop
                        ? "Счет регионального оператора"
                        : string.Empty;
                    section["State"] = protocol.State != null ? protocol.State.Name : string.Empty;
                    section["Destroy"] = protocol.Destroy ? "Да" : "Нет";
                    section["DestroyDate"] = protocol.DestroyDate.HasValue
                        ? protocol.DestroyDate.Value.ToShortDateString()
                        : string.Empty;
                    section["Reconstruction"] = protocol.Reconstruction ? "Да" : "Нет";
                    section["ReconstructionStart"] = protocol.DestroyDate.HasValue
                        ? protocol.DestroyDate.Value.ToShortDateString()
                        : string.Empty;
                    section["ReconstructionEnd"] = protocol.DestroyDate.HasValue
                        ? protocol.DestroyDate.Value.ToShortDateString()
                        : string.Empty;
                    section["TakeLandForGov"] = protocol.Destroy ? "Да" : "Нет";
                    section["TakeLandForGovDate"] = protocol.DestroyDate.HasValue
                        ? protocol.DestroyDate.Value.ToShortDateString()
                        : string.Empty;
                    section["TakeApartsForGov"] = protocol.Destroy ? "Да" : "Нет";
                    section["TakeApartsForGovDate"] = protocol.DestroyDate.HasValue
                        ? protocol.DestroyDate.Value.ToShortDateString()
                        : string.Empty;

                    section["MaxFund"] = protocol.MaxFund;
                }
            }

            #endregion

            #region ЕСЛИ ВЫБРАН ПАРАМЕТР НЕТ ПРОТОКОЛА

            else
            {
                var govDecisProtRoIds =
                GovDecisionDomain.GetAll()
                    .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.RealityObject.Municipality.Id
                                    : x.RealityObject.MoSettlement.Id))
                    .WhereIf(roIds.Any(), x => roIds.Contains(x.RealityObject.Id))
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                var ownDecisProtRoIds =
                    RealityObjectDecisionProtocolDomain.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.RealityObject.Municipality.Id
                                    : x.RealityObject.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.RealityObject.Id))
                        .Select(x => x.RealityObject.Id)
                        .ToArray();

                var realObjects =
                    RealityObjectRepo.GetAll()
                        .WhereIf(muIds.Any(), x => muIds.Contains(
                                moLevel == MoLevel.MunicipalUnion
                                    ? x.Municipality.Id
                                    : x.MoSettlement.Id))
                        .WhereIf(roIds.Any(), x => roIds.Contains(x.Id))
                        .Where(x => !govDecisProtRoIds.Contains(x.Id))
                        .Where(x => !ownDecisProtRoIds.Contains(x.Id))
                        .Select(x => new
                        {
                            RoId = x.Id,
                            x.ExternalId,
                            Municipality = moLevel == MoLevel.MunicipalUnion
                                ? x.Municipality.Name
                                : x.MoSettlement.Name,
                            Settlement = moLevel == MoLevel.MunicipalUnion
                                ? x.MoSettlement.Name
                                : string.Empty,
                            x.FiasAddress.PlaceName,
                            x.FiasAddress.StreetName,
                            x.FiasAddress.House,
                            x.FiasAddress.Housing,
                            x.FiasAddress.Letter
                        })
                        .OrderByDescending(x => x.Municipality)
                        .ThenBy(x => x.Settlement)
                        .ThenBy(x => x.PlaceName)
                        .ThenBy(x => x.StreetName)
                        .ThenBy(x => x.House)
                        .ToList();

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
                var num = 0;

                foreach (var realObj in realObjects)
                {
                    section.ДобавитьСтроку();
                    section["Num"] = ++num;
                    section["ExternalId"] = realObj.ExternalId;
                    section["Municipality"] = realObj.Municipality;
                    section["Settlement"] = realObj.Settlement;
                    section["PlaceName"] = realObj.PlaceName;
                    section["StreetName"] = realObj.StreetName;
                    section["House"] = realObj.House;
                    section["Housing"] = realObj.Housing;
                    section["Letter"] = realObj.Letter;
                    section["ProtocolType"] = "Нет протокола";
                }
            }

            #endregion
        }
    }
}