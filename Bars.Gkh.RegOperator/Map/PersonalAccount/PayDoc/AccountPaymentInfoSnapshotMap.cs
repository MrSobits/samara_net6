namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    
    
    /// <summary>Маппинг для "Данные для документа на оплату по ЛС"</summary>
    public class AccountPaymentInfoSnapshotMap : BaseEntityMap<AccountPaymentInfoSnapshot>
    {
        
        public AccountPaymentInfoSnapshotMap() : 
                base("Данные для документа на оплату по ЛС", "REGOP_PERS_PAYDOC_SNAP")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Snapshot, "Основная информация по документу").Column("SNAPSHOT_ID");
            this.Property(x => x.AccountId, "Id ЛС").Column("ACCOUNT_ID");
            this.Property(x => x.Data, "Данные для документа").Column("RAW_DATA").Length(250);
            this.Property(x => x.AccountNumber, "Номер ЛС").Column("ACC_NUM").Length(250);
            this.Property(x => x.RoomAddress, "Адрес до уровня квартиры. Т.е. Казань, Гаврилова 13, кв. 5").Column("ROOM_ADDRESS").Length(250);
            this.Property(x => x.RoomType, "Тип комнаты").Column("ROOM_TYPE");
            this.Property(x => x.Area, "Площадь помещения").Column("AREA");
            this.Property(x => x.Tariff, "Тариф").Column("TARIFF");
            this.Property(x => x.ChargeSum, "Сумма начисления Складывается из BaseTariffSum, DecisionTariffSum, PenaltySum").Column("CHARGE_SUM");
            this.Property(x => x.BaseTariffSum, "Служебное- К оплате по базовому тарифу").Column("BASE_TARIFF_SUM").NotNull();
            this.Property(x => x.DecisionTariffSum, "Служебное- К оплате по тарифу решения").Column("DEC_TARIFF_SUM").NotNull();
            this.Property(x => x.PenaltySum, "Служебное- Пени к оплате").Column("PENALTY_SUM").NotNull();
            this.Property(x => x.Services, "Оказанные услуги (в нашем случае пока только кап ремонт)").Column("SERVICES").Length(250);
        }
    }
}
