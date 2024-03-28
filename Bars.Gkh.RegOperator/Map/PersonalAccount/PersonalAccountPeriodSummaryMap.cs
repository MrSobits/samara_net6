namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Ситуация по ЛС на период"</summary>
    public class PersonalAccountPeriodSummaryMap : BaseImportableEntityMap<PersonalAccountPeriodSummary>
    {
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountPeriodSummaryMap() : 
                base("Ситуация по ЛС на период", "REGOP_PERS_ACC_PERIOD_SUMM")
        {
        }
        
        /// <summary>
        /// Map
        /// </summary>
        protected override void Map()
        {
            // Reference(x => x.PersonalAccount, "ЛС").Column("ACCOUNT_ID").NotNull();
            this.Reference(x => x.Period, "Период начисления").Column("PERIOD_ID").NotNull().Fetch();
            this.Property(x => x.ChargeTariff, "Начислено по тарифу").Column("CHARGE_TARIFF").NotNull();
            this.Property(x => x.ChargedByBaseTariff, "Начислено по базовому тарифу (Которое принято на МО. Есть еще начисленное сверх б" +
                    "азового тарифа, в случае, если решение собственников больше, чем базовый)").Column("CHARGE_BASE_TARIFF").NotNull();
            this.Property(x => x.RecalcByBaseTariff, "Перерасчет за период").Column("RECALC").NotNull();
            this.Property(x => x.RecalcByDecisionTariff, "Перерасчет за тариф решения").Column("RECALC_DECISION").NotNull();
            this.Property(x => x.RecalcByPenalty, "Перерасчет пени").Column("RECALC_PENALTY").NotNull();
            this.Property(x => x.Penalty, "Пени").Column("PENALTY").NotNull();
            this.Property(x => x.PenaltyPayment, "Оплачено пени").Column("PENALTY_PAYMENT").NotNull();
            this.Property(x => x.TariffPayment, "Оплачено по тарифу в текущем периоде").Column("TARIFF_PAYMENT").NotNull();
            this.Property(x => x.SaldoIn, "Входящее сальдо").Column("SALDO_IN").NotNull();
            this.Property(x => x.SaldoOut, "Исходящее сальдо").Column("SALDO_OUT").NotNull();
            this.Property(x => x.TariffDecisionPayment, "Оплачено по тарифу решения").Column("TARIFF_DESICION_PAYMENT").NotNull();
            this.Property(x => x.OverhaulPayment, "Оплачено по типу услуги \"Капремонт\" (грузится импортом ЛС для раздела \"Перечислен" +
                    "ия средств в фонд\")").Column("OVERHAUL_PAYMENT").NotNull();
            this.Property(x => x.RecruitmentPayment, "Оплачено по типу услуги \"Найм\" (грузится импортом ЛС для раздела \"Перечисления ср" +
                    "едств в фонд\")").Column("RECRUITMENT_PAYMENT").NotNull();
            this.Property(x => x.SaldoInFromServ, "Входящее сальдо").Column("SALDO_IN_SERV");
            this.Property(x => x.SaldoOutFromServ, "Исходящее сальдо").Column("SALDO_OUT_SERV");
            this.Property(x => x.SaldoChangeFromServ, "Изменение сальдо").Column("SALDO_CHANGE_SERV");

            this.Property(x => x.BaseTariffChange, "Сумма операций установки/изменения сальдо за период")
                .Column("BALANCE_CHANGE").DefaultValue(0m).NotNull();
            this.Property(x => x.DecisionTariffChange, "Сумма операций установки/изменения сальдо по тарифу решения за период")
                .Column("DEC_BALANCE_CHANGE").DefaultValue(0m).NotNull();
            this.Property(x => x.PenaltyChange, "Сумма операций установки/изменения сальдо по пени за период")
                .Column("PENALTY_BALANCE_CHANGE").DefaultValue(0m).NotNull();

            this.Property(x => x.BaseTariffDebt, "Задолженность по базовому тарифу на начало периода").Column("BASE_TARIFF_DEBT").DefaultValue(0m).NotNull();
            this.Property(x => x.DecisionTariffDebt, "Задолженность по тарифу решения на начало периода").Column("DEC_TARIFF_DEBT").DefaultValue(0m).NotNull();
            this.Property(x => x.PenaltyDebt, "Задолженность по пени на начало периода").Column("PENALTY_DEBT").DefaultValue(0m).NotNull();
            this.Property(x => x.PerformedWorkChargedBase, "Зачет средств за работы по базовому тарифу").Column("PERF_WORK_CHARGE").DefaultValue(0m).NotNull();
            this.Property(x => x.PerformedWorkChargedDecision, "Зачет средств за работы по тарифу решения").Column("PERF_WORK_CHARGE_DEC").DefaultValue(0m).NotNull();

            // WARNING: при добавлении новых полей не забудь учесть их в контрольных проверках и при закрытии периода, а также во всех методах PeriodSummary
        }
    }

    /// <summary>
    /// Маппинг
    /// </summary>
    public class PersonalAccountPeriodSummaryNHibernateMapping : ClassMapping<PersonalAccountPeriodSummary>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountPeriodSummaryNHibernateMapping()
        {
            this.ManyToOne(
                x => x.PersonalAccount,
                m =>
                    {
                        m.Column("ACCOUNT_ID");
                        m.Cascade(Cascade.DeleteOrphans);
                        m.Lazy(LazyRelation.Proxy);
                        m.Fetch(FetchKind.Select);
                        m.NotNullable(true);
                    });

            this.Bag(
                x => x.BalanceChanges,
                m =>
                    {
                        m.Access(Accessor.NoSetter);
                        m.Fetch(CollectionFetchMode.Select);
                        m.Cascade(Cascade.Persist | Cascade.DeleteOrphans);
                        m.Lazy(CollectionLazy.Extra);
                        m.Key(k => k.Column("SUMMARY_ID"));
                        m.Inverse(true);
                    },
                action => action.OneToMany());

            this.Bag(
                x => x.PenaltyChanges,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Extra);
                        mapper.Key(k => k.Column("SUMMARY_ID"));
                        mapper.Cascade(Cascade.Persist | Cascade.DeleteOrphans);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
