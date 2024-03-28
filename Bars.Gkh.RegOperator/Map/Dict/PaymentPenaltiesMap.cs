/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.Dict;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class PaymentPenaltiesMap : BaseImportableEntityMap<PaymentPenalties>
///     {
///         public PaymentPenaltiesMap()
///             : base("REGOP_PAYMENT_PENALTIES")
///         {
///             Map(x => x.Days, "DAYS");
///             Map(x => x.Percentage, "PERCENTAGE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.DecisionType, "DECISION_TYPE");
/// 
///             Bag(x => x.Excludes, m =>
///             {
///                 m.Access(Accessor.NoSetter);
///                 m.Fetch(CollectionFetchMode.Select);
///                 m.Cascade(Cascade.Remove);
///                 m.Lazy(CollectionLazy.Extra);
///                 m.Key(k => k.Column("PAY_PENALTIES_ID"));
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Справочник расчетов пеней"</summary>
    public class PaymentPenaltiesMap : BaseImportableEntityMap<PaymentPenalties>
    {
        
        public PaymentPenaltiesMap() : 
                base("Справочник расчетов пеней", "REGOP_PAYMENT_PENALTIES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Days, "Количество дней").Column("DAYS");
            Property(x => x.Percentage, "Ставка рефинансирования, %").Column("PERCENTAGE");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.DecisionType, "Способ формирования").Column("DECISION_TYPE");
        }
    }

    public class PaymentPenaltiesNHibernateMapping : ClassMapping<PaymentPenalties>
    {
        public PaymentPenaltiesNHibernateMapping()
        {
            Bag(
                x => x.Excludes,
                m =>
                    {
                        m.Access(Accessor.NoSetter);
                        m.Fetch(CollectionFetchMode.Select);
                        m.Cascade(Cascade.Remove);
                        m.Lazy(CollectionLazy.Extra);
                        m.Key(k => k.Column("PAY_PENALTIES_ID"));
                    },
                action => action.OneToMany());
        }
    }
}
