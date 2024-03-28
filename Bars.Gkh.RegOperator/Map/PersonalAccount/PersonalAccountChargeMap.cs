namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Начисления по Л/С"</summary>
    public class PersonalAccountChargeMap : BaseImportableEntityMap<PersonalAccountCharge>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountChargeMap() : 
                base("Начисления по Л/С", "REGOP_PERS_ACC_CHARGE")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.BasePersonalAccount, "Лицевой счет").Column("PERS_ACC_ID").NotNull().Fetch();
            this.Reference(x => x.ChargePeriod, "Период начисления").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.Packet, "Пакет подтвержденных начилсений для группировки").Column("PACKET_ID");

            this.Property(x => x.ChargeDate, "Дата начисления").Column("CHARGE_DATE").NotNull();
            this.Property(x => x.Guid, "GUID начисления (для связи с неподтвержденными)").Column("GUID").Length(40).NotNull();
            this.Property(x => x.Charge, "Сумма начисления. Складывается из суммы по тарифу, суммы по пени, суммы по перера" +
                    "счету").Column("CHARGE").NotNull();
            this.Property(x => x.ChargeTariff, "Сумма начисления по тарифу").Column("CHARGE_TARIFF").NotNull();
            this.Property(x => x.Penalty, "Сумма начисления по пени").Column("PENALTY").NotNull();
            this.Property(x => x.RecalcByBaseTariff, "Сумма перерасчета").Column("RECALC").NotNull();
            this.Property(x => x.RecalcByDecisionTariff, "Перерасчет по тарифу решения").Column("RECALC_DECISION").NotNull();
            this.Property(x => x.RecalcPenalty, "Перерасчет пени").Column("RECALC_PENALTY").NotNull();
            this.Property(x => x.OverPlus, "Избыток как разница между Решением собственников и базовым тарифом").Column("OVERPLUS").NotNull();
            this.Property(x => x.IsFixed, "Зафиксирована").Column("IS_FIXED").NotNull();
            this.Property(x => x.IsActive, "Начисление активно").Column("IS_ACTIVE").DefaultValue(true).NotNull();
        }
    }
}
