namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Счет оплат дома"</summary>
    public class RealityObjectPaymentAccountMap : BaseImportableEntityMap<RealityObjectPaymentAccount>
    {
        
        public RealityObjectPaymentAccountMap() : 
                base("Счет оплат дома", "REGOP_RO_PAYMENT_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.AccountNumber, "Номер счета").Column("ACC_NUM").Length(250);
            this.Property(x => x.DateOpen, "Дата открытия счета").Column("CDATE_OPEN").NotNull();
            this.Property(x => x.DateClose, "Дата закрытия счета").Column("CDATE_CLOSE");
            this.Reference(x => x.RealityObject, "Объект недвижимости").Column("RO_ID").NotNull().Fetch();
            this.Property(x => x.AccountType, "Тип счета").Column("CACC_TYPE").NotNull();
            this.Property(x => x.DebtTotal, "итого по дебету").Column("DEBT_TOTAL");
            this.Property(x => x.CreditTotal, "Итого по кредиту").Column("CREDIT_TOTAL");
            this.Property(x => x.MoneyLocked, "Заблокировано денег").Column("MONEY_LOCKED");
            this.Property(x => x.Loan, "Сумма непогашенных займов").Column("LOAN");
            this.Reference(x => x.BaseTariffPaymentWallet, "Кошелек оплат по базовому тарифу").Column("BT_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.DecisionPaymentWallet, "Кошелек оплат по тарифу решения").Column("DT_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.RentWallet, "Кошелек оплат по аренде").Column("R_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.PenaltyPaymentWallet, "Кошелек оплат по пени").Column("P_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.SocialSupportWallet, "Кошелек оплат по соц поддержке").Column("SS_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.PreviosWorkPaymentWallet, "Кошелек оплат за выполненные работы").Column("PWP_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.AccumulatedFundWallet, "Кошелек по ранее накопленным средствам").Column("AF_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.RestructAmicableAgreementWallet, "Кошелек оплат по мировому соглашению").Column("RAA_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.TargetSubsidyWallet, "Кошелек целевых субсидий").Column("TSU_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.FundSubsidyWallet, "Кошелек субсидий фонда").Column("FSU_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.RegionalSubsidyWallet, "Кошелек региональных субсидий").Column("RSU_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.StimulateSubsidyWallet, "Кошелек стимулирующей субсидий").Column("SSU_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.OtherSourcesWallet, "Кошелек иных поступлений").Column("OS_WALLET_ID").NotNull().Fetch();
            this.Reference(x => x.BankPercentWallet, "Кошелек процентов банка").Column("BP_WALLET_ID").NotNull().Fetch();
        }
    }
}
