namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;

    public class DecisionHistoryService : IDecisionHistoryService
    {
        protected IWindsorContainer Container { get; private set; }

        public DecisionHistoryService(IWindsorContainer container)
        {
            Container = container;
        }

        public IDataResult GetHistoryTree(BaseParams baseParams)
        {
            var result = new List<Proxy>();

            var roId = baseParams.Params.GetAsId("roId");

            var filterQuery = GetFilter(roId);

            var protocolStateHistory = GetProtocolHistory(filterQuery);

            result.AddRange(GetCreditOrgDecisionHistory(filterQuery, protocolStateHistory));

            result.AddRange(GetMkdManagDecisionHistory(filterQuery, protocolStateHistory));

            result.AddRange(GetAccountOwnerDecisionHistory(filterQuery, protocolStateHistory));

            result.AddRange(GetCrFundFormationDecisionHistory(filterQuery, protocolStateHistory));

            result.AddRange(GetMinFundAmountDecisionHistory(filterQuery, protocolStateHistory));

            result.AddRange(GetMonthlyFeeAmountDecisionHistory(filterQuery, protocolStateHistory));

            var proxy = GetAuthorizedPersonDecisionHistory(roId);
            if (proxy != null)
            {
                result.Add(proxy);
            }

            return new BaseDataResult(
                result.GroupBy(x => x.Type)
                    .Select(y => new
                    {
                        Type = y.Key,
                        leaf = false,
                        expanded = true,
                        children = y.Select(x => new
                        {
                            Type = x.Data,
                            x.DateStart,
                            x.DateEnd,
                            x.Protocol,
                            leaf = true
                        })
                    }));
        }

        private Proxy GetAuthorizedPersonDecisionHistory(long roId)
        {
            var protocol = Container.Resolve<IDomainService<RealityObjectDecisionProtocol>>().GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.State.FinalState)
                .ToList()
                .Where(x => !string.IsNullOrEmpty(x.AuthorizedPerson))
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault();

            if (protocol == null)
            {
                return null;
            }

            return new Proxy
            {
                Type = "Уполномоченное лицо",
                Data = protocol.Return(x => x.AuthorizedPerson),
                DateStart = protocol.Return(x => x.ProtocolDate),
                Protocol = string.Format("№{0} от {1}", protocol.DocumentNum, protocol.ProtocolDate.ToShortDateString())
            };

        }

        public IDataResult GetJobYearsHistory(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("roId");
            var loadParams = baseParams.GetLoadParam();

            var filterQuery = GetFilter(roId);

            var protocolStateHistory = GetProtocolHistory(filterQuery);

            var result = Container.ResolveDomain<JobYearDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    ProtocolOcd = x.Protocol.ObjectCreateDate,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.JobYears
                })
                .AsEnumerable()
                .OrderByDescending(x => protocolStateHistory.Get(x.ProtocolId))
                .ThenByDescending(x => x.ProtocolDate)
                .SelectMany(y => y.Data.Select(x => new
                {
                    JobName = x.Job.Name,
                    x.UserYear,
                    x.PlanYear,
                    Protocol = string.Format("№{0} от {1}", y.DocumentNum, y.ProtocolDate.ToShortDateString())
                }))
                .ToList();

            return new ListDataResult(result.AsQueryable().Filter(loadParams, Container).Order(loadParams).Paging(loadParams).ToList(), result.Count);
        }

        private IQueryable<UltimateDecision> GetFilter(long roId)
        {
            return Container.ResolveDomain<UltimateDecision>().GetAll()
                .Where(x => x.Protocol.RealityObject.Id == roId)
                .Where(x => x.Protocol.State.FinalState);
        }

        private Dictionary<long, DateTime> GetProtocolHistory(IQueryable<UltimateDecision> filterQuery)
        {
            return Container.ResolveDomain<StateHistory>().GetAll()
                .Where(x => x.TypeId == "gkh_real_obj_dec_protocol")
                .Where(x => x.FinalState.FinalState)
                .Where(x => filterQuery.Any(y => y.Protocol.Id == x.EntityId))
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, y => y.Max(x => x.ChangeDate));
        }

        private IEnumerable<Proxy> GetCreditOrgDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            Container.ResolveDomain<CreditOrgDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.Decision.Name
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(x =>
                {
                    var p = new Proxy
                    {
                        Type = "Кредитная организация",
                        Data = x.Data,
                        DateStart = x.StartDate != DateTime.MinValue ? x.StartDate : x.ProtocolDate,
                        Protocol = string.Format("№{0} от {1}", x.DocumentNum, x.ProtocolDate.ToShortDateString())
                    };

                    SetDateEnd(result, p);

                    result.Add(p);
                });

            return result;
        }

        private IEnumerable<Proxy> GetMkdManagDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            Container.ResolveDomain<MkdManagementDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.Decision.Contragent.Name
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(x =>
                {
                    var p = new Proxy
                    {
                        Type = "Управление домом",
                        Data = x.Data ?? "Непосредственное управление",
                        DateStart = x.StartDate != DateTime.MinValue ? x.StartDate : x.ProtocolDate,
                        Protocol = string.Format("№{0} от {1}", x.DocumentNum, x.ProtocolDate.ToShortDateString())
                    };

                    SetDateEnd(result, p);

                    result.Add(p);
                });

            return result;
        }

        private IEnumerable<Proxy> GetAccountOwnerDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            var contracts = Container.ResolveDomain<ManOrgContractRealityObject>().GetAll()
                .Where(y => filterQuery.Any(x => x.Protocol.RealityObject.Id == y.RealityObject.Id))
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                .Select(x => new
                {
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name,
                })
                .OrderByDescending(x => x.StartDate)
                .ToList();

            Container.ResolveDomain<AccountOwnerDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.DecisionType
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(x =>
                {
                    var p = new Proxy
                    {
                        Type = "Владелец счета фонда КР МКД",
                        Data = x.Data == AccountOwnerDecisionType.Custom
                            ? contracts
                                .Where(y => !y.EndDate.HasValue || y.EndDate >= x.ProtocolDate)
                                .FirstOrDefault(y => y.StartDate <= x.ProtocolDate).ReturnSafe(y => y.Name)
                                ?? "Не задано"
                            : x.Data.GetEnumMeta().Display,
                        DateStart = x.StartDate != DateTime.MinValue ? x.StartDate : x.ProtocolDate,
                        Protocol = string.Format("№{0} от {1}", x.DocumentNum, x.ProtocolDate.ToShortDateString())
                    };

                    SetDateEnd(result, p);

                    result.Add(p);
                });

            return result;
        }

        private IEnumerable<Proxy> GetCrFundFormationDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            Container.ResolveDomain<CrFundFormationDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    ProtocolOcd = x.Protocol.ObjectCreateDate,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.Decision
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(x =>
                {
                    var p = new Proxy
                    {
                        Type = "Способ формирования фонда КР МКД",
                        Data = x.Data.GetEnumMeta().Display,
                        DateStart = x.StartDate != DateTime.MinValue ? x.StartDate : x.ProtocolDate,
                        Protocol = string.Format("№{0} от {1}", x.DocumentNum, x.ProtocolDate.ToShortDateString())
                    };

                    SetDateEnd(result, p);

                    result.Add(p);
                });

            return result;
        }

        private IEnumerable<Proxy> GetMinFundAmountDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            Container.ResolveDomain<MinFundAmountDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.Decision
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(x =>
                {
                    var p = new Proxy
                    {
                        Type = "Минимальный размер фонда КР МКД",
                        Data = x.Data.RoundDecimal(2).ToString(CultureInfo.InvariantCulture),
                        DateStart = x.StartDate != DateTime.MinValue ? x.StartDate : x.ProtocolDate,
                        Protocol = string.Format("№{0} от {1}", x.DocumentNum, x.ProtocolDate.ToShortDateString())
                    };

                    SetDateEnd(result, p);

                    result.Add(p);
                });

            return result;
        }

        private IEnumerable<Proxy> GetMonthlyFeeAmountDecisionHistory(IQueryable<UltimateDecision> filterQuery, Dictionary<long, DateTime> protocolStateChangeHistory)
        {
            var result = new List<Proxy>();

            Container.ResolveDomain<MonthlyFeeAmountDecision>().GetAll()
                .Where(y => filterQuery.Any(x => x.Id == y.Id))
                .Select(x => new
                {
                    x.StartDate,
                    ProtocolId = x.Protocol.Id,
                    ProtocolOcd = x.Protocol.ObjectCreateDate,
                    x.Protocol.ProtocolDate,
                    x.Protocol.DocumentNum,
                    Data = x.Decision
                })
                .AsEnumerable()
                .OrderByDescending(x => x.ProtocolDate)
                .ThenByDescending(x => protocolStateChangeHistory.Get(x.ProtocolId))
                .ForEach(p =>
                {
                    var orderedDecisions =
                        p.Data
                            .OrderByDescending(x => x.From)
                            .ThenByDescending(x => x.To);

                    foreach (var periodMonthlyFee in orderedDecisions)
                    {
                        var fee = periodMonthlyFee;

                        //тут хитрое условие
                        //если был утвержден протокол и в нем указан период, который начинается (и может закончиваться) раньше 
                        //периода2, который был утвержден в предыдущих протоколах,
                        //то отсекаем все периоды2, которые находятся позже
                        if (result.Any(x => x.DateStart <= fee.From))
                        {
                            continue;
                        }

                        var pr = new Proxy
                        {
                            Type = "Ставка ежемесячного взноса, р.",
                            Data = fee.Value.ToString(CultureInfo.InvariantCulture),
                            DateStart = fee.From.ToDateTime(),
                            DateEnd = fee.To,
                            Protocol = string.Format("№{0} от {1}", p.DocumentNum, p.ProtocolDate.ToShortDateString())
                        };

                        SetDateEnd(result, pr);

                        result.Add(pr);
                    }
                });

            return result;
        }

        private static void SetDateEnd(IEnumerable<Proxy> proxies, Proxy current)
        {
            var lastAdded = proxies.LastOrDefault();

            if (lastAdded != null)
            {
                if (!current.DateEnd.HasValue || current.DateEnd > lastAdded.DateStart)
                {
                    current.DateEnd = lastAdded.DateStart.AddDays(-1);
                }
            }
        }
    }

    internal class Proxy
    {
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string Protocol { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }
    }
}