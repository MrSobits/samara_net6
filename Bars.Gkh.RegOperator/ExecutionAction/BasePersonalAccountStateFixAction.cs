namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class BasePersonalAccountStateFixAction : BaseExecutionAction
    {
        private readonly IDomainService<BasePersonalAccount> basePersonalAccDomainService;
        private readonly IDomainService<State> stateDomainService;
        private readonly IDomainService<GovDecision> govDecDomainService;
        private readonly IDomainService<RealityObjectDecisionProtocol> roDecProtocolDomainService;

        private Dictionary<long, CrFundFormationDecision> crDecisions;
        private Dictionary<long, AccountOwnerDecision> accOwnersDecisions;

        private State openPersonalAccountState;
        private State nonActivePersonalAccountState;
        private State defaultPersonalAccountState;

        private readonly IRealityObjectDecisionsService decisionService;

        private readonly ISessionProvider sessions;

        public BasePersonalAccountStateFixAction(
            IDomainService<BasePersonalAccount> basePersonalAccDomainService,
            IDomainService<State> stateDomainService,
            IRealityObjectDecisionsService decisionService,
            IDomainService<GovDecision> govDecDomainService,
            IDomainService<RealityObjectDecisionProtocol> roDecProtocolDomainService,
            ISessionProvider sessions)
        {
            this.decisionService = decisionService;
            this.sessions = sessions;
            this.basePersonalAccDomainService = basePersonalAccDomainService;
            this.stateDomainService = stateDomainService;
            this.govDecDomainService = govDecDomainService;
            this.roDecProtocolDomainService = roDecProtocolDomainService;
        }

        public override string Description => @"!!!Запускать только тогда, когда созданы соответствующие статусы!!!
                            У всех уже существующих ЛС со статусом с кодом 1 (Открыт) изменит статус на Не активен (код 4),
                            если в доме нет действующего протокола или решение о способе формирования фонда КР = Специальный счет
                            и Владелец специального счета != Региональный оператор.";

        public override string Name => "Изменение статуса Лицевого счета на Не активен";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var lst = this.basePersonalAccDomainService.GetAll().ToList();
            var roByAcc = this.basePersonalAccDomainService.GetAll()
                .Select(x => new {x.Id, RoId = x.Room.RealityObject.Id})
                .ToList()
                .ToDictionary(x => x.Id, x => new RealityObject {Id = x.RoId});

            var realityObjects = roByAcc.Values.Select(x => x);

            var ownerProtocolMaxDate = this.roDecProtocolDomainService.GetAll()
                .Select(
                    x => new
                    {
                        x.RealityObject.Id,
                        x.ProtocolDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.SafeMax(x => x.ProtocolDate));

            var govDecProtocolMaxDate = this.govDecDomainService.GetAll()
                .Select(
                    x => new
                    {
                        x.RealityObject.Id,
                        x.ProtocolDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.SafeMax(x => x.ProtocolDate));

            var govDecRoIds = govDecProtocolMaxDate.Where(x => ownerProtocolMaxDate.Get(x.Key) < x.Value).Select(x => x.Key).ToHashSet();

            this.crDecisions =
                this.decisionService.GetActualDecisionForCollection<CrFundFormationDecision>(realityObjects, true)
                    .ToDictionary(x => x.Key.Id, y => y.Value);
            this.accOwnersDecisions =
                this.decisionService.GetActualDecisionForCollection<AccountOwnerDecision>(realityObjects, true).ToDictionary(x => x.Key.Id, y => y.Value);

            var session = this.sessions.GetCurrentSession();
            var sb = new StringBuilder();

            foreach (var basePersonalAccount in lst)
            {
                var ro = roByAcc[basePersonalAccount.Id];
                var state = govDecRoIds.Contains(ro.Id) ? this.GetOpenPersonalAccountState() : this.GetPersonalAccountState(ro);

                sb.AppendFormat(
                    "update regop_pers_acc set state_id = {0} where Id = {1};",
                    state.Id,
                    basePersonalAccount.Id);
            }

            session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();

            return new BaseDataResult
            {
                Success = true
            };
        }

        private State GetOpenPersonalAccountState()
        {
            return this.openPersonalAccountState
                ?? (this.openPersonalAccountState =
                    this.stateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "1"));
        }

        private State GetNonActivePersonalAccountState()
        {
            return this.nonActivePersonalAccountState
                ?? (this.nonActivePersonalAccountState =
                    this.stateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "4"));
        }

        private State GetPersonalAccountState(RealityObject realityObject)
        {
            var state = this.GetDefaultPersonalAccountState();
            var nonActivePersonalAccountState = this.GetNonActivePersonalAccountState();
            var openPersonalAccountState = this.GetOpenPersonalAccountState();

            var crFundDecision = this.crDecisions.Get(realityObject.Id);
            if (crFundDecision == null)
            {
                if (nonActivePersonalAccountState != null)
                {
                    state = nonActivePersonalAccountState;
                }
            }
            else
            {
                switch (crFundDecision.Decision)
                {
                    case CrFundFormationDecisionType.Unknown:
                        if (nonActivePersonalAccountState != null)
                        {
                            state = nonActivePersonalAccountState;
                        }

                        break;
                    case CrFundFormationDecisionType.RegOpAccount:
                        if (openPersonalAccountState != null)
                        {
                            state = openPersonalAccountState;
                        }

                        break;
                    case CrFundFormationDecisionType.SpecialAccount:
                        var accOwnerDecision = this.accOwnersDecisions.Get(realityObject.Id);
                        if (accOwnerDecision == null)
                        {
                            if (nonActivePersonalAccountState != null)
                            {
                                state = nonActivePersonalAccountState;
                            }
                        }
                        else
                        {
                            switch (accOwnerDecision.DecisionType)
                            {
                                case AccountOwnerDecisionType.Custom:
                                    if (nonActivePersonalAccountState != null)
                                    {
                                        state = nonActivePersonalAccountState;
                                    }

                                    break;
                                case AccountOwnerDecisionType.RegOp:
                                    if (openPersonalAccountState != null)
                                    {
                                        state = openPersonalAccountState;
                                    }

                                    break;
                            }
                        }

                        break;
                }
            }

            return state;
        }

        private State GetDefaultPersonalAccountState()
        {
            return this.defaultPersonalAccountState
                ?? (this.defaultPersonalAccountState =
                    this.stateDomainService.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.StartState));
        }
    }
}