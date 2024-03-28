namespace Bars.Gkh.RegOperator.Modules.Reforma
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Decisions.Nso.Entities.Proxies;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Modules.Reforma;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Castle.Windsor;

    public class RealityObjectBothProtocolProvider : IRealityObjectBothProtocolProvider
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetData(long roId)
        {
            var decisionProtocolService = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var govDecisionService = Container.ResolveDomain<GovDecision>();
            try
            {
                var protocol =
                    decisionProtocolService.GetAll()
                                           .Where(x => x.RealityObject.Id == roId)
                                           .Select(x => new { x.Id, x.ProtocolDate, ProtocolNumber = x.DocumentNum, Type = 10 })
                                           .AsEnumerable()
                                           .Union(govDecisionService.GetAll().Where(x => x.RealityObject.Id == roId).Select(x => new { x.Id, x.ProtocolDate, x.ProtocolNumber, Type = 20 }))
                                           .OrderByDescending(x => x.ProtocolDate)
                                           .FirstOrDefault();

                if (protocol == null)
                {
                    return new BaseDataResult(false, string.Empty);
                }

                var baseParams = new BaseParams();

                if (protocol.Type == 10)
                {
                    baseParams.Params["id"] = protocol.Id;
                    var robjectDecisionService = Container.Resolve<IRobjectDecisionService>();
                    try
                    {
                        var decision = robjectDecisionService.Get(baseParams).Data as UltimateDecisionProxy;
                        if (decision != null)
                        {
                            return new BaseDataResult(this.GetDataByUltimateDecision(roId, decision));
                        }
                    }
                    catch
                    {
                        return new BaseDataResult(false, string.Empty);
                    }
                    finally
                    {
                        Container.Release(robjectDecisionService);
                    }
                }

                var govDecisionViewModel = Container.Resolve<IViewModel<GovDecision>>();
                baseParams.Params["id"] = protocol.Id;
                try
                {
                    var decision = govDecisionViewModel.Get(govDecisionService, baseParams).Data;
                    if (decision != null)
                    {
                        return new BaseDataResult(this.GetDataByGovDecision(roId, decision));
                    }
                }
                catch
                {
                    return new BaseDataResult(false, string.Empty);
                }
                finally
                {
                    Container.Release(govDecisionViewModel);
                }

                return new BaseDataResult(false, string.Empty);
            }
            finally
            {
                this.Container.Release(decisionProtocolService);
                this.Container.Release(govDecisionService);
            }
        }

        private RealityObjectBothProtocolData GetDataByGovDecision(long roId, dynamic decision)
        {
            var isRegop = (bool)decision.FundFormationByRegop;
            var paymentDecision = (PaymentAndFundDecisions)decision.PaymentAndFundDecisions;

            var result = new RealityObjectBothProtocolData();

            string providerInn;
            string providerName;
            if (isRegop)
            {
                if (this.GetRegOpInformation(out providerInn, out providerName))
                {
                    result.ProviderInn = providerInn;
                    result.ProviderName = providerName;
                }
            }
            else
            {
                if (this.GetManOrgInformation(roId, out providerInn, out providerName))
                {
                    result.ProviderInn = providerInn;
                    result.ProviderName = providerName;
                }
            }

            result.CommonMeetingProtocolDate = (DateTime)decision.ProtocolDate;
            result.CommonMeetingProtocolNumber = (string)decision.ProtocolNumber;

            result.PaymentAmount = paymentDecision.Return(x => x.MinFundPaymentSize);

            return result;
        }

        private RealityObjectBothProtocolData GetDataByUltimateDecision(long roId, UltimateDecisionProxy decision)
        {
            var isRegop = decision.CrFundFormationDecision.Return(x => x.Decision) == CrFundFormationDecisionType.RegOpAccount
                          || (decision.CrFundFormationDecision.Return(x => x.Decision) == CrFundFormationDecisionType.SpecialAccount
                              && decision.AccountOwnerDecision.Return(x => x.DecisionType) == AccountOwnerDecisionType.RegOp);

            var result = new RealityObjectBothProtocolData();

            string providerInn;
            string providerName;
            if (isRegop)
            {
                if (this.GetRegOpInformation(out providerInn, out providerName))
                {
                    result.ProviderInn = providerInn;
                    result.ProviderName = providerName;
                }
            }
            else
            {
                if (this.GetManOrgInformation(roId, out providerInn, out providerName))
                {
                    result.ProviderInn = providerInn;
                    result.ProviderName = providerName;
                }
            }

            result.CommonMeetingProtocolDate = decision.Protocol.ProtocolDate;
            result.CommonMeetingProtocolNumber = decision.Protocol.DocumentNum;
            result.PaymentAmount = decision.MinFundAmountDecision.Return(x => x.Decision).Return(x => x == 0 ? (decimal?)null : x);

            return result;
        }

        private bool GetManOrgInformation(long roId, out string inn, out string name)
        {
            var service = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            try
            {
                var manOrg =
                    service.GetAll()
                           .Where(x => x.RealityObject.Id == roId)
                           .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                           .Select(
                               x =>
                               new
                                   {
                                       x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                                       x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                       DateStart = x.ManOrgContract.StartDate,
                                       DateEnd = x.ManOrgContract.EndDate
                                   })
                           .Where(x => x.DateStart <= DateTime.Now)
                           .FirstOrDefault(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Now);
                if (manOrg == null)
                {
                    inn = null;
                    name = null;
                    return false;
                }

                inn = manOrg.Inn;
                name = manOrg.Name;
                return true;
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private bool GetRegOpInformation(out string inn, out string name)
        {
            var service = this.Container.ResolveDomain<RegOperator>();
            try
            {
                var regOp = service.GetAll().FirstOrDefault();
                if (regOp != null)
                {
                    inn = regOp.Contragent.Inn;
                    name = regOp.Contragent.Name;
                    return true;
                }

                inn = null;
                name = null;
                return false;
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}