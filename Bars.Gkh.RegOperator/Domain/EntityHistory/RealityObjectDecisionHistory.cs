namespace Bars.Gkh.RegOperator.Domain.EntityHistory
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.EntityHistory;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Сервис логирования протоколов решений собственников
    /// </summary>
    public class RealityObjectDecisionHistory : BaseEntityHistoryService<RealityObjectDecisionProtocol>
    {
        public IDomainService<CrFundFormationDecision> CrFundFormationDecisionDomain { get; set; }

        public IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain { get; set; }

        /// <inheritdoc />
        protected override EntityHistoryType GroupType { get; } = EntityHistoryType.DecisionProtocol;

        /// <inheritdoc />
        protected override void Init()
        {
            this.Map(x => x.DocumentNum, "Номер протокола");
            this.Map(x => x.ProtocolDate.ToShortDateString(), "Дата протокола");
            this.Map(x => x.DateStart.ToShortDateString(), "Дата вступления в силу");
            this.Map(this.GetCrFundFormationType, "Способ формирования фонда");
            this.Map(this.GetAccountOwner, "Владелец специального счета");
            this.Map(x => CoreDecisionType.Owners.GetDisplayName(), "Тип протокола");
        }

        private string GetCrFundFormationType(RealityObjectDecisionProtocol protocol)
        {
            var decision = this.CrFundFormationDecisionDomain.GetAll()
                .Where(x => x.Protocol.Id == protocol.Id)
                .Select(x => (CrFundFormationDecisionType?) x.Decision)
                .FirstOrDefault();

            return decision.HasValue ? decision.Value.GetDisplayName() : CrFundFormationDecisionType.RegOpAccount.GetDisplayName();
        }

        private string GetAccountOwner(RealityObjectDecisionProtocol protocol)
        {
            var owner = this.AccountOwnerDecisionDomain.GetAll()
                .Where(x => x.Protocol.Id == protocol.Id)
                .Select(x => (AccountOwnerDecisionType?) x.DecisionType)
                .FirstOrDefault();

            return owner.HasValue ? owner.Value.GetDisplayName() : AccountOwnerDecisionType.RegOp.GetDisplayName();
        }
    }
}