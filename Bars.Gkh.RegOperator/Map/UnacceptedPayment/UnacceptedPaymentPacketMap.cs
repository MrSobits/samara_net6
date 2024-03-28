/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class UnacceptedPaymentPacketMap : BaseImportableEntityMap<UnacceptedPaymentPacket>
///     {
///         public UnacceptedPaymentPacketMap()
///             : base("REGOP_UNACCEPT_PAY_PACKET")
///         {
///             Map(x => x.CreateDate, "CREATE_DATE", true);
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.Type, "TYPE");
///             Map(x => x.State, "STATE");
///             Map(x => x.Sum, "PACKET_SUM");
///             Map(x => x.TransferGuid, "TRANSFER_GUID");
///             Map(x => x.BankDocumentId, "BANK_DOC_ID");
/// 
///             //References(x => x.BankDocument, "BANK_DOC_ID", ReferenceMapConfig.Fetch);
/// 
///             Bag(x => x.Payments, m =>
///             {
///                 m.Key(k => k.Column("PACKET_ID"));
///                 m.Access(Accessor.NoSetter);
///                 m.Fetch(CollectionFetchMode.Select);
///                 m.Lazy(CollectionLazy.Lazy);
///                 m.Cascade(Cascade.All);
///                 m.Inverse(true);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Пакет неподтвержденных оплат"</summary>
    public class UnacceptedPaymentPacketMap : BaseImportableEntityMap<UnacceptedPaymentPacket>
    {
        
        public UnacceptedPaymentPacketMap() : 
                base("Пакет неподтвержденных оплат", "REGOP_UNACCEPT_PAY_PACKET")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CreateDate, "Дата создания").Column("CREATE_DATE").NotNull();
            Property(x => x.Description, "Описание пакета").Column("DESCRIPTION").Length(250);
            Property(x => x.Type, "Тип").Column("TYPE");
            Property(x => x.Sum, "Общая сумма по пакету оплат").Column("PACKET_SUM");
            Property(x => x.State, "Состояние").Column("STATE");
            Property(x => x.BankDocumentId, "BankDocumentId").Column("BANK_DOC_ID");
            Property(x => x.TransferGuid, "Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer").Column("TRANSFER_GUID").Length(250);
        }
    }

    public class UnacceptedPaymentPacketNHibernateMapping : ClassMapping<UnacceptedPaymentPacket>
    {
        public UnacceptedPaymentPacketNHibernateMapping()
        {
            Bag(
                x => x.Payments,
                m =>
                    {
                        m.Key(k => k.Column("PACKET_ID"));
                        m.Access(Accessor.NoSetter);
                        m.Fetch(CollectionFetchMode.Select);
                        m.Lazy(CollectionLazy.Lazy);
                        m.Cascade(Cascade.All);
                        m.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
