namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    public class PaymentImportMap : BaseImportableEntityMap<PaymentImport>
    {
        public PaymentImportMap() :
                base("Запись о импорте от ЧЭС", "REGOP_PAYMENTS_IMPORT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.PaymentDate, "Дата операции ").Column("PAYMENT_DATE").NotNull();
            this.Property(x => x.TariffDecisionPayment, "Сумма по тарифу решения").Column("DECISION_PAYMENT");
            this.Property(x => x.TariffPayment, "Сумма по базовому тарифу").Column("TARIFF_PAYMENT");
            this.Property(x => x.PenaltyPayment, "Сумма по пени").Column("PENALTY_PAYMENT");
            this.Property(x => x.PaymentType, "Тип оплаты").Column("PAYMENT_TYPE").NotNull();
            this.Property(x => x.RegistryNum, "Номер документа/реестра").Column("REGISTRY_NUM");
            this.Property(x => x.RegistryDate, "Дата документа/реестра").Column("REGISTRY_DATE");
            this.Reference(x => x.BasePersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.PaymentOperation, "Базовая сущность операции оплат").Column("PAYMENT_OP_ID").Fetch();
        }
    }
}