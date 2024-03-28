namespace Bars.Gkh.Regions.Tatarstan.Map.UtilityDebtor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Маппинг для "Задолженность по оплате ЖКУ"
    /// </summary>
    public class UtilityDebtorClaimWorkMap : JoinedSubClassMap<UtilityDebtorClaimWork>
    {
        public UtilityDebtorClaimWorkMap() : 
                base("Задолженность по оплате ЖКУ", "CLW_UTILITY_DEBTOR_CLAIM_WORK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AccountOwner, "Абонент").Column("ACCOUNT_OWNER").Length(150);
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").DefaultValue(OwnerType.Individual).NotNull();
            this.Property(x => x.ChargeDebt, "Сумма долга").Column("CHARGE_DEBT");
            this.Property(x => x.PenaltyDebt, "Сумма долга по пени").Column("PENALTY_DEBT");
            this.Property(x => x.PersonalAccountNum, "Номер лс").Column("ACCOUNT_NUM").Length(50);
            this.Property(x => x.PersonalAccountState, "Статус лс").Column("ACCOUNT_STATE").Length(150);
        }
    }
}
