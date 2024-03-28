namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Map
{
    using Entity;
    using B4.Modules.Mapping.Mappers;
    
    public class ViewIndividualClaimWorkMap : PersistentObjectMap<ViewIndividualClaimWork>
    {
        public ViewIndividualClaimWorkMap() :
                base("Bars.Gkh.RegOperator.Modules.ClaimWork.Entity.ViewIndividualClaimWork", "VIEW_CLW_INDIVIDUAL_CLAIM_WORK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            this.Property(x => x.DebtorState, "Статус").Column("DEBTOR_STATE").NotNull();
            this.Property(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY");
            this.Property(x => x.AccountOwnerName, "Наименование абонента").Column("ACCOUNT_OWNER_NAME");
            this.Property(x => x.RegistrationAddress, "Адрес прописки").Column("REGISTRATION_ADDRESS");
            this.Property(x => x.AccountsAddress, "Адреса ЛС").Column("ACCOUNTS_ADDRESS");
            this.Property(x => x.AccountsNumber, "Количество ЛС").Column("ACCOUNTS_NUMBER");
            this.Property(x => x.CurrChargeBaseTariffDebt, "Сумма текущей задолженности по базовому тарифу").Column("CUR_CHARGE_BASE_TARIFF_DEBT");
            this.Property(x => x.CurrChargeDecisionTariffDebt, "Сумма текущей задолженности по тарифу решения").Column("CUR_CHARGE_DECISION_TARIFF_DEBT");
            this.Property(x => x.CurrChargeDebt, "Сумма текущей задолженности").Column("CUR_CHARGE_DEBT");
            this.Property(x => x.CurrPenaltyDebt, "Сумма текущей задолженности по пени").Column("CUR_PENALTY_DEBT");
            this.Property(x => x.IsDebtPaid, "Задолженность погашена").Column("IS_DEBT_PAID");
            this.Property(x => x.DebtPaidDate, "Дата погашения задолженности").Column("DEBT_PAID_DATE");
            this.Property(x => x.PIRCreateDate, "Дата создания ПИР").Column("pir_create_date");
            this.Property(x => x.FirstDocCreateDate, "Дата создания первого документа").Column("first_document_date");
            this.Property(x => x.HasCharges185FZ, "Начисления по 185ФЗ").Column("HAS_CHARGES_185FZ");
            this.Property(x => x.MinShare, "Мин. доля").Column("MIN_SHARE");
            this.Property(x => x.Underage, "Имеется несовершеннолетний").Column("Underage");
            this.Reference(x => x.User, "Пользователь").Column("USER_ID");
            this.Property(x => x.ObjectCreateDate, "Дата создания").Column("OBJECT_CREATE_DATE");
        }
    }
}
