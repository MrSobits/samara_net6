namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class LawsuitReferenceCalculationMap : BaseEntityMap<LawsuitReferenceCalculation>
    {
        public LawsuitReferenceCalculationMap() : base("Эталонный расчет даты начала задолженности", "REGOP_LAWSUIT_REFERENCE_CALCULATION") { }

        protected override void Map()
        {
            this.Property(x => x.AccountNumber, "Номер ЛС").Column("ACC_NUM");
            this.Property(x => x.AreaShare, "Доля собственности").Column("AREA_SHARE").NotNull();
            this.Property(x => x.BaseTariff, "Тариф").Column("BASE_TARIF").NotNull();
            this.Reference(x => x.PeriodId, "Период").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.PersonalAccountId, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Property(x => x.RoomArea, "Площадь помещения").Column("ROOM_AREA").NotNull();
            this.Property(x => x.TarifDebt, "Задолженность").Column("TARIF_DEBT").NotNull();
            this.Reference(x => x.Lawsuit, "Исковое заявление").Column("LAWSUIT_ID").NotNull();
            this.Property(x => x.TariffCharged, "Начислено").Column("TARIF_CHARGED").NotNull();
            this.Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            this.Property(x => x.TarifPayment, "Уплачено").Column("TARIF_PAYMENTS").NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }
    }
}