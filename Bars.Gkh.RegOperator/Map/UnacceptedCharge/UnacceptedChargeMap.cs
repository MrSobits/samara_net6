/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.UnacceptedCharge
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class UnacceptedChargeMap : BaseImportableEntityMap<UnacceptedCharge>
///     {
///         public UnacceptedChargeMap() : base("REGOP_UNACCEPT_CHARGE")
///         {
///             Map(x => x.Charge, "CCHARGE", true);
///             Map(x => x.ChargeTariff, "CCHARGE_TARIFF", true);
///             Map(x => x.Guid, "CGUID", false, 40);
///             Map(x => x.Penalty, "CPENALTY", true);
///             Map(x => x.RecalcByBaseTariff, "CRECALC", true);
///             Map(x => x.RecalcByDecision, "RECALC_DECISION", true);
///             Map(x => x.RecalcPenalty, "RECALC_PENALTY", true);
///             Map(x => x.TariffOverplus, "TARIFF_OVERPLUS", true, 0m);
///             Map(x => x.Accepted, "ACCEPTED", true, 0m);
///             Map(x => x.Description, "DESCR", false, 200);
///             Map(x => x.ContragentAccountNumber, "CONTRAGENT_ACC_NUM", false, 100);
///             Map(x => x.CrFundFormationDecisionType, "FUND_FORMATION_TYPE", true, (object)-1);
/// 
///             References(x => x.Packet, "PACKET_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.PersonalAccount, "ACC_ID", ReferenceMapConfig.Fetch);
///             References(x => x.AccountState, "STATE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Неподтвержденное начисление"</summary>
    public class UnacceptedChargeMap : BaseImportableEntityMap<UnacceptedCharge>
    {
        
        public UnacceptedChargeMap() : 
                base("Неподтвержденное начисление", "REGOP_UNACCEPT_CHARGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Packet, "Ссылка на пакет неподтвержденных начислений").Column("PACKET_ID").NotNull().Fetch();
            Reference(x => x.PersonalAccount, "ЛС").Column("ACC_ID").Fetch();
            Property(x => x.Guid, "GUID начисления").Column("CGUID").Length(40);
            Property(x => x.Charge, "Сумма начисления. Складывается из суммы по базовому тарифу, суммы переплаты, сумм" +
                    "ы по пени, суммы по перерасчету").Column("CCHARGE").NotNull();
            Property(x => x.ChargeTariff, "Сумма начисления по тарифу").Column("CCHARGE_TARIFF").NotNull();
            Property(x => x.TariffOverplus, "Сумма начисления, которая пришла сверх базового тарифа").Column("TARIFF_OVERPLUS").DefaultValue(0m).NotNull();
            Property(x => x.Penalty, "Сумма начисления по пени").Column("CPENALTY").NotNull();
            Property(x => x.RecalcPenalty, "Перерасчет пени").Column("RECALC_PENALTY").NotNull();
            Property(x => x.RecalcByBaseTariff, "Сумма перерасчета по базовому тарифу").Column("CRECALC").NotNull();
            Property(x => x.RecalcByDecision, "Перерасчет по тарифу решения").Column("RECALC_DECISION").NotNull();
            Property(x => x.Accepted, "Подтверждено").Column("ACCEPTED").DefaultValue(false).NotNull();
            Property(x => x.Description, "Примечание").Column("DESCR").Length(200);
            Reference(x => x.AccountState, "Статус ЛС на момент расчета").Column("STATE_ID").Fetch();
            Property(x => x.ContragentAccountNumber, "Номер расчетного счета").Column("CONTRAGENT_ACC_NUM").Length(100);
            Property(x => x.CrFundFormationDecisionType, "Способ формирования фонда КР").Column("FUND_FORMATION_TYPE").DefaultValue(CrFundFormationDecisionType.Unknown).NotNull();
        }
    }
}
