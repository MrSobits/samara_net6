namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Services.DataContracts;

    public partial class Service
    {
        public GetRealObjDecisionProtocolResponse GetRealObjDecisionProtocol(string houseId)
        {
            var roId = houseId.ToInt();

            var realObjDecProtocolDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var decisionDomain = Container.ResolveDomain<UltimateDecision>();
            var govDomain = Container.ResolveDomain<GovDecision>();

            try
            {
                var protocolQuery = realObjDecProtocolDomain.GetAll()
                    .Where(x => x.State.FinalState)
                    .Where(x => x.RealityObject.Id == roId);

                var decisions = decisionDomain.GetAll()
                    .Where(x => protocolQuery.Any(y => y.Id == x.Protocol.Id))
                    .Select(x => new
                    {
                        x.Protocol.Id,
                        Type = x.GetType(),
                        Decision = x
                    })
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key);

                var protocols = protocolQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.ProtocolDate,
                        DateStart = x.DateStart > DateTime.MinValue ? x.DateStart : x.ProtocolDate,
                        x.DocumentNum,
                        FileName = x.File != null ? x.File.Name : string.Empty,
                        FileId = x.File != null ? x.File.Id : 0L
                    })
                    .ToList()
                    .Select(x =>
                    {
                        var decs = decisions.Get(x.Id);

                        var result = new RealObjDecisionProtocolProxy
                        {
                            ProtocolType = CoreDecisionType.Owners.GetEnumMeta().Display,
                            ProtocolDate = x.ProtocolDate.ToShortDateString(),
                            DateStart = x.DateStart.ToShortDateString(),
                            ProtocolNum = x.DocumentNum,
                            FileName = x.FileName,
                            FileId = x.FileId
                        };

                        if (decs != null)
                        {
                            var decision = decs.Where(y => y.Type == typeof (CrFundFormationDecision))
                                .Select(y => y.Decision as CrFundFormationDecision)
                                .FirstOrDefault();
                            if (decision != null)
                            {
                                result.FundFormationType = decision.Decision.GetEnumMeta().Display;
                                if (decision.Decision == CrFundFormationDecisionType.RegOpAccount)
                                {
                                    result.SpecialAccOwner = string.Empty;
                                }
                                else
                                {
                                    result.SpecialAccOwner =
                                decs.Where(y => y.Type == typeof(AccountOwnerDecision))
                                    .Select(y =>
                                    {
                                        var res = y.Decision as AccountOwnerDecision;
                                        return res.DecisionType.GetEnumMeta().Display;
                                    })
                                    .FirstOrDefault() ?? "Не задано";
                                }
                            }
                            else
                            {
                                result.FundFormationType = "Не задано";
                                result.SpecialAccOwner =
                                decs.Where(y => y.Type == typeof(AccountOwnerDecision))
                                    .Select(y =>
                                    {
                                        var res = y.Decision as AccountOwnerDecision;
                                        return res.DecisionType.GetEnumMeta().Display;
                                    })
                                    .FirstOrDefault() ?? "Не задано";
                            }

                            result.MinFundAmount =
                                    decs.Where(y => y.Type == typeof(MonthlyFeeAmountDecision))
                                        .Select(y =>
                                        {
                                            var dec = y.Decision as MonthlyFeeAmountDecision;
                                            var res = dec.Decision.Return(z => z
                                                .Where(k => k.From <= DateTime.Now)
                                                .OrderByDescending(k => k.From)
                                                .FirstOrDefault(k => !k.To.HasValue || k.To >= DateTime.Now));

                                            return res != null ? string.Format("{0:0.00}", res.Value) : "Не задано";
                                        })
                                        .FirstOrDefault() ?? "Не задано";

                            result.CreditOrg =
                                decs.Where(y => y.Type == typeof(CreditOrgDecision))
                                    .Select(y =>
                                    {
                                        var res = y.Decision as CreditOrgDecision;
                                        return res.Decision.Name;
                                    })
                                    .FirstOrDefault() ?? "Не задано";
                        }

                        return result;
                    }).ToList();

                var govDecisions = govDomain.GetAll()
                    .Where(x => x.State.FinalState)
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => new
                    {
                        x.Id,
                        x.ProtocolDate,
                        DateStart = x.DateStart > DateTime.MinValue ? x.DateStart : x.ProtocolDate,
                        x.ProtocolNumber,
                        x.FundFormationByRegop,
                        FileName = x.ProtocolFile != null ? x.ProtocolFile.Name : string.Empty,
                        FileId = x.ProtocolFile != null ? x.ProtocolFile.Id : 0L
                    })
                    .ToList()
                    .Select(x => new RealObjDecisionProtocolProxy
                    {
                        ProtocolType = CoreDecisionType.Government.GetEnumMeta().Display,
                        ProtocolDate = x.ProtocolDate.ToShortDateString(),
                        DateStart = x.DateStart.ToShortDateString(),
                        ProtocolNum = x.ProtocolNumber,
                        FileName = x.FileName,
                        FileId = x.FileId,
                        FundFormationType = x.FundFormationByRegop ? "Счет регионального оператора" : "Специальный счет",
                        SpecialAccOwner = x.FundFormationByRegop ? string.Empty : "Не задано",
                        MinFundAmount = "Не задано",
                        CreditOrg = "Не задано"
                    });

                protocols.AddRange(govDecisions);

                return new GetRealObjDecisionProtocolResponse
                {
                    RealObjDecisionProtocols = protocols.OrderBy(x => x.ProtocolNum).ToArray()
                };

            }
            finally
            {
                Container.Release(realObjDecProtocolDomain);
                Container.Release(decisionDomain);
                Container.Release(govDomain);
            }
        }
    }
}