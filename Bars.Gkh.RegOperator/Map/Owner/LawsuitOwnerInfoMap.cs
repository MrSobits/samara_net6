namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities.Owner;

    public class LawsuitOwnerInfoMap : BaseEntityMap<LawsuitOwnerInfo>
    {
        public LawsuitOwnerInfoMap() :  base("Собственник в исковом заявлении", "REGOP_LAWSUIT_OWNER_INFO"){}

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование собственника").Column("NAME").NotNull();
            this.Property(x => x.OwnerType, "Тип собственника").Column("OWNER_TYPE").NotNull();
            this.Property(x => x.AreaShareNumerator, "Числитель новой доли собственности").Column("AREA_SHARE_NUM").NotNull();
            this.Property(x => x.AreaShareDenominator, "Знаменатель новой доли собственности").Column("AREA_SHARE_DEN").NotNull();
            this.Property(x => x.DebtBaseTariffSum, "Новая задолженность по базовому тарифу").Column("DEBT_BASE_TARIFF_SUM").NotNull();
            this.Property(x => x.DebtDecisionTariffSum, "Новая задолженность по тарифу решения").Column("DEBT_DECISION_TARIFF_SUM").NotNull();
            this.Property(x => x.PenaltyDebt, "Новая задолженность по пени").Column("PENALTY_DEBT").NotNull();
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("PERSONAL_ACCOUNT_ID").NotNull();
            this.Reference(x => x.StartPeriod, "Период с").Column("START_PERIOD_ID").NotNull();
            this.Reference(x => x.EndPeriod, "Пероид по").Column("END_PERIOD_ID").NotNull();
            this.Reference(x => x.Lawsuit, "Исковое заявление").Column("LAWSUIT_ID").NotNull();
            this.Reference(x => x.JurInstitution, "Суд").Column("JURINST_ID");
            this.Property(x => x.SharedOwnership, "Совместная собвстенник").Column("SHARED_OWNERSHIP");
            this.Property(x => x.Underage, "Несовершеннолетний").Column("UNDERAGE");
            this.Property(x => x.ClaimNumber, "Номер заявляния").Column("CLAIM_NUMBER");
            this.Property(x => x.SNILS, "СНИЛС").Column("SNILS");
        }
    }
}
