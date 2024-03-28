namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Основание претензионно исковой работы для неплательщиков"</summary>
    public class DebtorClaimWorkMap : JoinedSubClassMap<DebtorClaimWork>
    {
        public DebtorClaimWorkMap()
            : base("Основание претензионно исковой работы для неплательщиков", "CLW_DEBTOR_CLAIM_WORK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AccountOwner, "Абонент").Column("OWNER_ID").NotNull();
            this.Property(x => x.DebtorType, "Тип должника").Column("DEBTOR_TYPE").NotNull();
            this.Property(x => x.CurrChargeBaseTariffDebt, "Сумма текущей задолженности по базовому тарифу").Column("CUR_CHARGE_BASE_TARIFF_DEBT").NotNull();
            this.Property(x => x.CurrChargeDecisionTariffDebt, "Сумма текущей задолженности по тарифу решения").Column("CUR_CHARGE_DECISION_TARIFF_DEBT").NotNull();
            this.Property(x => x.CurrChargeDebt, "Сумма текущей задолженности").Column("CUR_CHARGE_DEBT").NotNull();
            this.Property(x => x.CurrPenaltyDebt, "Сумма текущей задолженности по пени").Column("CUR_PENALTY_DEBT").NotNull();
            this.Property(x => x.OrigChargeBaseTariffDebt, "Сумма исходной задолженности по базовому тарифу").Column("ORIG_CHARGE_BASE_TARIFF_DEBT").NotNull();
            this.Property(x => x.OrigChargeDecisionTariffDebt, "Сумма исходной задолженности по тарифу решения").Column("ORIG_CHARGE_DECISION_TARIFF_DEBT").NotNull();
            this.Property(x => x.OrigChargeDebt, "Сумма исходной задолженности").Column("ORIG_CHARGE_DEBT").NotNull();
            this.Property(x => x.OrigPenaltyDebt, "Сумма исходной задолженности по пени").Column("ORIG_PENALTY_DEBT").NotNull();
            this.Property(x => x.DebtorState, "Статус ПИР по неплательщику").Column("DEBTOR_STATE").NotNull();
            this.Property(x => x.DebtorStateHistory, "Прошлый статус приостановленного ПИР").Column("DEBTOR_STATE_HISTORY");
            this.Property(x => x.PIRCreateDate, "Дата создания ПИР").Column("PIR_CREATE_DATE");
            this.Property(x => x.SubContractNum, "Номер договора подряда").Column("SUB_CONTRACT_NUM");
            this.Property(x => x.SubContractDate, "Дата договора подряда").Column("SUB_CONTRACT_DATE");

            //this.Property(x => x.RdDocument, "Документ (pеструктуризация долга)").Column("RD_DOCUMENT").Length(500);
            //this.Property(x => x.RdDocNumber, "Номер (pеструктуризация долга)").Column("RD_DOC_NUMBER").Length(100);
            //this.Property(x => x.RdDocDate, "Дата документа (pеструктуризация долга)").Column("RD_DOC_DATE");
            //this.Reference(x => x.RdDocFile, "Файл документа (pеструктуризация долга)").Column("RD_DOC_FILE_ID");
            //this.Reference(x => x.RdPaymentScheduleFile, "Файл графика платежей (pеструктуризация долга)").Column("RD_PAY_SCHDL_FILE_ID");
            //this.Property(x => x.RdDebtSum, "Сумма задолженности (pеструктуризация долга)").Column("RD_DEBT_SUM").NotNull();
            //this.Property(x => x.RdPenaltyDebt, "Задолженность по пени (pеструктуризация долга)").Column("RD_PENALTY_DEBT").NotNull();
            //this.Property(x => x.RdSumPercent, "В т.ч. проценты (руб.) (pеструктуризация долга)").Column("RD_SUM_PERCENT");
            //this.Property(x => x.RestructDebtStatus, "Статус реструктуризации долга").Column("RD_STATUS").NotNull();
            this.Reference(x => x.User, "Пользователь, начавший претензионную работу").Column("USER_ID");
            this.Reference(x => x.SubContragent, "Подрядчик").Column("SUBCONTRAGENT_ID");
            
            //this.Reference(x => x.IdPartial, "ID долевого собственника").Column("id_partial").Fetch();
        }
    }

    public class DebtorClaimWorkNHibernateMapping : JoinedSubclassMapping<DebtorClaimWork>
    {
        public DebtorClaimWorkNHibernateMapping()
        {
            this.Bag(
                x => x.AccountDetails,
                m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.Fetch(CollectionFetchMode.Select);
                    m.Cascade(Cascade.Remove);
                    m.Lazy(CollectionLazy.Extra);
                    m.Key(k => k.Column("CLAIM_WORK_ID"));
                    m.Inverse(true);
                },
                action => action.OneToMany());
        }
    }
}