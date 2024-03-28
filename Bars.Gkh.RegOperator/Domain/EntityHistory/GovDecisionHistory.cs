namespace Bars.Gkh.RegOperator.Domain.EntityHistory
{
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain.EntityHistory;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Сервис логирования протоколов решений ОГВ
    /// </summary>
    public class GovDecisionHistory : BaseEntityHistoryService<GovDecision>
    {
        /// <inheritdoc />
        protected override EntityHistoryType GroupType { get; } = EntityHistoryType.DecisionProtocol;

        /// <inheritdoc />
        protected override void Init()
        {
            this.Map(x => x.ProtocolNumber, "Номер протокола");
            this.Map(x => x.ProtocolDate.ToShortDateString(), "Дата протокола");
            this.Map(x => x.DateStart.ToShortDateString(), "Дата вступления в силу");
            this.Map(x => x.FundFormationByRegop
                    ? CrFundFormationType.RegOpAccount.GetDisplayName()
                    : CrFundFormationType.NotSelected.GetDisplayName(),
                "Способ формирования фонда");
            this.Map(x => x.FundFormationByRegop
                    ? "Региональный оператор"
                    : "Не выбран",
                "Владелец специального счета");
            this.Map(x => CoreDecisionType.Government.GetDisplayName(), "Тип протокола");
        }
    }
}