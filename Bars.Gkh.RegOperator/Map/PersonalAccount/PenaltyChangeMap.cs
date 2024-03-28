/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class PenaltyChangeMap: BaseImportableEntityMap<PenaltyChange>
///     {
///         public PenaltyChangeMap()
///             : base("REGOP_PENALTY_CHANGE")
///         {
///             Map(x => x.CurrentValue, "CURRENT_VAL");
///             Map(x => x.NewValue, "NEW_VAL");
///             Map(x => x.Reason, "REASON");
///             Map(x => x.Guid, "GUID");
///             Map(x => x.TransferGuid, "C_GUID", false, length: 40);
///             References(x => x.AccountPeriodSummary, "SUMMARY_ID");
///             References(x => x.Account, "ACCOUNT_ID");
///             References(x => x.Document, "DOC_ID");
/// 
///             Bag(x => x.Operations, mapper =>
///             {
///                 mapper.Access(Accessor.NoSetter);
///                 mapper.Fetch(CollectionFetchMode.Select);
///                 mapper.Lazy(CollectionLazy.Lazy);
///                 mapper.Key(k =>
///                 {
///                     k.Column("C_GUID");
///                     k.PropertyRef(x => x.TransferGuid);
///                 });
///                 mapper.Cascade(Cascade.DeleteOrphans | Cascade.Persist);
///                 mapper.Inverse(true);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Изменение пени счета Сущность является источником для операции на кошельке ЛС по изменению пеней Операция может быть только одна и отмены данного вида операции не предполагается"</summary>
    public class PenaltyChangeMap : BaseImportableEntityMap<PenaltyChange>
    {
        
        public PenaltyChangeMap() : 
                base("Изменение пени счета Сущность является источником для операции на кошельке ЛС по " +
                        "изменению пеней Операция может быть только одна и отмены данного вида операции н" +
                        "е предполагается", "REGOP_PENALTY_CHANGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Счет").Column("ACCOUNT_ID");
            Reference(x => x.AccountPeriodSummary, "Суммарные значения по периоду для ЛС").Column("SUMMARY_ID");
            Property(x => x.CurrentValue, "Значение перед изменением").Column("CURRENT_VAL");
            Property(x => x.NewValue, "Новое значение").Column("NEW_VAL");
            Reference(x => x.Document, "Документ-основание для изменения").Column("DOC_ID");
            Property(x => x.Reason, "Причина изменения").Column("REASON").Length(250);
            Property(x => x.Guid, "Guid").Column("GUID").Length(250);
            Property(x => x.TransferGuid, "На этот гуид будут вязаться различные операции, происходящие при распределении. Н" +
                    "апример, возврат займа дома").Column("C_GUID").Length(40);
        }
    }

    public class PenaltyChangeNHibernateMapping : ClassMapping<PenaltyChange>
    {
        public PenaltyChangeNHibernateMapping()
        {
            Bag(
                x => x.Operations,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Key(
                            k =>
                                {
                                    k.Column("C_GUID");
                                    k.PropertyRef(x => x.TransferGuid);
                                });
                        mapper.Cascade(Cascade.DeleteOrphans | Cascade.Persist);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
