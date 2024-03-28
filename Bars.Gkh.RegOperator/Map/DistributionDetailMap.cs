namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    
    
    /// <summary>Маппинг для "Детализация распределения"</summary>
    public class DistributionDetailMap : BaseImportableEntityMap<DistributionDetail>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DistributionDetailMap() : 
                base("Детализация распределения", "REGOP_DISTR_DETAIL")
        {
        }
        
        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.EntityId, "Идентификатор сущности").Column("ENTITY_ID").NotNull();
            this.Property(x => x.Source, "Источник распределения").Column("DISTR_SOURCE").NotNull();
            this.Property(x => x.Object, "Получатель денег").Column("DOBJECT").Length(500);
            this.Property(x => x.PaymentAccount, "Р/С получателя").Column("PAYMENT_ACC");
            this.Property(x => x.Sum, "Сумма").Column("DSUM").NotNull();
            this.Property(x => x.Destination, "Назначение").Column("DESTINATION").Length(500);
            this.Property(x => x.UserLogin, "Логин пользователя").Column("USER_LOGIN").Length(100);
        }
    }
}
