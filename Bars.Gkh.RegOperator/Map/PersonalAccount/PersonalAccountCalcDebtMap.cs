namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>Маппинг для "Отмена оплаты"</summary>
    public class PersonalAccountCalcDebtMap : BaseEntityMap<PersonalAccountCalcDebt>
    {

        public PersonalAccountCalcDebtMap() :
            base("Расчет долга", "REGOP_PERS_ACC_CALC_DEBT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.PreviousOwner, "Прежний абонент").Column("PREV_OWNER_ID").NotNull().Fetch();
            this.Reference(x => x.Document, "Документ-основание").Column("DOCUMENT_ID").Fetch();
            this.Property(x => x.AgreementNumber, "Номер соглашения").Column("AGREEMENT_NUMBER");
            this.Property(x => x.DateStart, "Период с").Column("START_DATE").NotNull();
            this.Property(x => x.DateEnd, "Дата по").Column("END_DATE").NotNull();
        }
    }
}