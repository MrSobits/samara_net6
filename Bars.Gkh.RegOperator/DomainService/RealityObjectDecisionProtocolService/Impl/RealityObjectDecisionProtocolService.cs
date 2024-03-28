namespace Bars.Gkh.RegOperator.DomainService.RealityObjectDecisionProtocolService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class RealityObjectDecisionProtocolService : IRealityObjectDecisionProtocolService
    {
        private readonly IDomainService<RealityObjectDecisionProtocol> _domainService;
        private readonly IDomainService<CalcAccountRealityObject> _accountroDomain;
        private readonly IDomainService<AccountOwnerDecision> _ownerDecisionDomain;
        private readonly IDomainService<CrFundFormationDecision> _formationDecisionDomain;
        private readonly IDomainService<SpecialCalcAccount> _specaccDomain;
        private readonly IDomainService<GovDecision> _govDecisionDomain;
        private readonly ISpecialCalcAccountService _accountService;

        public RealityObjectDecisionProtocolService(
            IDomainService<RealityObjectDecisionProtocol> domainService,
            IDomainService<CalcAccountRealityObject> accountroDomain,
            IDomainService<AccountOwnerDecision> ownerDecisionDomain,
            IDomainService<CrFundFormationDecision> formationDecisionDomain,
            IDomainService<SpecialCalcAccount> specaccDomain,
            IDomainService<GovDecision> govDecisionDomain,
            ISpecialCalcAccountService accountService)
        {
            _domainService = domainService;
            _accountroDomain = accountroDomain;
            _ownerDecisionDomain = ownerDecisionDomain;
            _formationDecisionDomain = formationDecisionDomain;
            _specaccDomain = specaccDomain;
            _govDecisionDomain = govDecisionDomain;
            _accountService = accountService;
        }

        public RealityObjectDecisionProtocol GetActiveProtocol(RealityObject realityObject)
        {
            return GetActiveProtocolForDate(realityObject, DateTime.Now);
        }

        public RealityObjectDecisionProtocol GetActiveProtocolForDate(RealityObject realityObject, DateTime date)
        {
            return
                _domainService.GetAll()
                    .OrderByDescending(x => x.ProtocolDate)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.RealityObject.Id == realityObject.Id)
                    .FirstOrDefault(x => x.DateStart <= date);
        }

        public void SetNextActualProtocol<T>(T entity) where T : IDecisionProtocol
        {
            var nextActual = _domainService.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Where(x => x.State.FinalState)
                    .WhereIf(entity is RealityObjectDecisionProtocol, x => x.Id != entity.Id)
                    .OrderByDescending(x => x.ProtocolDate)
                    .FirstOrDefault();

            var govDecision = _govDecisionDomain.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id && x.FundFormationByRegop)
                    .Where(x => x.State.FinalState)
                    .WhereIf(entity is GovDecision, x => x.Id != entity.Id)
                    .OrderByDescending(x => x.ProtocolDate)
                    .FirstOrDefault();

            if (nextActual == null && govDecision == null)
            {
                SetRealtyAccountNonActive(entity.RealityObject);
            }

            if (nextActual != null && govDecision != null)
            {
                if (nextActual.DateStart > govDecision.DateStart)
                {
                    govDecision = null;
                }
                else
                {
                    nextActual = null;
                }
            }

            if (nextActual != null)
            {
                var ownerDecision = _ownerDecisionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == nextActual.Id);
                var fundFormationDec =
                    _formationDecisionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == nextActual.Id);

                if (ownerDecision != null && fundFormationDec != null)
                {
                    _accountService.HandleSpecialAccountByProtocolChange(ownerDecision, fundFormationDec, null,
                        entity.RealityObject);
                }
            }

            if (govDecision != null)
            {
                _accountService.HandleSpecialAccountByProtocolChange(null, null, govDecision, entity.RealityObject);
            }
        }

        private void SetRealtyAccountNonActive(RealityObject ro)
        {
            _accountroDomain.GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Select(x => x.Account)
                .Where(x => x.TypeAccount == TypeCalcAccount.Special)
                .ForEach(x =>
                {
                    var acc = (SpecialCalcAccount)x;
                    acc.IsActive = false;
                    _specaccDomain.Update(acc);
                });
        }
    }
}